namespace CloudStorage.Domain;

public sealed class Resource : Entity
{
    public string? Key { get; set; }
    public string Name { get; set; }
    public string? ContentType { get; set; }
    public long? ContentLength { get; set; }
    public bool IsFolder { get; set; }
    
    public Guid? ParentId { get; set; }
    public Resource? Parent { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? LastModifiedAtUtc { get; set; }
    public bool IsUploaded { get; set; }
    public DateTime? UploadedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
    
    public ICollection<Resource> Children { get; set; } = new List<Resource>();
}