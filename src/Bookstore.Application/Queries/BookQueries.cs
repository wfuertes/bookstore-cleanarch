using Bookstore.Application.DTOs;

namespace Bookstore.Application.Queries;

public record GetBookByIdQuery(int Id);
public record GetBookByIsbnQuery(string ISBN);
public record GetAllBooksQuery();
public record SearchBooksByTitleQuery(string Title);
public record SearchBooksByAuthorQuery(string Author);
