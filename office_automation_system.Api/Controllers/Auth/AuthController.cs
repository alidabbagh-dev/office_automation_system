using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Auth;
using office_automation_system.application.Dto.Auth;
using office_automation_system.application.Wrapper;
using System.Security.Claims;

namespace office_automation_system.Api.Controllers.Auth
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                if (result?.errors != null)
                {
                    return BadRequest(new ApiResponse<object>(
                          false,null,null,result.errors
                    ));
                }
                return Ok(new ApiResponse<AuthResponseDto>(
                    true,"user registered successfully",result,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                if (result?.errors != null) return Unauthorized(new ApiResponse<object>(
                    false,"could not login",null,result?.errors    
                ));
                return Ok(new ApiResponse<AuthResponseDto>(
                    true,"user logged in successfully",result, null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                if (result == null) return Unauthorized(new ApiResponse<object>(
                    false,"Token Is Invalid or expired", null, ["Token is invalid or expired"]    
                ));
                return Ok(new ApiResponse<AuthResponseDto>(
                    true,"new Refresh token",result,null    
                ));
            }
            catch (Exception ex) { 
                return StatusCode(500,new ApiResponse<object>(
                    false, ex.Message,null,null    
                ));
            }
            
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized(new ApiResponse<object>(
                    false,"You can not loggout", null, ["You can not logout"]    
                ));

                var result = await _authService.LogoutAsync(userId);
                if (!result) return BadRequest(new ApiResponse<object>(
                    false,"Logout Failed", null, ["Logout Failed"]    
                ));

                return Ok(new ApiResponse<object>(
                    true,"Logged out successfully",null,null    
                ));
            }
            catch (Exception ex) {
                return StatusCode(500, new ApiResponse<object>(
                    false,ex.Message,null,null    
                ));
            }
            
        }

    }

}
