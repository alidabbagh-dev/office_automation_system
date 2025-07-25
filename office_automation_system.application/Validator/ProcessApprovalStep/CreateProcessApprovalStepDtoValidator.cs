using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace office_automation_system.application.Validator.ProcessApprovalStep
{
    using FluentValidation;
    using office_automation_system.application.Dto.ProcessApprovalStep;

    public class CreateProcessApprovalStepDtoValidator : AbstractValidator<CreateProcessApprovalStepDto>
    {
        public CreateProcessApprovalStepDtoValidator()
        {
            RuleFor(x => x.StepTitle)
                .NotEmpty().WithMessage("StepTitle is required.")
                .MaximumLength(100).WithMessage("StepTitle must not exceed 100 characters.");

            RuleFor(x => x.AdministrativeProcessId)
                .NotNull().WithMessage("AdministrativeProcessId is required.")
                .Must(id => id != Guid.Empty)
                .WithMessage("AdministrativeProcessId must be a valid non-empty GUID.");

            RuleFor(x => x.Order)
                .NotNull().WithMessage("Order is required.")
                .Must(x => x is int).WithMessage("Order must be a valid integer.");

            RuleFor(x => x.CreatedAt)
                .Must(BeValidDateTimeOrNull).When(x => x.CreatedAt.HasValue)
                .WithMessage("CreatedAt must be a valid date.");

            RuleFor(x => x.UpdatedAt)
                .Must(BeValidDateTimeOrNull).When(x => x.UpdatedAt.HasValue)
                .WithMessage("UpdatedAt must be a valid date.");

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
        }

        private bool BeValidDateTimeOrNull(DateTime? dt)
        {
            if (!dt.HasValue) return true;
            return dt.Value > DateTime.MinValue && dt.Value < DateTime.MaxValue;
        }
    }

}
