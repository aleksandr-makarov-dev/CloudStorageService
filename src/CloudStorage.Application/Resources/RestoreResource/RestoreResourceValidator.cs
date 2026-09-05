using FluentValidation;

namespace CloudStorage.Application.Resources.RestoreResource;

internal sealed class RestoreResourceValidator : AbstractValidator<RestoreResourceCommand>
{
    public RestoreResourceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}