using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using office_automation_system.domain.Entities;
using office_automation_system.domain.Enums.SystemStaticRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.Identity.Seed
{
    public static class AdminUserSeeder
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin#1380";

            // اگر قبلاً این کاربر ساخته نشده
            var existingUser = await userManager.FindByEmailAsync(adminEmail);
            if (existingUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    UserCode = "ADM-001"
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);

                if (result.Succeeded)
                {
                    // اضافه کردن به نقش Admin
                    await userManager.AddToRoleAsync(newAdmin, SystemRoles.Admin.ToString());
                }
                else
                {
                    Console.WriteLine("❌ Admin user creation failed:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }
            }
        }
    }
}
