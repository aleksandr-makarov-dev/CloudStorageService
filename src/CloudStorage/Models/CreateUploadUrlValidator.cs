using FluentValidation;

namespace CloudStorage.Models;

public class CreateUploadUrlValidator : AbstractValidator<CreateUploadUrlRequest>
{
    public CreateUploadUrlValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.ContentLength)
            .NotNull()
            .GreaterThan(0);
    }
}