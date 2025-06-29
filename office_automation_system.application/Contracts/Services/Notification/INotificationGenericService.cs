using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.application.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.Notification
{
    public interface INotificationGenericService
    {
        Task<(bool IsSuccess, List<string> Errors)> CreateAsync(CreateNotificationDto dto);
        Task<(bool IsSuccess, List<string> Errors)> EditAsync(Guid id, EditNotificationDto dto);
        Task<List<GetNotificationDto>> GetAllAsync();
        Task<GetNotificationDto?> GetByIdAsync(Guid id);
        Task<List<GetNotificationDto>> FindAsync(Expression<Func<office_automation_system.domain.Entities.Notification, bool>> predicate);

        Task<bool> DeleteAsync(Guid id);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
