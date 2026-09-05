using FluentValidation;

namespace CloudStorage.Application.Resources.GetDownloadUrl;

internal sealed class GetDownloadUrlValidator : AbstractValidator<GetDownloadUrlQuery>
{
    public GetDownloadUrlValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}