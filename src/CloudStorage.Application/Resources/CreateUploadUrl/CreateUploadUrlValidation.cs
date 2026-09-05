using FluentValidation;

namespace CloudStorage.Application.Resources.CreateUploadUrl;

internal sealed class CreateUploadUrlValidation : AbstractValidator<CreateUploadUrlCommand>
{
    public CreateUploadUrlValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(128);

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.ContentLength)
            .NotEmpty()
            .GreaterThan(0);
    }
}