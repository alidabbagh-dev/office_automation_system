using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using office_automation_system.Api.Middleware.GeneralMiddlewares;
using office_automation_system.application.Contracts.Services.AdministrativeProcess;
using office_automation_system.application.Contracts.Services.ApplicationUser;
using office_automation_system.application.Contracts.Services.Auth;
using office_automation_system.application.Contracts.Services.FileManager;
using office_automation_system.application.Contracts.Services.Notification;
using office_automation_system.application.Contracts.Services.ProcessApprovalStep;
using office_automation_system.application.Contracts.Services.Request;
using office_automation_system.application.Contracts.Services.RequestStep;
using office_automation_system.application.Contracts.Services.RequestStepFile;
using office_automation_system.application.Contracts.Services.Role;
using office_automation_system.application.Contracts.UnitOfWork;
using office_automation_system.application.Mapping;
using office_automation_system.application.Validator.AdministrativeProcess;
using office_automation_system.application.Validator.ApplicationUser;
using office_automation_system.application.Validator.Auth;
using office_automation_system.application.Validator.Notification;
using office_automation_system.application.Validator.ProcessApprovalStep;
using office_automation_system.application.Validator.Request;
using office_automation_system.application.Validator.RequestStep;
using office_automation_system.application.Validator.RequestStepFile;
using office_automation_system.application.Validator.Role;
using office_automation_system.domain.Entities;
using office_automation_system.Infrastructure.Data;
using office_automation_system.Infrastructure.Identity.Seed;
using office_automation_system.Infrastructure.Services.AdministrativeProcess;
using office_automation_system.Infrastructure.Services.ApplicationUser;
using office_automation_system.Infrastructure.Services.Auth;
using office_automation_system.Infrastructure.Services.FileManager;
using office_automation_system.Infrastructure.Services.Notification;
using office_automation_system.Infrastructure.Services.ProcessApprovalStep;
using office_automation_system.Infrastructure.Services.Request;
using office_automation_system.Infrastructure.Services.RequestStep;
using office_automation_system.Infrastructure.Services.RequestStepFile;
using office_automation_system.Infrastructure.Services.Role;
using office_automation_system.Infrastructure.UnitOfWork;
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


builder.Services.AddAutoMapper(typeof(MappingProfile));

//Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>() // or without Guid if you're using string
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Add Authentication with JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
//UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateAdministrativeProcessDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditAdministrativeProcessDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateApplicationUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditApplicationUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProcessApprovalStepDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditProcessApprovalStepDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRequestStepDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditRequestStepDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRequestStepFileDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRoleDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AssignRoleDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateNotificationDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<EditNotificationDtoValidator>();


//Services
builder.Services.AddScoped<IAdministrativeProcessGenericService, AdministrativeProcessGenericService>();
builder.Services.AddScoped<IFileManagerGenericService, FileManagerGenericService>();
builder.Services.AddScoped<INotificationGenericService, NotificationGenericService>();
builder.Services.AddScoped<IProcessApprovalStepGenericService, ProcessApprovalStepGenericService>();
builder.Services.AddScoped<IRequestGenericService, RequestGenericService>();
builder.Services.AddScoped<IRequestStepGenericService, RequestStepGenericService>();
builder.Services.AddScoped<IRequestStepFileGenericService, RequestStepFileGenericService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IApplicationUserGenericService, ApplicationUserGenericService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
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
