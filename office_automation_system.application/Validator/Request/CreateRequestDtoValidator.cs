using FluentValidation;
using office_automation_system.application.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Validator.Request
{
    public class CreateRequestDtoValidator : AbstractValidator<CreateRequestDto>
    {
        public CreateRequestDtoValidator()
        {
            RuleFor(x => x.ProcessId)
                .NotNull().WithMessage("ProcessId is required.")
                .Must(id => id != Guid.Empty).WithMessage("ProcessId must be a valid GUID.");

            RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required.")
                .Must(id => id != Guid.Empty).WithMessage("UserId must be a valid GUID.");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("Status is required.")
                .IsInEnum().WithMessage("Status must be a valid enum value (0, 1, 2, or 3).");

            RuleFor(x => x.CreatedAt)
                .Must(BeValidDateTimeOrNull).When(x => x.CreatedAt.HasValue)
                .WithMessage("CreatedAt must be a valid date.");

            RuleFor(x => x.UpdatedAt)
                .Must(BeValidDateTimeOrNull).When(x => x.UpdatedAt.HasValue)
                .WithMessage("UpdatedAt must be a valid date.");
        }

        private bool BeValidDateTimeOrNull(DateTime? dt)
        {
            if (!dt.HasValue) return true;
            return dt.Value > DateTime.MinValue && dt.Value < DateTime.MaxValue;
        }
    }
}
