using FluentValidation;

namespace CloudStorage.Application.Resources.SoftDeleteResource;

internal sealed class SoftDeleteResourceValidator : AbstractValidator<SoftDeleteResourceCommand>
{
    public SoftDeleteResourceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}