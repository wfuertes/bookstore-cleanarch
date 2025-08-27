using Bookstore.Application.DTOs;

namespace Bookstore.Application.Commands;

public record CreateBookCommand(
    string Title,
    string Author,
    string ISBN,
    decimal Price,
    int StockQuantity,
    DateTime PublishedDate
);

public record UpdateBookCommand(
    int Id,
    string Title,
    string Author,
    decimal Price,
    int StockQuantity
);

public record DeleteBookCommand(int Id);
