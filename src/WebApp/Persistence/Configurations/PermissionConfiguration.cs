using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class PermissionConfiguration: IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.HasKey(p => p.PermissionId);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.DocumentShares)
            .WithOne(ds => ds.Permission)
            .HasForeignKey(ds => ds.PermissionId);
        
        builder.HasData(
            new PermissionEntity { PermissionId = 1, Name = "Reader" },
            new PermissionEntity { PermissionId = 2, Name = "Editor" }
        );
    }
}