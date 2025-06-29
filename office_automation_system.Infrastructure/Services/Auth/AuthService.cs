using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using office_automation_system.application.Contracts.Services.Auth;
using office_automation_system.application.Dto.Auth;
using office_automation_system.domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<office_automation_system.domain.Entities.ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<office_automation_system.domain.Entities.ApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            var user = new office_automation_system.domain.Entities.ApplicationUser { UserName = dto?.Email, Email = dto?.Email };
            var result = await _userManager.CreateAsync(user, dto?.Password);
            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description).ToList();
                var authResponseDto = new AuthResponseDto();
                authResponseDto.errors = errors;
                return authResponseDto;
            }

            await _userManager.AddToRoleAsync(user, "User"); //

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) {
                var authResponseDto = new AuthResponseDto();
                authResponseDto.errors = new List<string>();
                authResponseDto.errors.Add("User not found");
                return authResponseDto;
            }

            
            else if(!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                var authResponseDto = new AuthResponseDto();
                authResponseDto.errors = new List<string>();
                authResponseDto.errors.Add("Couldn't Login the User");
                return authResponseDto;
            }
            

            return await GenerateTokensAsync(user);
        }

        public async Task<bool> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }


        public async Task<AuthResponseDto?> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow) return null;

            return await GenerateTokensAsync(user);
        }

        private async Task<AuthResponseDto> GenerateTokensAsync(office_automation_system.domain.Entities.ApplicationUser user)
        {
            // دریافت نقش‌های کاربر
            var userRoles = await _userManager.GetRolesAsync(user);

            // تعریف Claims برای توکن
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user?.Email!)
    };

            // افزودن نقش‌ها به Claims
            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // کلید رمزنگاری برای امضای توکن
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            // ساخت توکن JWT
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            // نوشتن توکن به رشته
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // تولید Refresh Token به صورت رندوم (می‌تونی الگوریتم پیچیده‌تر هم بزاری)
            var refreshToken = Guid.NewGuid().ToString();

            // ذخیره RefreshToken و تاریخ انقضای آن داخل دیتابیس کاربر
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_config["Jwt:RefreshTokenExpiryDays"]));
            await _userManager.UpdateAsync(user);

            // ساخت DTO خروجی
            return new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

    }

}
