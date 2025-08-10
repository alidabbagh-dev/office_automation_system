using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using office_automation_system.Infrastructure.UnitOfWork;
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
using office_automation_system.domain.Entities;
using office_automation_system.Infrastructure.Data;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.Infrastructure.InfrastructureConfiguration
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>() // Guid-based identity
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Authentication - JWT
            var jwtKey = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                    ValidateLifetime = true
                };
            });

            // UnitOfWork
            services.AddScoped<IUnitOfWork,office_automation_system.Infrastructure.UnitOfWork.UnitOfWork>();

            // Application Services
            services.AddScoped<IAdministrativeProcessGenericService, AdministrativeProcessGenericService>();
            services.AddScoped<IFileManagerGenericService, FileManagerGenericService>();
            services.AddScoped<INotificationGenericService, NotificationGenericService>();
            services.AddScoped<IProcessApprovalStepGenericService, ProcessApprovalStepGenericService>();
            services.AddScoped<IRequestGenericService, RequestGenericService>();
            services.AddScoped<IRequestStepGenericService, RequestStepGenericService>();
            services.AddScoped<IRequestStepFileGenericService, RequestStepFileGenericService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IApplicationUserGenericService, ApplicationUserGenericService>();

            return services;
        }
    }
}
