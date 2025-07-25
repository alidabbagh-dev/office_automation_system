using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using office_automation_system.application.Contracts.Services.AdministrativeProcess;
using office_automation_system.application.Contracts.Services.ProcessApprovalStep;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Dto.Request;
using office_automation_system.application.Dto.RequestStep;
using office_automation_system.application.Validator.Request;
using office_automation_system.domain.Enums;
using office_automation_system.Infrastructure.Services.ProcessApprovalStep;
using office_automation_system.Infrastructure.Services.RequestStep;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace office_automation_system.Infrastructure.Services.Request
{
    public class RequestGenericService : IRequestGenericService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProcessApprovalStepGenericService _processApprovalStepGenericService;
        private readonly IRequestStepGenericService _requestStepGenericService;
        private readonly UserManager<office_automation_system.domain.Entities.ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CreateRequestDtoValidator _createValidator;
        private readonly EditRequestDtoValidator _editValidator;

        public RequestGenericService(
            IUnitOfWork unitOfWork,
            IMapper mapper,IProcessApprovalStepGenericService processApprovalStepGenericService
            ,UserManager<office_automation_system.domain.Entities.ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IHttpContextAccessor httpContextAccessor,
            IRequestStepGenericService requestStepGenericService,
            CreateRequestDtoValidator createValidator,
            EditRequestDtoValidator editValidator
            )

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _processApprovalStepGenericService = processApprovalStepGenericService;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _requestStepGenericService = requestStepGenericService;
            _createValidator = createValidator;
            _editValidator = editValidator;
        }

        //this method checks if a user can read a request from database or not
        public async Task<bool> IsUserAllowedToAccessRequest(Guid RequestId)
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

            var Request = await GetByIdAsync(RequestId);
            if(Request == null) return false;

            if(Request.UserId == userIdGuid)
            {
                return true;
            }

            var RequestSteps = await _requestStepGenericService.FindAsync(p => (p.RequestId == RequestId)
                && ((p.OwnerId == userIdGuid) || (p.RoleId == roleId))
            );

            if (RequestSteps == null || !RequestSteps.Any()) { 
                return false; 
            
            } 

            return true;

        }


        //this method checks a user can create a request or not
        public async Task<bool> CheckIsVerified(CreateRequestDto dto)
        {
            var result = await _processApprovalStepGenericService.FindAsync(p =>
                 p.IsDeleted == false && p.AdministrativeProcessId == dto.ProcessId);

            if(result == null || result.Count == 0  )
            {
                return false;
            }

            var orderedResult = result.OrderBy(r => r.Order).ToList();
            var firstStep = orderedResult.FirstOrDefault();
            var userObj = _httpContextAccessor?.HttpContext?.User;
            var userIdString = userObj?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userIdGuid = Guid.Parse(userIdString);
            if(userIdGuid != dto?.UserId)
            {
                return false;
            }
            var user = await _userManager.FindByIdAsync(userIdGuid.ToString());
            if (user == null)
                return false;

            var roleNames = await _userManager.GetRolesAsync(user);
            var firstRoleName = roleNames.FirstOrDefault();

            if (firstRoleName == null)
                return false;

            
            var role = await _roleManager.FindByNameAsync(firstRoleName);
            var roleId = role?.Id;

            if(firstStep?.OwnerId == userIdGuid || firstStep?.RoleId == roleId)
            {
                return true;
            }


            return false;


        }
        public async Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateRequestDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _createValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }


            var IsVerified = await CheckIsVerified(dto); 
            if(IsVerified == false)
            {
                return (false, ["you are not allowed to make this request"]);
            }

            dto.Status = RequestStatus.Sent;
            
            var Request = _mapper.Map<office_automation_system.domain.Entities.Request>(dto);
            await _unitOfWork.Requests.AddAsync(Request);
            await _unitOfWork.CompleteAsync();

            var result = await _processApprovalStepGenericService.FindAsync(p =>
                p.IsDeleted == false && p.AdministrativeProcessId == dto.ProcessId);

            var orderedResult = result.OrderBy(r => r.Order).ToList();


            int counter = 0;
            bool IsCurrentStep;
            foreach (var processApprovalStep in orderedResult) 
            {
                if(counter == 0)
                {
                    IsCurrentStep = true;
                } else
                {
                    IsCurrentStep = false;
                }

                var CreateRequestStepDto = new CreateRequestStepDto();
                CreateRequestStepDto.RequestId = Request?.Id;
                CreateRequestStepDto.Order = processApprovalStep?.Order;
                CreateRequestStepDto.IsCurrentStep = IsCurrentStep;
                CreateRequestStepDto.OwnerId = processApprovalStep?.OwnerId;
                CreateRequestStepDto.RoleId = processApprovalStep?.RoleId;
                CreateRequestStepDto.Title = processApprovalStep?.StepTitle;
                CreateRequestStepDto.Description = null;
                CreateRequestStepDto.CreatedAt = DateTime.UtcNow;
                CreateRequestStepDto.UpdatedAt = DateTime.UtcNow;

                var RequestStep = _mapper.Map<office_automation_system.domain.Entities.RequestStep>(CreateRequestStepDto);
                await _unitOfWork.RequestSteps.AddAsync(RequestStep);
                await _unitOfWork.CompleteAsync();
                counter++;
            }

            return (true, []);
        }


        public async Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditRequestDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _editValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }

            var Request = await _unitOfWork.Requests.GetByIdAsync(id);
            if (Request == null) return (false, new List<string> { "Request Not Found with this id" });

            _mapper.Map(dto, Request);
            _unitOfWork.Requests.Update(Request);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<List<GetRequestDto>> GetAllAsync()
        {
            var Requests = await _unitOfWork.Requests.GetAllAsync();
            return _mapper.Map<List<GetRequestDto>>(Requests);
        }

        /*this method returns requests which are related to user requests which he made or 
         * he is in one of request steps*/
        public async Task<List<GetRequestDto>> GetUserRelatedRequestsAsync()
        {
            var AllRequests = await GetAllAsync();
            if(AllRequests == null || !AllRequests.Any())
            {
                return new List<GetRequestDto>();
            }


            var UserRelatedRequests = new List<GetRequestDto>();
            foreach(var Request in AllRequests)
            {
                var IsUserAllowed = await IsUserAllowedToAccessRequest(Request.Id);
                if(IsUserAllowed)
                {
                    UserRelatedRequests.Add(Request);
                }
            }

            return UserRelatedRequests;

        }



        public async Task<GetRequestDto?> GetByIdAsync(Guid id)
        {
            var Request = await _unitOfWork.Requests.GetByIdAsync(id);
            return Request == null ? null : _mapper.Map<GetRequestDto>(Request);
        }


        public async Task<List<GetRequestDto>> FindAsync(
        Expression<Func<office_automation_system.domain.Entities.Request, bool>> predicate)
        {
            // 1. Use the generic repository to query entities
            var Requests = await _unitOfWork.Requests
                .FindAsync(predicate);

            // 2. Convert entities to DTOs using AutoMapper
            var dtos = _mapper.Map<List<GetRequestDto>>(Requests);

            // 3. Return the DTO list
            return dtos;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var Request = await _unitOfWork.Requests.GetByIdAsync(id);
            if (Request == null) return false;

            _unitOfWork.Requests.Remove(Request);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var Request = await _unitOfWork.Requests.GetByIdAsync(id);
            if (Request == null)
            {
                return false;
            }
            if(Request?.Status == RequestStatus.Sent)
            {
                Request.IsDeleted = true;
                _unitOfWork.Requests.Update(Request);
                await _unitOfWork.CompleteAsync();
                var result = await _requestStepGenericService.FindAsync(p =>
                 p.RequestId == id);

                foreach (var requestStep in result) {
                    await _requestStepGenericService.SoftDeleteAsync(requestStep.Id);
                }
                return true;
            }

            return false;
        }

    }
}
