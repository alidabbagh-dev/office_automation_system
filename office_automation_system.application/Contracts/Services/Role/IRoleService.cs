using Microsoft.AspNetCore.Identity;
using office_automation_system.application.Dto.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Contracts.Services.Role
{
    public interface IRoleService
{
    Task<(bool IsSuccess, List<string>? Errors)> CreateAsync(CreateRoleDto dto);
    Task<List<IdentityRole<Guid>>> GetAllAsync();
    
}


}
