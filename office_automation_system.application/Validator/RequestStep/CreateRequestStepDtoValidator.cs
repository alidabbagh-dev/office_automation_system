using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using office_automation_system.application.Dto.RequestStep;
namespace office_automation_system.application.Validator.RequestStep
{
   
 
    public class CreateRequestStepDtoValidator : AbstractValidator<CreateRequestStepDto>
    {
        public CreateRequestStepDtoValidator()
        {
            RuleFor(x => x.RequestId)
                .NotNull().WithMessage("RequestId is required.")
                .Must(id => id != Guid.Empty).WithMessage("RequestId must be a valid GUID.");

            RuleFor(x => x)
                .Must(x => x.OwnerId != null || x.RoleId != null)
                .WithMessage("Either OwnerId or RoleId must be provided.");

            RuleFor(x => x)
                 .Must(x =>
                     (x.OwnerId.HasValue && x.OwnerId.Value != Guid.Empty) ||
                     (x.RoleId.HasValue && x.RoleId.Value != Guid.Empty)
                 )
                 .WithMessage("Either OwnerId or RoleId must be provided.");

            RuleFor(x => x.OwnerId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("OwnerId must be a valid non-empty GUID when provided.");

            RuleFor(x => x.RoleId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("RoleId must be a valid non-empty GUID when provided.");

            RuleFor(x => x.Order)
                .NotNull().WithMessage("Order is required.")
                .Must(order => order is int).WithMessage("Order must be a valid integer");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

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
