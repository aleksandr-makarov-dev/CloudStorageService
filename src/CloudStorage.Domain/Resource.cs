namespace CloudStorage.Domain;

public sealed class Resource : Entity
{
    public string? Key { get; private set; }
    public string Name { get; private set; }
    public string? ContentType { get; private set; }
    public long? ContentLength { get; private set; }
    public bool IsFolder { get; private set; }

    public Guid? ParentId { get; private set; }
    public Resource? Parent { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? LastModifiedAtUtc { get; private set; }
    public bool IsUploaded { get; private set; }
    public DateTime? UploadedAtUtc { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAtUtc { get; private set; }

    public ICollection<Resource> Children { get; private set; } = new List<Resource>();

    private Resource()
    {
    }

    public static Resource File(Guid id, string key, string name, string contentType, long contentLength,
        Guid? parentId = null)
    {
        return new Resource
        {
            Id = id,
            Key = key,
            Name = name,
            ContentType = contentType,
            ContentLength = contentLength,
            IsFolder = false,
            ParentId = parentId,
            CreatedAtUtc = DateTime.UtcNow,
            IsUploaded = false,
            IsDeleted = false,
        };
    }

    public static Resource Folder(string name, Guid? parentId = null)
    {
        return new Resource
        {
            Id = Guid.NewGuid(),
            Name = name,
            ContentType = null,
            ContentLength = null,
            IsFolder = true,
            ParentId = parentId,
            CreatedAtUtc = DateTime.UtcNow,
            IsUploaded = false,
            IsDeleted = false
        };
    }

    public void Rename(string name)
    {
        Name = name;
        LastModifiedAtUtc = DateTime.UtcNow;
    }

    public void MarkUploaded()
    {
        var utcNow = DateTime.UtcNow;

        IsUploaded = true;
        UploadedAtUtc = utcNow;
        LastModifiedAtUtc = utcNow;
    }

    public void MarkDeleted()
    {
        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAtUtc = null;
    }
}