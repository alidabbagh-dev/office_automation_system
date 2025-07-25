using FluentValidation;
using office_automation_system.application.Dto.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Validator.ApplicationUser
{
    public class EditApplicationUserDtoValidator : AbstractValidator<EditApplicationUserDto>
    {
        public EditApplicationUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

          

            RuleFor(x => x.UserCode)
                .NotEmpty().WithMessage("UserCode is required.")
                .Length(7).WithMessage("UserCode must be exactly 7 characters.");

            RuleFor(x => x.RoleId)
                .NotNull().WithMessage("RoleId is required.")
                .Must(roleId => roleId != null && roleId != Guid.Empty)
                .WithMessage("RoleId must be a valid non-empty GUID.");
        }
    }
}
