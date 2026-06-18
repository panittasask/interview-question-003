using ApproveDocument.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApproveDocument.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Document> Documents => Set<Document>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(x => x.DocumentName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.DocumentDetail)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(x => x.StatusReason)
                .HasMaxLength(1000);
        });
    }
}
