using office_automation_system.application.Dto.RequestStepFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.RequestStepFile
{
    public interface IRequestStepFileGenericService
    {
        Task<bool> CheckIsVerified(Guid RequestStepId);
        Task<( bool IsSuccess, List<string> Errors)> CreateAsync(CreateRequestStepFileDto dto);
        Task<List<GetRequestStepFileDto>> GetAllAsync();
        Task<GetRequestStepFileDto?> GetByIdAsync(Guid id);
        Task<List<GetRequestStepFileDto>> FindAsync(Expression<Func<office_automation_system.domain.Entities.RequestStepFile, bool>> predicate);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
