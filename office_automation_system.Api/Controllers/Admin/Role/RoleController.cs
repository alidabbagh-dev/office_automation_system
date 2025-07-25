using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Role;
using office_automation_system.application.Dto.Role;

namespace office_automation_system.Api.Controllers.Admin.Role
{

    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            try
            {
                var result = await _roleService.CreateAsync(dto);
                if (!result.IsSuccess)
                    return BadRequest(result.Errors);

                return Ok("Role Created Successfully");
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetAllAsync();
                return Ok(new { roles });
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }

     
    }


}
