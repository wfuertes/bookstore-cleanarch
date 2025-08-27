using Bookstore.Application.DTOs;

namespace Bookstore.Application.Interfaces;

public interface IBookService
{
    Task<BookDto?> GetBookByIdAsync(int id);
    Task<BookDto?> GetBookByIsbnAsync(string isbn);
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<IEnumerable<BookDto>> SearchBooksByTitleAsync(string title);
    Task<IEnumerable<BookDto>> SearchBooksByAuthorAsync(string author);
    Task<BookDto> CreateBookAsync(CreateBookDto createBookDto);
    Task<BookDto> UpdateBookAsync(int id, UpdateBookDto updateBookDto);
    Task DeleteBookAsync(int id);
}
