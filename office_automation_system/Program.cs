using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using office_automation_system.application.Contracts.Services.AdministrativeProcess;
using office_automation_system.application.Contracts.Services.FileManager;
using office_automation_system.application.Contracts.Services.Notification;
using office_automation_system.application.Contracts.Services.ProcessApprovalStep;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Contracts.Services.RequestStepFile;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Mapping;
using office_automation_system.domain.Entities;
using office_automation_system.Infrastructure.Data;
using office_automation_system.Infrastructure.Identity.Seed;
using office_automation_system.Infrastructure.Services.AdministrativeProcess;
using office_automation_system.Infrastructure.Services.FileManager;
using office_automation_system.Infrastructure.Services.Notification;
using office_automation_system.Infrastructure.Services.ProcessApprovalStep;
using office_automation_system.Infrastructure.Services.Request;
using office_automation_system.Infrastructure.Services.RequestStep;
using office_automation_system.Infrastructure.Services.RequestStepFile;
using office_automation_system.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await RoleSeeder.SeedRolesAsync(services);
        await AdminUserSeeder.SeedAdminUserAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Seeding roles failed: {ex.Message}");
    }
}




app.Run();
