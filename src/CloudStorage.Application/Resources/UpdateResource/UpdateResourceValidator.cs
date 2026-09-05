using FluentValidation;

namespace CloudStorage.Application.Resources.UpdateResource;

internal sealed class UpdateResourceValidator : AbstractValidator<UpdateResourceRequest>
{
    public UpdateResourceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(128);
    }
}