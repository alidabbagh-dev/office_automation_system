using AutoMapper;
using office_automation_system.application.Contracts.Services.AdministrativeProcess;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Services.AdministrativeProcess
{
    public class AdministrativeProcessGenericService : IAdministrativeProcessGenericService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;



        public AdministrativeProcessGenericService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
            
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        public async Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateAdministrativeProcessDto dto)
        {
            //ValidationResult validation = await _createValidator.ValidateAsync(dto);
            //if (!validation.IsValid)
            //    return (false, validation.Errors.Select(e => e.ErrorMessage).ToList());

            var AdministrativeProcess = _mapper.Map<office_automation_system.domain.Entities.AdministrativeProcess>(dto);
            await _unitOfWork.AdministrativeProcesses.AddAsync(AdministrativeProcess);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditAdministrativeProcessDto dto)
        {
            //ValidationResult validation = await _editValidator.ValidateAsync(dto);
            //if (!validation.IsValid)
            //    return (false, validation.Errors.Select(e => e.ErrorMessage).ToList());

            var AdministrativeProcess = await _unitOfWork.AdministrativeProcesses.GetByIdAsync(id);
            if (AdministrativeProcess == null) return (false, new List<string> { "AdministrativeProcess Not Found with this id" });

            _mapper.Map(dto, AdministrativeProcess);
            _unitOfWork.AdministrativeProcesses.Update(AdministrativeProcess);
            await _unitOfWork.CompleteAsync();
            return (true, []);
        }


        public async Task<List<GetAdministrativeProcessDto>> GetAllAsync()
        {
            var AdministrativeProcesses = await _unitOfWork.AdministrativeProcesses.GetAllAsync();
            return _mapper.Map<List<GetAdministrativeProcessDto>>(AdministrativeProcesses);
        }



        public async Task<GetAdministrativeProcessDto?> GetByIdAsync(Guid id)
        {
            var AdministrativeProcess = await _unitOfWork.AdministrativeProcesses.GetByIdAsync(id);
            return AdministrativeProcess == null ? null : _mapper.Map<GetAdministrativeProcessDto>(AdministrativeProcess);
        }


        public async Task<List<GetAdministrativeProcessDto>> FindAsync(
        Expression<Func<office_automation_system.domain.Entities.AdministrativeProcess, bool>> predicate)
        {
            // 1. Use the generic repository to query entities
            var AdministrativeProcesses = await _unitOfWork.AdministrativeProcesses
                .FindAsync(predicate);

            // 2. Convert entities to DTOs using AutoMapper
            var dtos = _mapper.Map<List<GetAdministrativeProcessDto>>(AdministrativeProcesses);

            // 3. Return the DTO list
            return dtos;
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var AdministrativeProcess = await _unitOfWork.AdministrativeProcesses.GetByIdAsync(id);
            if (AdministrativeProcess == null) return false;

            _unitOfWork.AdministrativeProcesses.Remove(AdministrativeProcess);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var AdministrativeProcess = await _unitOfWork.AdministrativeProcesses.GetByIdAsync(id);
            if (AdministrativeProcess == null)
            {
                return false;
            }

            AdministrativeProcess.IsDeleted = true;
            _unitOfWork.AdministrativeProcesses.Update(AdministrativeProcess);
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }
}
