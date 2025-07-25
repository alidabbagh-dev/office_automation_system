using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using office_automation_system.application.Dto.Notification;
namespace office_automation_system.application.Validator.Notification
{

    public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
    {
        public CreateNotificationDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required.")
                .Must(id => id != Guid.Empty).WithMessage("UserId must be a valid non-empty GUID.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

            RuleFor(x => x.IsSeen)
                .NotNull().WithMessage("IsSeen is required.");

            RuleFor(x => x.SentAt)
                .NotNull().WithMessage("SentAt is required.")
                .Must(BeAValidDate).WithMessage("SentAt must be a valid date.");

            RuleFor(x => x.CreatedAt)
                .Must(BeValidDateTimeOrNull).WithMessage("CreatedAt must be a valid date.")
                .When(x => x.CreatedAt.HasValue);

            RuleFor(x => x.UpdatedAt)
                .Must(BeValidDateTimeOrNull).WithMessage("UpdatedAt must be a valid date.")
                .When(x => x.UpdatedAt.HasValue);
        }

        private bool BeAValidDate(DateTime? date)
        {
            if (!date.HasValue) return false;

            return   date.Value > DateTime.MinValue && date.Value < DateTime.MaxValue;
        }

        private bool BeValidDateTimeOrNull(DateTime? dt)
        {
            if (!dt.HasValue) return true;
            return dt.Value > DateTime.MinValue && dt.Value < DateTime.MaxValue;
        }
    }


}
