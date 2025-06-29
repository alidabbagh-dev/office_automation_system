using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.AdministrativeProcess
{
    public interface IAdministrativeProcessGenericService
    {
        Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateAdministrativeProcessDto dto);
        Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditAdministrativeProcessDto dto);
        Task<List<GetAdministrativeProcessDto>> GetAllAsync();
        Task<GetAdministrativeProcessDto?> GetByIdAsync(Guid id);
        Task<List<GetAdministrativeProcessDto>> FindAsync(Expression<Func<office_automation_system.domain.Entities.AdministrativeProcess, bool>> predicate);

        Task<bool> DeleteAsync(Guid id);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
