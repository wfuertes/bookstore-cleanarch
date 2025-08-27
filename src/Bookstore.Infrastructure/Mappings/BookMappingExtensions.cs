using System.Reflection;
using Bookstore.Domain.Entities;
using Bookstore.Infrastructure.Entities;

namespace Bookstore.Infrastructure.Mappings;

public static class BookMappingExtensions
{
    public static BookEntity ToEntity(this Book domainBook)
    {
        return new BookEntity
        {
            Id = domainBook.Id,
            Title = domainBook.Title,
            Author = domainBook.Author,
            ISBN = domainBook.ISBN,
            Price = domainBook.Price,
            StockQuantity = domainBook.StockQuantity,
            PublishedDate = domainBook.PublishedDate,
            CreatedAt = domainBook.CreatedAt,
            UpdatedAt = domainBook.UpdatedAt
        };
    }

    public static Book ToDomain(this BookEntity entity)
    {
        // Create instance using reflection to bypass constructor validation
        var book = (Book)Activator.CreateInstance(typeof(Book), true)!;
        
        // Set properties using reflection (since they have private setters)
        SetPrivateProperty(book, nameof(Book.Id), entity.Id);
        SetPrivateProperty(book, nameof(Book.Title), entity.Title);
        SetPrivateProperty(book, nameof(Book.Author), entity.Author);
        SetPrivateProperty(book, nameof(Book.ISBN), entity.ISBN);
        SetPrivateProperty(book, nameof(Book.Price), entity.Price);
        SetPrivateProperty(book, nameof(Book.StockQuantity), entity.StockQuantity);
        SetPrivateProperty(book, nameof(Book.PublishedDate), entity.PublishedDate);
        SetPrivateProperty(book, nameof(Book.CreatedAt), entity.CreatedAt);
        SetPrivateProperty(book, nameof(Book.UpdatedAt), entity.UpdatedAt!);

        return book;
    }

    private static void SetPrivateProperty(object obj, string propertyName, object? value)
    {
        var property = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (property != null && property.CanWrite)
        {
            property.SetValue(obj, value);
        }
        else
        {
            // Using reflection to access backing field - this is safe in our controlled mapping scenario
            var field = obj.GetType().GetField($"<{propertyName}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(obj, value);
        }
    }
}
