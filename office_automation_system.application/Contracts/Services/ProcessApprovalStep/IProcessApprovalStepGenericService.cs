using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.application.Dto.ProcessApprovalStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.ProcessApprovalStep
{
    public interface IProcessApprovalStepGenericService
    {
        Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateProcessApprovalStepDto dto);
        Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditProcessApprovalStepDto dto);
        Task<List<GetProcessApprovalStepDto>> GetAllAsync();
        Task<GetProcessApprovalStepDto?> GetByIdAsync(Guid id);
        Task<List<GetProcessApprovalStepDto>> FindAsync(Expression<Func<office_automation_system.domain.Entities.ProcessApprovalStep, bool>> predicate);

        Task<bool> DeleteAsync(Guid id);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
