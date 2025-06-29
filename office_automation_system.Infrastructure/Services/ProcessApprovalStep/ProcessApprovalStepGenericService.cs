using AutoMapper;
using office_automation_system.application.Contracts.Services.ProcessApprovalStep;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Dto.ProcessApprovalStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Services.ProcessApprovalStep
{
    public class ProcessApprovalStepGenericService : IProcessApprovalStepGenericService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;



        public ProcessApprovalStepGenericService(
            IUnitOfWork unitOfWork,
            IMapper mapper)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public async Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateProcessApprovalStepDto dto)
        {
            //ValidationResult validation = await _createValidator.ValidateAsync(dto);
            //if (!validation.IsValid)
            //    return (false, validation.Errors.Select(e => e.ErrorMessage).ToList());

            var ProcessApprovalStep = _mapper.Map<office_automation_system.domain.Entities.ProcessApprovalStep>(dto);
            await _unitOfWork.ProcessApprovalSteps.AddAsync(ProcessApprovalStep);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditProcessApprovalStepDto dto)
        {
            //ValidationResult validation = await _editValidator.ValidateAsync(dto);
            //if (!validation.IsValid)
            //    return (false, validation.Errors.Select(e => e.ErrorMessage).ToList());

            var ProcessApprovalStep = await _unitOfWork.ProcessApprovalSteps.GetByIdAsync(id);
            if (ProcessApprovalStep == null) return (false, new List<string> { "ProcessApprovalStep Not Found with this id" });

            _mapper.Map(dto, ProcessApprovalStep);
            _unitOfWork.ProcessApprovalSteps.Update(ProcessApprovalStep);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<List<GetProcessApprovalStepDto>> GetAllAsync()
        {
            var ProcessApprovalSteps = await _unitOfWork.ProcessApprovalSteps.GetAllAsync();

            return _mapper.Map<List<GetProcessApprovalStepDto>>(ProcessApprovalSteps.OrderBy(p => p.Order));
        }



        public async Task<GetProcessApprovalStepDto?> GetByIdAsync(Guid id)
        {
            var ProcessApprovalStep = await _unitOfWork.ProcessApprovalSteps.GetByIdAsync(id);
            return ProcessApprovalStep == null ? null : _mapper.Map<GetProcessApprovalStepDto>(ProcessApprovalStep);
        }


        public async Task<List<GetProcessApprovalStepDto>> FindAsync(
        Expression<Func<office_automation_system.domain.Entities.ProcessApprovalStep, bool>> predicate)
        {
            // 1. Use the generic repository to query entities
            var ProcessApprovalSteps = await _unitOfWork.ProcessApprovalSteps
                .FindAsync(predicate);

            // 2. Convert entities to DTOs using AutoMapper
            var dtos = _mapper.Map<List<GetProcessApprovalStepDto>>(ProcessApprovalSteps.OrderBy(p => p.Order));

            // 3. Return the DTO list
            return dtos;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var ProcessApprovalStep = await _unitOfWork.ProcessApprovalSteps.GetByIdAsync(id);
            if (ProcessApprovalStep == null) return false;

            _unitOfWork.ProcessApprovalSteps.Remove(ProcessApprovalStep);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var ProcessApprovalStep = await _unitOfWork.ProcessApprovalSteps.GetByIdAsync(id);
            if (ProcessApprovalStep == null)
            {
                return false;
            }

            ProcessApprovalStep.IsDeleted = true;
            _unitOfWork.ProcessApprovalSteps.Update(ProcessApprovalStep);
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }
}
