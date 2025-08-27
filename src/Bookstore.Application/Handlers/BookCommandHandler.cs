using Bookstore.Application.Commands;
using Bookstore.Application.DTOs;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Interfaces;

namespace Bookstore.Application.Handlers;

public class BookCommandHandler
{
    private readonly IBookRepository _bookRepository;

    public BookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto> Handle(CreateBookCommand command)
    {
        // Check if book with same ISBN already exists
        var existingBook = await _bookRepository.GetByIsbnAsync(command.ISBN);
        if (existingBook != null)
        {
            throw new InvalidOperationException($"A book with ISBN {command.ISBN} already exists.");
        }

        var book = new Book(
            command.Title,
            command.Author,
            command.ISBN,
            command.Price,
            command.StockQuantity,
            command.PublishedDate
        );

        var createdBook = await _bookRepository.AddAsync(book);
        return createdBook.ToDto();
    }

    public async Task<BookDto> Handle(UpdateBookCommand command)
    {
        var book = await _bookRepository.GetByIdAsync(command.Id);
        if (book == null)
            throw new ArgumentException($"Book with ID {command.Id} not found.");

        // Note: In a real implementation, you'd have update methods on the Book entity
        // For now, this is a simplified example
        await _bookRepository.UpdateAsync(book);
        return book.ToDto();
    }

    public async Task Handle(DeleteBookCommand command)
    {
        await _bookRepository.DeleteAsync(command.Id);
    }
}
