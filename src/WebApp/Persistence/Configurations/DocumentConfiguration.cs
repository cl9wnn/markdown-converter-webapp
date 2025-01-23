using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<DocumentEntity>
{
    public void Configure(EntityTypeBuilder<DocumentEntity> builder)
    {
        builder.HasKey(d => d.DocumentId);
        
        builder.HasOne(d => d.Author)
            .WithMany(a => a.Documents)  
            .HasForeignKey(d => d.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);  

        builder.Property(d => d.Name)
            .HasMaxLength(100)  
            .IsRequired(true);  

        builder.Property(d => d.CreatedAt)
            .IsRequired(); 
        
        builder.HasMany(d => d.SharedWith)
            .WithOne(ds => ds.Document)
            .HasForeignKey(ds => ds.DocumentId);
    }
}
