namespace CloudStorage.Application.Common;

public static class StorageKeyBuilder
{
    public static string Build(Guid objectId, DateTime createdAtUtc)
    {
        // TODO: add ownerId
        return $"{createdAtUtc:yyyy}/{createdAtUtc:MM}/{objectId}";
    }
}