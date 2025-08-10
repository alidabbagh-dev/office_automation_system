using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using office_automation_system.application.Contracts.Services.Auth;
using office_automation_system.application.Wrapper;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Api.Middleware
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public RefreshTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuthService authService, IConfiguration config)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var refreshToken = context.Request.Headers["X-Refresh-Token"].FirstOrDefault();

            if (!string.IsNullOrEmpty(token))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);

                try
                {
                    // بررسی اعتبار توکن
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    }, out _);
                }
                catch (SecurityTokenExpiredException)
                {
                    // اگر توکن منقضی شده و رفرش توکن موجوده
                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        var newTokens = await authService.RefreshTokenAsync(refreshToken);
                        if (newTokens != null)
                        {
                            // ارسال توکن جدید در هدر
                            context.Response.Headers["X-New-Token"] = newTokens.Token!;
                            context.Response.Headers["X-New-Refresh-Token"] = newTokens.RefreshToken!;
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsJsonAsync(

                               new ApiResponse<object>(false, "Refresh token is invalid or expired",
                                    null, ["Refresh token is invalid or expired"]
                               )
                            );

                            return;
                        }
                    }
                }
            }

            await _next(context);
        }
    }

    // متد اکستنشن برای اضافه کردن Middleware
    public static class RefreshTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseRefreshTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RefreshTokenMiddleware>();
        }
    }
}
