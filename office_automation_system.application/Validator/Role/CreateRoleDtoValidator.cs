using FluentValidation;
using office_automation_system.application.Dto.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Validator.Role
{
    public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role Name Is Required")
                .MaximumLength(100).WithMessage("Role Name Maximum Length is 100 characters");
        }
    }
}
