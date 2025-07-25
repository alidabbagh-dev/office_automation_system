using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using office_automation_system.application.Contracts.Services.FileManager;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Contracts.Services.RequestStepFile;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Dto.RequestStepFile;
using office_automation_system.application.Validator.RequestStepFile;
using office_automation_system.Infrastructure.Services.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Services.RequestStepFile
{
    public class RequestStepFileGenericService : IRequestStepFileGenericService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileManagerGenericService _FileManagerGenericService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<office_automation_system.domain.Entities.ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        private readonly IRequestStepGenericService _requestStepGenericService;
        private readonly CreateRequestStepFileDtoValidator _createValidator;


        public RequestStepFileGenericService(
            IUnitOfWork unitOfWork,
            IMapper mapper,IFileManagerGenericService fileManagerGenericService, IHttpContextAccessor httpContextAccessor,
            UserManager<office_automation_system.domain.Entities.ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager, IRequestStepGenericService requestStepGenericService,
            CreateRequestStepFileDtoValidator createValidator
            )

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _FileManagerGenericService = fileManagerGenericService;
            _requestStepGenericService = requestStepGenericService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _createValidator = createValidator;
        }



        //this method checks that is user allowed to create or delete step attachment file or not
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

            var RequestStep = await _requestStepGenericService.GetByIdAsync(RequestStepId);
            if (RequestStep == null) return false;
            if ((RequestStep?.OwnerId == userIdGuid || RequestStep?.RoleId == roleId) && RequestStep?.IsCurrentStep == true)
            {
                return true;
            }


            return false;
        }



        public async Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateRequestStepFileDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _createValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }

            var IsVerified = await CheckIsVerified(dto.RequestStepId.GetValueOrDefault());
            if(!IsVerified)
            {
                return (false, ["You are not allowed to create attachment file for this step"]);
            }

            if (dto.File != null) {
                var path = await _FileManagerGenericService.UploadFileAsync(dto.File, "requestStepFiles");
                dto.FileUrl = path;
                var RequestStepFile = _mapper.Map<office_automation_system.domain.Entities.RequestStepFile>(dto);
                await _unitOfWork.RequestStepFiles.AddAsync(RequestStepFile);
                await _unitOfWork.CompleteAsync();
                return (true, []);
            } else
            {
                return (false, ["Could not Upload Attachment File"]);
            }

            
           
        }




        public async Task<List<GetRequestStepFileDto>> GetAllAsync()
        {
            var RequestStepFiles = await _unitOfWork.RequestStepFiles.GetAllAsync();
            return _mapper.Map<List<GetRequestStepFileDto>>(RequestStepFiles);
        }



        public async Task<GetRequestStepFileDto?> GetByIdAsync(Guid id)
        {
            var RequestStepFile = await _unitOfWork.RequestStepFiles.GetByIdAsync(id);
            return RequestStepFile == null ? null : _mapper.Map<GetRequestStepFileDto>(RequestStepFile);
        }


        public async Task<List<GetRequestStepFileDto>> FindAsync(
        Expression<Func<office_automation_system.domain.Entities.RequestStepFile, bool>> predicate)
        {
            // 1. Use the generic repository to query entities
            var RequestStepFiles = await _unitOfWork.RequestStepFiles
                .FindAsync(predicate);

            // 2. Convert entities to DTOs using AutoMapper
            var dtos = _mapper.Map<List<GetRequestStepFileDto>>(RequestStepFiles);

            // 3. Return the DTO list
            return dtos;
        }


    

        public async Task<bool> SoftDeleteAsync(Guid RequestStepFileId)
        {

           
            var RequestStepFile = await _unitOfWork.RequestStepFiles.GetByIdAsync(RequestStepFileId);
            if (RequestStepFile == null)
            {
                return false;
            }



            var RequestStep = await _requestStepGenericService.GetByIdAsync((Guid) RequestStepFile.RequestStepId);
            var RequestStepId = RequestStep?.Id;

            var IsVerified = await CheckIsVerified((Guid)RequestStepId);
            if (!IsVerified)
            {
                return false;
            }

            RequestStepFile.IsDeleted = true;
            _unitOfWork.RequestStepFiles.Update(RequestStepFile);
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }
}
