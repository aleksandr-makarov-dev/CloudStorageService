using FluentValidation;

namespace CloudStorage.Application.Resources.CreateFolder;

internal sealed class CreateFolderValidator : AbstractValidator<CreateFolderCommand>
{
    public CreateFolderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(128);
    }
}