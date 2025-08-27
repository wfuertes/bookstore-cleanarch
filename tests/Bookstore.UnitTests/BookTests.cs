using Xunit;
using Bookstore.Domain.Entities;

namespace Bookstore.UnitTests;

public class BookTests
{
    [Fact]
    public void Book_Constructor_ShouldSetProperties()
    {
        // Arrange
        var title = "Test Book";
        var author = "Test Author";
        var isbn = "978-0123456789";
        var price = 29.99m;
        var stock = 10;
        var publishedDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Act
        var book = new Book(title, author, isbn, price, stock, publishedDate);

        // Assert
        Assert.Equal(title, book.Title);
        Assert.Equal(author, book.Author);
        Assert.Equal(isbn, book.ISBN);
        Assert.Equal(price, book.Price);
        Assert.Equal(stock, book.StockQuantity);
        Assert.Equal(publishedDate, book.PublishedDate);
        Assert.True(book.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void UpdatePrice_WithValidPrice_ShouldUpdatePrice()
    {
        // Arrange
        var book = new Book("Test", "Author", "ISBN", 20.00m, 5, DateTime.Now);
        var newPrice = 25.00m;

        // Act
        book.UpdatePrice(newPrice);

        // Assert
        Assert.Equal(newPrice, book.Price);
        Assert.NotNull(book.UpdatedAt);
    }

    [Fact]
    public void UpdatePrice_WithInvalidPrice_ShouldThrowException()
    {
        // Arrange
        var book = new Book("Test", "Author", "ISBN", 20.00m, 5, DateTime.Now);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => book.UpdatePrice(-5.00m));
        Assert.Throws<ArgumentException>(() => book.UpdatePrice(0));
    }

    [Fact]
    public void UpdateStock_WithValidQuantity_ShouldUpdateStock()
    {
        // Arrange
        var book = new Book("Test", "Author", "ISBN", 20.00m, 5, DateTime.Now);
        var newStock = 15;

        // Act
        book.UpdateStock(newStock);

        // Assert
        Assert.Equal(newStock, book.StockQuantity);
        Assert.NotNull(book.UpdatedAt);
    }

    [Fact]
    public void UpdateStock_WithNegativeQuantity_ShouldThrowException()
    {
        // Arrange
        var book = new Book("Test", "Author", "ISBN", 20.00m, 5, DateTime.Now);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => book.UpdateStock(-1));
    }
}
