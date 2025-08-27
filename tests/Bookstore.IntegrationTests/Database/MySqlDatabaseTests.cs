using Bookstore.Domain.Entities;
using Bookstore.Infrastructure.Data;
using Bookstore.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;
using Xunit;

namespace Bookstore.IntegrationTests.Database;

public class MySqlDatabaseTests : IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithDatabase("bookstore_db_test")
        .WithUsername("testuser")
        .WithPassword("testpass")
        .Build();

    private BookstoreDbContext? _context;

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
        
        var options = new DbContextOptionsBuilder<BookstoreDbContext>()
            .UseMySql(_mySqlContainer.GetConnectionString(), 
                new MySqlServerVersion(new Version(8, 0, 21)))
            .Options;

        _context = new BookstoreDbContext(options);
        await _context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
        await _mySqlContainer.DisposeAsync();
    }

    [Fact]
    public async Task Database_ShouldCreateAndQueryBooks_Successfully()
    {
        // Arrange
        Assert.NotNull(_context);
        
        var book = new Book(
            "Test Book",
            "Test Author", 
            "978-0123456789",
            29.99m,
            10,
            DateTime.UtcNow
        );

        // Act - Add book through EF
        var bookEntity = book.ToEntity();
        _context.Books.Add(bookEntity);
        await _context.SaveChangesAsync();

        // Assert - Query book back
        var retrievedEntity = await _context.Books
            .FirstOrDefaultAsync(b => b.ISBN == "978-0123456789");
        
        Assert.NotNull(retrievedEntity);
        Assert.Equal("Test Book", retrievedEntity.Title);
        Assert.Equal("Test Author", retrievedEntity.Author);
        Assert.Equal(29.99m, retrievedEntity.Price);
    }

    [Fact]
    public async Task Database_ShouldEnforceUniqueISBN_Constraint()
    {
        // Arrange
        Assert.NotNull(_context);
        
        var book1 = new Book("Book 1", "Author 1", "978-0123456789", 29.99m, 10, DateTime.UtcNow);
        var book2 = new Book("Book 2", "Author 2", "978-0123456789", 39.99m, 5, DateTime.UtcNow);

        // Act & Assert
        _context.Books.Add(book1.ToEntity());
        await _context.SaveChangesAsync();

        _context.Books.Add(book2.ToEntity());
        
        // Should throw exception due to unique ISBN constraint
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await _context.SaveChangesAsync();
        });
    }

    [Fact]
    public async Task Repository_ShouldWork_WithRealDatabase()
    {
        // Arrange
        Assert.NotNull(_context);
        var repository = new EfBookRepository(_context);
        
        var book = new Book(
            "Repository Test Book",
            "Repository Author", 
            "978-9876543210",
            45.99m,
            15,
            DateTime.UtcNow
        );

        // Act
        var addedBook = await repository.AddAsync(book);
        var retrievedBook = await repository.GetByIdAsync(addedBook.Id);

        // Assert
        Assert.NotNull(retrievedBook);
        Assert.Equal("Repository Test Book", retrievedBook.Title);
        Assert.Equal("Repository Author", retrievedBook.Author);
        Assert.Equal("978-9876543210", retrievedBook.ISBN);
        Assert.Equal(45.99m, retrievedBook.Price);
        Assert.Equal(15, retrievedBook.StockQuantity);
    }
}
