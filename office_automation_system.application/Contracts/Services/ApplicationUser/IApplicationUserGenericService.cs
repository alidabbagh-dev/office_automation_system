using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.application.Dto.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.ApplicationUser
{
    public interface IApplicationUserGenericService
    {
        Task<List<GetApplicationUserDto>> GetAllUsersAsync();
        Task<GetApplicationUserDto?> GetUserByIdAsync(Guid userId);
        Task<List<GetApplicationUserDto>> FilterUsersAsync(UserFilterDto filter);
        Task<(bool Success, List<string> Errors)> CreateUserAsync(CreateApplicationUserDto dto);
        Task<(bool Success, List<string> Errors)> UpdateUserAsync(Guid Id, EditApplicationUserDto dto);
        Task<bool> LockUserAsync(Guid userId);
        Task<bool> UnlockUserAsync(Guid userId);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<(bool Success, List<string> Errors)> AssignRoleToUserAsync(AssignRoleDto dto);
    }
}
