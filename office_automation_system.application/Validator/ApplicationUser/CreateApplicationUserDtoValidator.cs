using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using office_automation_system.application.Dto.ApplicationUser;
namespace office_automation_system.application.Validator.ApplicationUser
{
    

    public class CreateApplicationUserDtoValidator : AbstractValidator<CreateApplicationUserDto>
    {
        public CreateApplicationUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                 .NotEmpty().WithMessage("Password is required.")
                 .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                 .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                 .Matches(@"\d").WithMessage("Password must contain at least one number.")
                 .Matches(@"[!@#$%^&*()_+\-=,<>?;:{}\[\]]")
                 .WithMessage("Password must contain at least one special character.");

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
