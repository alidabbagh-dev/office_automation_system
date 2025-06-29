
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using office_automation_system.domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace office_automation_system.Api.Middleware.GeneralMiddlewares
{
   

    public class CheckUserLockoutMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckUserLockoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            // اگر کاربر احراز هویت نشده بود، برو مرحله بعد
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                await _next(context);
                return;
            }

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                await _next(context);
                return;
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user != null && await userManager.IsLockedOutAsync(user))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = 403,
                    message = "User account is locked.",
                    data = (object?)null,
                    errors = new[] { "Your account is locked. Please contact administrator." }
                });
                return;
            }

            await _next(context);
        }
    }

}
