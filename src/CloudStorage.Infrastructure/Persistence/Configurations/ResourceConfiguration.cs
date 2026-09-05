using CloudStorage.Application.Common;
using CloudStorage.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudStorage.Infrastructure.Persistence.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("Resources");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key)
            .HasMaxLength(128);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(x => x.ContentType)
            .HasMaxLength(32);

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Key).IsUnique();

        builder.HasIndex(x => x.IsDeleted);

        builder.HasQueryFilter(QueryFilters.SoftDelete, x => !x.IsDeleted);
    }
}