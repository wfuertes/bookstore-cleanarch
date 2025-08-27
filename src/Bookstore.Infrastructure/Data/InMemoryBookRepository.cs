using Bookstore.Domain.Entities;
using Bookstore.Domain.Interfaces;

namespace Bookstore.Infrastructure.Data;

public class InMemoryBookRepository : IBookRepository
{
    private readonly List<Book> _books = new();
    private int _nextId = 1;

    public Task<Book?> GetByIdAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }

    public Task<Book?> GetByIsbnAsync(string isbn)
    {
        var book = _books.FirstOrDefault(b => b.ISBN == isbn);
        return Task.FromResult(book);
    }

    public Task<IEnumerable<Book>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Book>>(_books);
    }

    public Task<IEnumerable<Book>> SearchByTitleAsync(string title)
    {
        var books = _books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(books);
    }

    public Task<IEnumerable<Book>> SearchByAuthorAsync(string author)
    {
        var books = _books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(books);
    }

    public Task<Book> AddAsync(Book book)
    {
        // Use reflection to set the private Id property for this in-memory implementation
        var idProperty = typeof(Book).GetProperty("Id");
        idProperty?.SetValue(book, _nextId++);
        
        _books.Add(book);
        return Task.FromResult(book);
    }

    public Task UpdateAsync(Book book)
    {
        var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
        if (existingBook != null)
        {
            var index = _books.IndexOf(existingBook);
            _books[index] = book;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            _books.Remove(book);
        }
        return Task.CompletedTask;
    }
}
