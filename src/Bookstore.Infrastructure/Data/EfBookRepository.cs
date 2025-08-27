using Bookstore.Domain.Entities;
using Bookstore.Domain.Interfaces;
using Bookstore.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Data;

public class EfBookRepository : IBookRepository
{
    private readonly BookstoreDbContext _context;

    public EfBookRepository(BookstoreDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        var entity = await _context.Books.FindAsync(id);
        return entity?.ToDomain();
    }

    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        var entity = await _context.Books
            .FirstOrDefaultAsync(b => b.ISBN == isbn);
        return entity?.ToDomain();
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        var entities = await _context.Books.ToListAsync();
        return entities.Select(e => e.ToDomain());
    }

    public async Task<IEnumerable<Book>> SearchByTitleAsync(string title)
    {
        var entities = await _context.Books
            .Where(b => b.Title.Contains(title))
            .ToListAsync();
        return entities.Select(e => e.ToDomain());
    }

    public async Task<IEnumerable<Book>> SearchByAuthorAsync(string author)
    {
        var entities = await _context.Books
            .Where(b => b.Author.Contains(author))
            .ToListAsync();
        return entities.Select(e => e.ToDomain());
    }

    public async Task<Book> AddAsync(Book book)
    {
        var entity = book.ToEntity();
        entity.Id = 0; // Let EF generate the ID
        
        _context.Books.Add(entity);
        await _context.SaveChangesAsync();
        
        return entity.ToDomain();
    }

    public async Task UpdateAsync(Book book)
    {
        var entity = book.ToEntity();
        _context.Books.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Books.FindAsync(id);
        if (entity != null)
        {
            _context.Books.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
