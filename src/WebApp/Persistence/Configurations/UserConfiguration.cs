using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<AccountEntity>
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
    }
}