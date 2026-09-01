namespace CloudStorage.Models;

public sealed record CreateFolderRequest
{
    public Guid? ParentId { get; init; }
    public string Name { get; init; }
};