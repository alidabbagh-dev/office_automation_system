using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.ApplicationConfiguration
{
    public static class ApplicationServiceRegistration
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));
            //validators
            services.AddValidatorsFromAssemblyContaining<EditAdministrativeProcessDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateApplicationUserDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateAdministrativeProcessDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<EditApplicationUserDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProcessApprovalStepDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<EditProcessApprovalStepDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateRequestDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<EditRequestDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateRequestStepDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<EditRequestStepDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateRequestStepFileDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateRoleDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<AssignRoleDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateNotificationDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<EditNotificationDtoValidator>();

            return services;
        }
    }
}
