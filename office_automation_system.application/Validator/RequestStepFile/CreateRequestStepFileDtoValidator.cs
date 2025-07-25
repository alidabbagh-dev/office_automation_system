using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using office_automation_system.application.Dto.RequestStepFile;
namespace office_automation_system.application.Validator.RequestStepFile
{
    
    

    public class CreateRequestStepFileDtoValidator : AbstractValidator<CreateRequestStepFileDto>
    {
        public CreateRequestStepFileDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is Required")
                .MaximumLength(200).WithMessage("Title has maximum length of 200 characters");

            RuleFor(x => x.File)
                .NotNull().WithMessage("file is required")
                .Must(f => f?.Length <= 104857600) // 100MB = 100 * 1024 * 1024
                .WithMessage("file maximum size is 100mb");

            RuleFor(x => x.RequestStepId)
                .NotNull().WithMessage("RequestStepId is required")
                .Must(id => id != Guid.Empty).WithMessage("request step id should be a valid guid");

            RuleFor(x => x.CreatedAt)
                .Must(BeValidDateTimeOrNull).WithMessage("CreatedAt is not valid date")
                .When(x => x.CreatedAt.HasValue);

            RuleFor(x => x.UpdatedAt)
                .Must(BeValidDateTimeOrNull).WithMessage("updatedAt is not valid date")
                .When(x => x.UpdatedAt.HasValue);
        }

        private bool BeValidDateTimeOrNull(DateTime? dt)
        {
            if (!dt.HasValue) return true;
            return dt.Value > DateTime.MinValue && dt.Value < DateTime.MaxValue;
        }
    }

}
