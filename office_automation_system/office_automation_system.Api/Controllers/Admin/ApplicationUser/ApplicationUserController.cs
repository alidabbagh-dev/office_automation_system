using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.ApplicationUser;
using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.application.Dto.Role;
using System;
using System.Threading.Tasks;

namespace office_automation_system.API.Controllers
{
    [Route("api/admin/[Controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Restrict to Admin users only
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserGenericService _userService;

        public ApplicationUserController(IApplicationUserGenericService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("search")]
        public async Task<IActionResult> FilterUsers([FromQuery] UserFilterDto filter)
        {
            var result = await _userService.FilterUsersAsync(filter);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateApplicationUserDto dto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var result = await _userService.CreateUserAsync(dto);
            if (!result)
                return BadRequest("Failed to create user");

            return Ok("User created successfully");
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] EditApplicationUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserAsync(userId, dto);
            if (!result)
                return BadRequest("Failed to update user");

            return Ok("User updated successfully");
        }

        [HttpPost("lock/{userId}")]
        public async Task<IActionResult> LockUser(Guid userId)
        {
            var result = await _userService.LockUserAsync(userId);
            if (!result)
                return BadRequest("Failed to lock user");

            return Ok("User locked successfully");
        }

        [HttpPost("unlock/{userId}")]
        public async Task<IActionResult> UnlockUser(Guid userId)
        {
            var result = await _userService.UnlockUserAsync(userId);
            if (!result)
                return BadRequest("Failed to unlock user");

            return Ok("User unlocked successfully");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result)
                return BadRequest("Failed to delete user");

            return Ok("User deleted successfully");
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.AssignRoleToUserAsync(dto);
            if (!result)
                return BadRequest("Failed to assign role");

            return Ok("Role assigned successfully");
        }
    }
}