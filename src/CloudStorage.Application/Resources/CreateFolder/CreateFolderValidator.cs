using FluentValidation;

namespace CloudStorage.Application.Resources.CreateFolder;

internal sealed class CreateFolderValidator : AbstractValidator<CreateFolderRequest>
{
    public CreateFolderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(128);
    }
}