using FluentValidation;
using office_automation_system.application.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Validator.Notification
{
    public class EditNotificationDtoValidator : AbstractValidator<EditNotificationDto>
    {
        public EditNotificationDtoValidator()
        {
   

            RuleFor(x => x.IsSeen)
                .NotNull().WithMessage("IsSeen is required.");

           
        }
    }
}
