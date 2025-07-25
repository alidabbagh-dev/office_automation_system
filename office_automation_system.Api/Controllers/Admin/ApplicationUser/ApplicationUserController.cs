using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.ApplicationUser;
using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.application.Dto.Role;
using System;
using System.Threading.Tasks;

namespace office_automation_system.Api.Controllers.Admin.ApplicationUser
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
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("search")]
        public async Task<IActionResult> FilterUsers([FromQuery] UserFilterDto filter)
        {
            try
            {
                var result = await _userService.FilterUsersAsync(filter);
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateApplicationUserDto dto)
        {

            try
            {
                var result = await _userService.CreateUserAsync(dto);
                if (!result.Success)
                    return BadRequest(result.Errors);

                return Ok("User created successfully");
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            
            }
            
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] EditApplicationUserDto dto)
        {

            try
            {
                var result = await _userService.UpdateUserAsync(userId, dto);
                if (!result.Success)
                    return BadRequest(result.Errors);

                return Ok("User updated successfully");
            }
            catch (Exception ex) {
                return StatusCode(500,ex.Message);
            
            }
            
        }

        [HttpPost("lock/{userId}")]
        public async Task<IActionResult> LockUser(Guid userId)
        {
            try
            {
                var result = await _userService.LockUserAsync(userId);
                if (!result)
                    return BadRequest("Failed to lock user");

                return Ok("User locked successfully");
            }
            catch (Exception ex) { 
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost("unlock/{userId}")]
        public async Task<IActionResult> UnlockUser(Guid userId)
        {
            try
            {
                var result = await _userService.UnlockUserAsync(userId);
                if (!result)
                    return BadRequest("Failed to unlock user");

                return Ok("User unlocked successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        //[HttpDelete("{userId}")]
        //public async Task<IActionResult> DeleteUser(Guid userId)
        //{
        //    var result = await _userService.DeleteUserAsync(userId);
        //    if (!result)
        //        return BadRequest("Failed to delete user");

        //    return Ok("User deleted successfully");
        //}

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userService.AssignRoleToUserAsync(dto);
                if (!result)
                    return BadRequest("Failed to assign role");

                return Ok("Role assigned successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }
    }
}