using FluentValidation;
using office_automation_system.application.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Validator.Auth
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        public LoginDtoValidator() {
            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("Email is required.");



            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
                
        }
    }
}
