using Bookstore.Domain.Entities;

namespace Bookstore.Application.DTOs;

public record BookDto(
    int Id,
    string Title,
    string Author,
    string ISBN,
    decimal Price,
    int StockQuantity,
    DateTime PublishedDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateBookDto(
    string Title,
    string Author,
    string ISBN,
    decimal Price,
    int StockQuantity,
    DateTime PublishedDate
);

public record UpdateBookDto(
    string Title,
    string Author,
    decimal Price,
    int StockQuantity
);

public static class BookDtoExtensions
{
    public static BookDto ToDto(this Book book)
    {
        return new BookDto(
            book.Id,
            book.Title,
            book.Author,
            book.ISBN,
            book.Price,
            book.StockQuantity,
            book.PublishedDate,
            book.CreatedAt,
            book.UpdatedAt
        );
    }
}
