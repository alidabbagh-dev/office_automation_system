using FluentValidation;
using office_automation_system.application.Dto.RequestStep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Validator.RequestStep
{
    public class EditRequestStepDtoValidator : AbstractValidator<EditRequestStepDto>
    {
        public EditRequestStepDtoValidator()
        {
            
            

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.IsCurrentStep)
                .NotNull().WithMessage("IsCurrentStep is required.");

            RuleFor(x => x.CreatedAt)
                .Must(BeValidDateTimeOrNull).WithMessage("CreatedAt must be a valid date.")
                .When(x => x.CreatedAt.HasValue);

            RuleFor(x => x.UpdatedAt)
                .Must(BeValidDateTimeOrNull).WithMessage("UpdatedAt must be a valid date.")
                .When(x => x.UpdatedAt.HasValue);
        }

        private bool BeValidDateTimeOrNull(DateTime? dt)
        {
            if (!dt.HasValue) return true;
            return dt.Value > DateTime.MinValue && dt.Value < DateTime.MaxValue;
        }
    }
}
