using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using office_automation_system.application.Contracts.Services.Auth;
using office_automation_system.application.Dto.Auth;
using System.Security.Claims;

namespace office_automation_system.Api.Controllers.Auth
{
    //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ5MTI4MGNjLTdiZDMtNDdjYi1hNWJjLTA4ZGRhNmMxZDVhYyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFsaUBnbWFpbC5jb20iLCJleHAiOjE3NDk0MTE3OTYsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTA4OSIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTA4OSJ9.l7LhLe-rIqHsUVBT7cCKxKzFlsXwF0-DI6Lio0wZl4E
    //b9498501-1dd8-4420-9424-79f27f864caf
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
            var result = await _authService.RegisterAsync(dto);
            if (result?.errors != null) { 
                return BadRequest(result.errors);
            } 
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result?.errors != null) return Unauthorized(result?.errors);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto refreshTokenDto)
        {
            
            var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken);
            if (result == null) return Unauthorized("Refresh token is invalid or expired");
            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var result = await _authService.LogoutAsync(userId);
            if (!result) return BadRequest("Logout failed");

            return Ok("User logged out successfully");
        }

    }

}
