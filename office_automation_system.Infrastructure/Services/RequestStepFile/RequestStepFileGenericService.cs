using AutoMapper;
using office_automation_system.application.Contracts.Services.FileManager;
using office_automation_system.application.Contracts.Services.RequestStepFile;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Dto.RequestStepFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Services.RequestStepFile
{
    public class RequestStepFileGenericService : IRequestStepFileGenericService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileManagerGenericService _FileManagerGenericService;


        public RequestStepFileGenericService(
            IUnitOfWork unitOfWork,
            IMapper mapper,IFileManagerGenericService fileManagerGenericService)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _FileManagerGenericService = fileManagerGenericService;
        }



        public async Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateRequestStepFileDto dto)
        {
            //ValidationResult validation = await _createValidator.ValidateAsync(dto);
            //if (!validation.IsValid)
            //    return (false, validation.Errors.Select(e => e.ErrorMessage).ToList());

            if (dto.File != null) {
                await _FileManagerGenericService.UploadFileAsync(dto.File, "requestStepFiles");
            }

            var RequestStepFile = _mapper.Map<office_automation_system.domain.Entities.RequestStepFile>(dto);
            await _unitOfWork.RequestStepFiles.AddAsync(RequestStepFile);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditRequestStepFileDto dto)
        {
            //ValidationResult validation = await _editValidator.ValidateAsync(dto);
            //if (!validation.IsValid)
            //    return (false, validation.Errors.Select(e => e.ErrorMessage).ToList());
            if (dto.File != null)
            {
                await _FileManagerGenericService.UploadFileAsync(dto.File, "requestStepFiles");
            }

            var RequestStepFile = await _unitOfWork.RequestStepFiles.GetByIdAsync(id);
            if (RequestStepFile == null) return (false, new List<string> { "RequestStepFile Not Found with this id" });

            _mapper.Map(dto, RequestStepFile);
            _unitOfWork.RequestStepFiles.Update(RequestStepFile);
            await _unitOfWork.CompleteAsync();
            return (true, []);
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


        public async Task<bool> DeleteAsync(Guid id)
        {
            var RequestStepFile = await _unitOfWork.RequestStepFiles.GetByIdAsync(id);
            if (RequestStepFile == null) return false;

            _unitOfWork.RequestStepFiles.Remove(RequestStepFile);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var RequestStepFile = await _unitOfWork.RequestStepFiles.GetByIdAsync(id);
            if (RequestStepFile == null)
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
