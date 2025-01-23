using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class AccountConfiguration: IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.HasKey(a => a.AccountId);

        builder.Property(a => a.Email)
            .IsRequired() 
            .HasMaxLength(60);

        builder.Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(100); 

        builder.Property(a => a.PasswordHash)
            .IsRequired(); 
        
        builder.HasMany(a => a.Documents)
            .WithOne(d => d.Author)
            .HasForeignKey(d => d.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(a => a.SharedDocuments)
            .WithOne(ds => ds.Account)
            .HasForeignKey(ds => ds.AccountId);
    }
}