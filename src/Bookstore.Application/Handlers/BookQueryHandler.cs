using Bookstore.Application.DTOs;
using Bookstore.Application.Queries;
using Bookstore.Domain.Interfaces;

namespace Bookstore.Application.Handlers;

public class BookQueryHandler
{
    private readonly IBookRepository _bookRepository;

    public BookQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto?> Handle(GetBookByIdQuery query)
    {
        var book = await _bookRepository.GetByIdAsync(query.Id);
        return book?.ToDto();
    }

    public async Task<BookDto?> Handle(GetBookByIsbnQuery query)
    {
        var book = await _bookRepository.GetByIsbnAsync(query.ISBN);
        return book?.ToDto();
    }

    public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery query)
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(b => b.ToDto());
    }

    public async Task<IEnumerable<BookDto>> Handle(SearchBooksByTitleQuery query)
    {
        var books = await _bookRepository.SearchByTitleAsync(query.Title);
        return books.Select(b => b.ToDto());
    }

    public async Task<IEnumerable<BookDto>> Handle(SearchBooksByAuthorQuery query)
    {
        var books = await _bookRepository.SearchByAuthorAsync(query.Author);
        return books.Select(b => b.ToDto());
    }
}
