namespace Bookstore.Domain.Exceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException(int bookId) 
        : base($"Book with ID {bookId} was not found.")
    {
    }

    public BookNotFoundException(string isbn) 
        : base($"Book with ISBN {isbn} was not found.")
    {
    }
}
