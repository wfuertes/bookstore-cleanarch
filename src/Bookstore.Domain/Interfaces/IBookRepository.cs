using Bookstore.Domain.Entities;

namespace Bookstore.Domain.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByIsbnAsync(string isbn);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> SearchByTitleAsync(string title);
    Task<IEnumerable<Book>> SearchByAuthorAsync(string author);
    Task<Book> AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
}
