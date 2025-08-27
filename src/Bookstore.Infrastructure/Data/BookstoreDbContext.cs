using Bookstore.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Data;

public class BookstoreDbContext : DbContext
{
    public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options)
    {
    }

    public DbSet<BookEntity> Books { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure BookEntity
        modelBuilder.Entity<BookEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Author)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Price)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            entity.Property(e => e.StockQuantity)
                .IsRequired();

            entity.Property(e => e.PublishedDate)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired(false);

            // Add unique index for ISBN
            entity.HasIndex(e => e.ISBN)
                .IsUnique()
                .HasDatabaseName("IX_Books_ISBN");
        });
    }
}
