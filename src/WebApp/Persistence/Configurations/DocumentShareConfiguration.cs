using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class DocumentShareConfiguration: IEntityTypeConfiguration<DocumentShareEntity>
{
    public void Configure(EntityTypeBuilder<DocumentShareEntity> builder)
    {
        builder.HasKey(ds => new { ds.DocumentId, ds.AccountId });

        builder.HasOne(ds => ds.Document)
            .WithMany(d => d.SharedWith)
            .HasForeignKey(ds => ds.DocumentId);

        builder.HasOne(ds => ds.Account)
            .WithMany(a => a.SharedDocuments)
            .HasForeignKey(ds => ds.AccountId);

        builder.HasOne(ds => ds.Permission)
            .WithMany(p => p.DocumentShares)
            .HasForeignKey(ds => ds.PermissionId);
    }
}