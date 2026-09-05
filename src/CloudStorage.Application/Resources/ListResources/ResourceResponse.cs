namespace CloudStorage.Application.Resources.ListResources;

public record ResourceResponse(
    Guid Id,
    string Name,
    string? ContentType,
    long? ContentLength,
    bool IsFolder,
    DateTime CreatedAtUtc,
    DateTime? LastModifiedAtUtc
);