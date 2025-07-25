using FluentValidation;
using office_automation_system.application.Dto.AdministrativeProcess;
using office_automation_system.application.Dto.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Validator.AdministrativeProcess
{
    public class CreateAdministrativeProcessDtoValidator: AbstractValidator<CreateAdministrativeProcessDto>
    {
        public CreateAdministrativeProcessDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is Required")
                .MaximumLength(100).WithMessage("Title has maximum length of 100");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description Is Required")
                .MaximumLength(1000).WithMessage("Description has maximum length of 1000");

            RuleFor(x => x.CreatedAt)
                .Must(BeValidDateTimeOrNull)
                .WithMessage("CreatedAt is not valid Date");

            RuleFor(x => x.UpdatedAt)
                .Must(BeValidDateTimeOrNull)
                .WithMessage("UpdatedAt is not valid Date");
        }

        private bool BeValidDateTimeOrNull(DateTime? dt)
        {
            if (!dt.HasValue) return true;
            return dt.Value > DateTime.MinValue && dt.Value < DateTime.MaxValue;
        }
    }
}
