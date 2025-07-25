using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Auth;
using office_automation_system.application.Dto.Auth;
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
                    return BadRequest(result.errors);
                }
                return Ok(result);
            }
            catch (Exception ex) { 
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                if (result?.errors != null) return Unauthorized(result?.errors);
                return Ok(result);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
                if (result == null) return Unauthorized("Refresh token is invalid or expired");
                return Ok(result);
            }
            catch (Exception ex) { 
                return StatusCode(500,ex.Message);
            }
            
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();

                var result = await _authService.LogoutAsync(userId);
                if (!result) return BadRequest("Logout failed");

                return Ok("User logged out successfully");
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
            
        }

    }

}
