using FluentValidation;

namespace CloudStorage.Application.Resources.CompleteUpload;

internal sealed class CompleteUploadValidator : AbstractValidator<CompleteUploadCommand>
{
    public CompleteUploadValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}