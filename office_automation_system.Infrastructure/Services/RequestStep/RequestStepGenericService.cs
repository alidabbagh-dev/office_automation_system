using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Dto.RequestStep;
using office_automation_system.application.Validator.RequestStep;
using office_automation_system.domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace office_automation_system.Infrastructure.Services.RequestStep
{
    public class RequestStepGenericService : IRequestStepGenericService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<office_automation_system.domain.Entities.ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly CreateRequestStepDtoValidator _createValidator;
        private readonly EditRequestStepDtoValidator _editValidator;


        public RequestStepGenericService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            UserManager<office_automation_system.domain.Entities.ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            CreateRequestStepDtoValidator createValidator,
            EditRequestStepDtoValidator editValidator

            )

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _createValidator = createValidator;
            _editValidator = editValidator;
            
        }


        //this method checks that is user allowed to edit step or not
        public async Task<bool> CheckIsVerified(Guid RequestStepId)
        {
            var userObj = _httpContextAccessor?.HttpContext?.User;
            var userIdString = userObj?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIdGuid = Guid.Parse(userIdString);
            var user = await _userManager.FindByIdAsync(userIdGuid.ToString());
            if (user == null)
                return false;

            var roleNames = await _userManager.GetRolesAsync(user);
            var firstRoleName = roleNames.FirstOrDefault();

            if (firstRoleName == null)
                return false;


            var role = await _roleManager.FindByNameAsync(firstRoleName);
            var roleId = role?.Id;

            var RequestStep = await GetByIdAsync(RequestStepId);
            if (RequestStep == null) return false;
            if((RequestStep?.OwnerId == userIdGuid || RequestStep?.RoleId == roleId) && RequestStep?.IsCurrentStep == true)
            {
                return true;
            }

         
            return false;
        }


        public async Task<int> CheckStepPosition(Guid RequestStepId, Guid RequestId)
        {
            var result = await FindAsync(p =>
             p.IsDeleted == false &&
             (p.RequestId == RequestId)
            );

            var OrderedResult = result.OrderBy(p => p.Order);
            int count = 0;
            foreach (var step in OrderedResult)
            {
                if (step?.Id == RequestStepId && count == 0)
                {
                    return 1;
                }
                else if (step?.Id == RequestStepId && count != 0 && count != result.Count - 1)
                {
                    return 2;
                }

                else if(step?.Id == RequestStepId && count == result.Count -1 )
                {
                    return 3;
                }
                count++;
            }

            return -1;
        }
        public async Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateRequestStepDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _createValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }

            var RequestStep = _mapper.Map<office_automation_system.domain.Entities.RequestStep>(dto);
            await _unitOfWork.RequestSteps.AddAsync(RequestStep);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditRequestStepDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _editValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }

            var IsVerified = await CheckIsVerified(id);
            if (IsVerified == false)
            {
                return (false, ["you are not allowed to edit this request step"]);
            }
            var RequestStep = await _unitOfWork.RequestSteps.GetByIdAsync(id);
            if (RequestStep == null) return (false, new List<string> { "RequestStep Not Found with this id" });

            _mapper.Map(dto, RequestStep);
            _unitOfWork.RequestSteps.Update(RequestStep);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }

        public async Task<(bool IsSuccess, List<string> Errors)> ReferToPreviousStep(Guid RequestStepId)
        {
            var IsVerified = await CheckIsVerified(RequestStepId);
            if(!IsVerified)
            {
                return (false, ["You are not allowed to Refer this Step back"]);
            }

            var RequestStep = await GetByIdAsync(RequestStepId);
            var RequestId = RequestStep?.RequestId;
            var StepPosition = await CheckStepPosition(RequestStepId, (Guid)RequestId);
            if(StepPosition == 2 || StepPosition == 3)
            {
                var result = await FindAsync(p =>
                    p.IsDeleted == false &&
                    (p.RequestId == RequestId)
                );

                var OrderedResult = result.OrderBy(p => p.Order).ToList();
                var currentStep = OrderedResult.FirstOrDefault(p => p.Id == RequestStepId);

                if (currentStep == null)
                {
                    
                    return (false, ["Could not refer back the step"]);
                }

                
                var previousStep = OrderedResult
                    .Where(p => p.Order < currentStep?.Order)
                    .OrderByDescending(p => p.Order)
                    .FirstOrDefault();

                foreach(var step in result)
                {
                    if(step?.Id == previousStep?.Id)
                    {
                        var RequestStepRow = await _unitOfWork.RequestSteps.GetByIdAsync(step.Id);

                        var EditRequestStepDto = new EditRequestStepDto();
                        EditRequestStepDto.Description = step?.Description;
                        EditRequestStepDto.IsCurrentStep = true;
                        EditRequestStepDto.UpdatedAt = step?.UpdatedAt;
                        EditRequestStepDto.CreatedAt = step?.CreatedAt;
                        _mapper.Map(EditRequestStepDto, RequestStepRow);
                        _unitOfWork.RequestSteps.Update(RequestStepRow);
                        await _unitOfWork.CompleteAsync();
                        
                    } else
                    {
                        var RequestStepRow = await _unitOfWork.RequestSteps.GetByIdAsync(step.Id);

                        var EditRequestStepDto = new EditRequestStepDto();
                        EditRequestStepDto.Description = step?.Description;
                        EditRequestStepDto.IsCurrentStep = false;
                        EditRequestStepDto.UpdatedAt = step?.UpdatedAt;
                        EditRequestStepDto.CreatedAt = step?.CreatedAt;
                        _mapper.Map(EditRequestStepDto, RequestStepRow);
                        _unitOfWork.RequestSteps.Update(RequestStepRow);
                        await _unitOfWork.CompleteAsync();
                        
                    }
                }

                var Request = await _unitOfWork.Requests.GetByIdAsync((Guid) RequestId);
                Request.Status = RequestStatus.Reverted;
                _unitOfWork.Requests.Update(Request);
                await _unitOfWork.CompleteAsync();
                

                return (true, ["Step referred back"]);
                

            }
            else
            {
                return (false, ["Could not Refer back the step"]);
            }
        }

        public async Task<(bool IsSuccess, List<string> Errors)> ReferToNextStep(Guid RequestStepId)
        {
            var IsVerified = await CheckIsVerified(RequestStepId);
            if (!IsVerified)
            {
                return (false, ["You are not allowed to Refer this Step to next"]);
            }

            var RequestStep = await GetByIdAsync(RequestStepId);
            var RequestId = RequestStep?.RequestId;
            var StepPosition = await CheckStepPosition(RequestStepId, (Guid)RequestId);

            if (StepPosition == 1 || StepPosition == 2)
            {
                var result = await FindAsync(p =>
                    p.IsDeleted == false &&
                    (p.RequestId == RequestId)
                );

                var OrderedResult = result.OrderBy(p => p.Order).ToList();
                var currentStep = OrderedResult.FirstOrDefault(p => p.Id == RequestStepId);

                if (currentStep == null)
                {

                    return (false, ["Could not refer back the step"]);
                }

                var NextStep = OrderedResult
                    .Where(p => p.Order > currentStep.Order)
                    .FirstOrDefault();


                foreach(var step in result)
                {
                    if(step?.Id == NextStep?.Id)
                    {
                        var RequestStepRow = await _unitOfWork.RequestSteps.GetByIdAsync(step.Id);

                        var EditRequestStepDto = new EditRequestStepDto();
                        EditRequestStepDto.Description = step?.Description;
                        EditRequestStepDto.IsCurrentStep = true;
                        EditRequestStepDto.UpdatedAt = step?.UpdatedAt;
                        EditRequestStepDto.CreatedAt = step?.CreatedAt;
                        _mapper.Map(EditRequestStepDto, RequestStepRow);
                        _unitOfWork.RequestSteps.Update(RequestStepRow);
                        await _unitOfWork.CompleteAsync();
                    } 
                    else
                    {
                        var RequestStepRow = await _unitOfWork.RequestSteps.GetByIdAsync(step.Id);

                        var EditRequestStepDto = new EditRequestStepDto();
                        EditRequestStepDto.Description = step?.Description;
                        EditRequestStepDto.IsCurrentStep = false;
                        EditRequestStepDto.UpdatedAt = step?.UpdatedAt;
                        EditRequestStepDto.CreatedAt = step?.CreatedAt;
                        _mapper.Map(EditRequestStepDto, RequestStepRow);
                        _unitOfWork.RequestSteps.Update(RequestStepRow);
                        await _unitOfWork.CompleteAsync();
                    }
                }

                var Request = await _unitOfWork.Requests.GetByIdAsync((Guid)RequestId);

                Request.Status = RequestStatus.Pending;
                _unitOfWork.Requests.Update(Request);
                await _unitOfWork.CompleteAsync();
                return (true, ["Current Step referred to the next step"]);


            } else
            {
                return (false, ["Could not refer to the next step"]);
            }

        }

        public async Task<(bool IsSuccess, List<string> Errors)> ConfirmFinalStep(Guid RequestStepId)
        {
            var IsVerified = await CheckIsVerified(RequestStepId);
            if (!IsVerified)
            {
                return (false, ["You are not allowed to Refer this Step to next"]);
            }

            var RequestStep = await GetByIdAsync(RequestStepId);
            var RequestId = RequestStep?.RequestId;
            var StepPosition = await CheckStepPosition(RequestStepId, (Guid)RequestId);

            if(StepPosition == 3)
            {
                var result = await FindAsync(p =>
                   p.IsDeleted == false &&
                   (p.RequestId == RequestId)
               );

                foreach(var step in result)
                {
                    var RequestStepRow = await _unitOfWork.RequestSteps.GetByIdAsync(step.Id);

                    var EditRequestStepDto = new EditRequestStepDto();
                    EditRequestStepDto.Description = step?.Description;
                    EditRequestStepDto.IsCurrentStep = false;
                    EditRequestStepDto.UpdatedAt = step?.UpdatedAt;
                    EditRequestStepDto.CreatedAt = step?.CreatedAt;
                    _mapper.Map(EditRequestStepDto, RequestStepRow);
                    _unitOfWork.RequestSteps.Update(RequestStepRow);
                    await _unitOfWork.CompleteAsync();
                }

                var Request = await _unitOfWork.Requests.GetByIdAsync((Guid)RequestId);

                Request.Status = RequestStatus.Archived;
                _unitOfWork.Requests.Update(Request);

                await _unitOfWork.CompleteAsync();


                return (true, ["Final Step Has Been Confirmed"]);
            } 
            else
            {
                return (false, ["Could Not Confirm Final Step"]);
            }

        }


        public async Task<List<GetRequestStepDto>> GetAllAsync()
        {
            var RequestSteps = await _unitOfWork.RequestSteps.GetAllAsync();
            return _mapper.Map<List<GetRequestStepDto>>(RequestSteps);
        }



        public async Task<GetRequestStepDto?> GetByIdAsync(Guid id)
        {
            var RequestStep = await _unitOfWork.RequestSteps.GetByIdAsync(id);
            return RequestStep == null ? null : _mapper.Map<GetRequestStepDto>(RequestStep);
        }


        public async Task<List<GetRequestStepDto>> FindAsync(
        Expression<Func<office_automation_system.domain.Entities.RequestStep, bool>> predicate)
        {
            // 1. Use the generic repository to query entities
            var RequestSteps = await _unitOfWork.RequestSteps
                .FindAsync(predicate);

            // 2. Convert entities to DTOs using AutoMapper
            var dtos = _mapper.Map<List<GetRequestStepDto>>(RequestSteps);

            // 3. Return the DTO list
            return dtos;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var RequestStep = await _unitOfWork.RequestSteps.GetByIdAsync(id);
            if (RequestStep == null) return false;

            _unitOfWork.RequestSteps.Remove(RequestStep);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var RequestStep = await _unitOfWork.RequestSteps.GetByIdAsync(id);
            if (RequestStep == null)
            {
                return false;
            }

            RequestStep.IsDeleted = true;
            _unitOfWork.RequestSteps.Update(RequestStep);
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }
}
