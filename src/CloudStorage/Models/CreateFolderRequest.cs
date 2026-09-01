namespace CloudStorage.Models;

public sealed record CreateFolderRequest
{
    public string Name { get; init; }
};