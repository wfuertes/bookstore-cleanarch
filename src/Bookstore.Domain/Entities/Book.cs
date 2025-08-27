namespace Bookstore.Domain.Entities;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public string ISBN { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public DateTime PublishedDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Book() { } // Required for EF Core

    public Book(string title, string author, string isbn, decimal price, int stockQuantity, DateTime publishedDate)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        Price = price;
        StockQuantity = stockQuantity;
        PublishedDate = publishedDate;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be greater than zero.");
        
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Stock quantity cannot be negative.");
        
        StockQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}
