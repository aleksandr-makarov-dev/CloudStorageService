namespace CloudStorage.Services;

public static class StorageKeyBuilder
{
    public static string Build(Guid resourceId, DateTime createdAtUtc)
    {
        // TODO: add ownerId
        return $"{createdAtUtc:yyyy}/{createdAtUtc:MM}/{resourceId}";
    }
}