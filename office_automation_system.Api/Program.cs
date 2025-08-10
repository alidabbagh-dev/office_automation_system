using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using office_automation_system.Api.Middleware;
using office_automation_system.Api.Middleware.GeneralMiddlewares;
using office_automation_system.application.ApplicationConfiguration;
using office_automation_system.domain.Entities;
using office_automation_system.Infrastructure.Identity.Seed;
using office_automation_system.Infrastructure.InfrastructureConfiguration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddOpenApi();

//Application services
builder.Services.AddApplicationServices();
//Infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<RefreshTokenMiddleware>();
app.UseMiddleware<CheckUserLockoutMiddleware>();
app.UseAuthorization();

app.MapControllers();

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
