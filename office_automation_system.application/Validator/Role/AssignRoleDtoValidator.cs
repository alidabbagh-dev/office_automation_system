using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using office_automation_system.application.Dto.Role;
namespace office_automation_system.application.Validator.Role
{
   
    

    public class AssignRoleDtoValidator : AbstractValidator<AssignRoleDto>
    {
        public AssignRoleDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required")
                .Must(id => id.HasValue && id.Value != Guid.Empty)
                .WithMessage("UserId should be a valid Guid");

            RuleFor(x => x.RoleId)
                .NotNull().WithMessage("RoleId is required")
                .Must(id => id.HasValue && id.Value != Guid.Empty)
                .WithMessage("RoleId should be a valid Guid");
        }
    }

}
