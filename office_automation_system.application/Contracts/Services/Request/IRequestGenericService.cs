using office_automation_system.application.Dto.ProcessApprovalStep;
using office_automation_system.application.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.Request
{
    public interface IRequestGenericService
    {
        Task<bool> CheckIsVerified(CreateRequestDto dto);//this method checks that can user edit it or not
        Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateRequestDto dto);
        Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditRequestDto dto);
        Task<List<GetRequestDto>> GetAllAsync();
        Task<GetRequestDto?> GetByIdAsync(Guid id);
        Task<List<GetRequestDto>> FindAsync(Expression<Func<office_automation_system.domain.Entities.Request, bool>> predicate);

        Task<bool> DeleteAsync(Guid id);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
