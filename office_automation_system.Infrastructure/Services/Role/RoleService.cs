using Microsoft.AspNetCore.Identity;
using office_automation_system.application.Contracts.Services.Role;
using office_automation_system.application.Dto.Role;
using office_automation_system.application.Validator.Role;
using office_automation_system.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
namespace office_automation_system.Infrastructure.Services.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<office_automation_system.domain.Entities.ApplicationUser> _userManager;
        private readonly CreateRoleDtoValidator _createValidator;

        public RoleService(RoleManager<IdentityRole<Guid>> roleManager
            , UserManager<office_automation_system.domain.Entities.ApplicationUser> userManager
            , CreateRoleDtoValidator createValidator
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _createValidator = createValidator;
        }

        public async Task<(bool IsSuccess, List<string>? Errors)> CreateAsync(CreateRoleDto dto)
        {
            FluentValidation.Results.ValidationResult validation = await _createValidator.ValidateAsync(dto);

            if (!validation.IsValid)
            {
                var errorList = validation.Errors
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return (false, errorList);
            }

            if (await _roleManager.RoleExistsAsync(dto.Name))
                return (false, new List<string> { "Role already exists." });

            var role = new IdentityRole<Guid> { Name = dto?.Name };
            var result = await _roleManager.CreateAsync(role);

            return result.Succeeded
                ? (true, null)
                : (false, result.Errors.Select(e => e.Description).ToList());
        }

        public async Task<List<IdentityRole<Guid>>> GetAllAsync()
        {
            return await Task.FromResult(_roleManager.Roles.ToList());
        }

        
    }

}
