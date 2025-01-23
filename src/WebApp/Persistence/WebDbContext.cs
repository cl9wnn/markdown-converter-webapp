using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;
using Persistence.Entities;

namespace Persistence;

public class WebDbContext(DbContextOptions<WebDbContext> options): DbContext(options)
{ 
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<DocumentEntity> Documents { get; set; }
    public DbSet<PermissionEntity> Permissions { get; set; }
    public DbSet<DocumentShareEntity> DocumentShares { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentShareConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
    }
}