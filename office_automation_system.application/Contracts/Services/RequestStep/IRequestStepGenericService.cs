using office_automation_system.application.Dto.RequestStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.RequestStep
{
    public interface IRequestStepGenericService
    {
        Task<bool> CheckIsVerified(Guid RequestStepId);
        Task<int> CheckStepPosition(Guid RequestStepId, Guid RequestId);
        Task<(bool IsSuccess, List<string> Errors)> ReferToPreviousStep(Guid RequestStepId);
        Task<(bool IsSuccess, List<string> Errors)> ReferToNextStep(Guid RequestStepId);
        Task<(bool IsSuccess, List<string> Errors)> ConfirmFinalStep(Guid RequestStepId);
        Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateRequestStepDto dto);
        Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditRequestStepDto dto);
        Task<List<GetRequestStepDto>> GetAllAsync();
        Task<GetRequestStepDto?> GetByIdAsync(Guid id);
        Task<List<GetRequestStepDto>> FindAsync(Expression<Func<office_automation_system.domain.Entities.RequestStep, bool>> predicate);

        Task<bool> DeleteAsync(Guid id);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
