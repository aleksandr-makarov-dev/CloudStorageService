namespace CloudStorage.Domain;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTime? DeletedAtUtc { get; }

    void MarkDeleted(DateTime deletedAtUtc);
    void Restore();
}