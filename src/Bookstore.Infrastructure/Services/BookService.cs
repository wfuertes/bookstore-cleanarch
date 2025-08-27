using Bookstore.Application.DTOs;
using Bookstore.Application.Interfaces;
using Bookstore.Application.Commands;
using Bookstore.Application.Queries;
using Bookstore.Application.Handlers;

namespace Bookstore.Infrastructure.Services;

public class BookService : IBookService
{
    private readonly BookQueryHandler _queryHandler;
    private readonly BookCommandHandler _commandHandler;

    public BookService(BookQueryHandler queryHandler, BookCommandHandler commandHandler)
    {
        _queryHandler = queryHandler;
        _commandHandler = commandHandler;
    }

    public async Task<BookDto?> GetBookByIdAsync(int id)
    {
        return await _queryHandler.Handle(new GetBookByIdQuery(id));
    }

    public async Task<BookDto?> GetBookByIsbnAsync(string isbn)
    {
        return await _queryHandler.Handle(new GetBookByIsbnQuery(isbn));
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        return await _queryHandler.Handle(new GetAllBooksQuery());
    }

    public async Task<IEnumerable<BookDto>> SearchBooksByTitleAsync(string title)
    {
        return await _queryHandler.Handle(new SearchBooksByTitleQuery(title));
    }

    public async Task<IEnumerable<BookDto>> SearchBooksByAuthorAsync(string author)
    {
        return await _queryHandler.Handle(new SearchBooksByAuthorQuery(author));
    }

    public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
    {
        var command = new CreateBookCommand(
            createBookDto.Title,
            createBookDto.Author,
            createBookDto.ISBN,
            createBookDto.Price,
            createBookDto.StockQuantity,
            createBookDto.PublishedDate
        );
        return await _commandHandler.Handle(command);
    }

    public async Task<BookDto> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
    {
        var command = new UpdateBookCommand(
            id,
            updateBookDto.Title,
            updateBookDto.Author,
            updateBookDto.Price,
            updateBookDto.StockQuantity
        );
        return await _commandHandler.Handle(command);
    }

    public async Task DeleteBookAsync(int id)
    {
        await _commandHandler.Handle(new DeleteBookCommand(id));
    }
}
