using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.ApplicationUser;
using office_automation_system.application.Dto.ApplicationUser;
using office_automation_system.application.Dto.Role;
using System;
using System.Threading.Tasks;
using office_automation_system.application.Wrapper;
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
                return Ok(new ApiResponse<List<GetApplicationUserDto>>(
                    true,"List of all users",users,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null) { 
                    return NotFound(new ApiResponse<object>(
                        false,"User not found",null,null    
                    ));
                }

                return Ok(new ApiResponse<GetApplicationUserDto>(
                    true,"User Received successfully",user,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null      
                ));
            }
            
        }

        [HttpGet("search")]
        public async Task<IActionResult> FilterUsers([FromQuery] UserFilterDto filter)
        {
            try
            {
                var result = await _userService.FilterUsersAsync(filter);
                return Ok(new ApiResponse<List<GetApplicationUserDto>>(
                    true,"List of Searched users",result,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateApplicationUserDto dto)
        {

            try
            {
                var result = await _userService.CreateUserAsync(dto);
                if (!result.Success)
                    return BadRequest(new ApiResponse<object>(
                        false,null,null,result.Errors    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"User Created Successfully",null,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            
            }
            
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] EditApplicationUserDto dto)
        {

            try
            {
                var result = await _userService.UpdateUserAsync(userId, dto);
                if (!result.Success)
                    return BadRequest(new ApiResponse<object>(
                        false,null, null,result.Errors    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"User Updated Successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500,new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            
            }
            
        }

        [HttpPost("lock/{userId}")]
        public async Task<IActionResult> LockUser(Guid userId)
        {
            try
            {
                var result = await _userService.LockUserAsync(userId);
                if (!result)
                    return BadRequest(new ApiResponse<object>(
                        false,"Failed to Lock user",null,null    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"User Locked Successfully",null,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        [HttpPost("unlock/{userId}")]
        public async Task<IActionResult> UnlockUser(Guid userId)
        {
            try
            {
                var result = await _userService.UnlockUserAsync(userId);
                if (!result)
                    return BadRequest(new ApiResponse<object>(
                        false,"Failed to Unlock user",null,null    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"User Unlocked Successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
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
               

                var result = await _userService.AssignRoleToUserAsync(dto);
                if (!result.Success)
                    return BadRequest(new ApiResponse<object>(
                        false,null,null,result.Errors    
                    ));

                return Ok(new ApiResponse<object>(
                    true,"Role Assigned to User Successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            }
            
        }
    }
}