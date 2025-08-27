using Microsoft.AspNetCore.Mvc;
using Bookstore.Application.DTOs;
using Bookstore.Application.Interfaces;

namespace Bookstore.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookDto>> GetBook(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();
        
        return Ok(book);
    }

    [HttpGet("isbn/{isbn}")]
    public async Task<ActionResult<BookDto>> GetBookByIsbn(string isbn)
    {
        var book = await _bookService.GetBookByIsbnAsync(isbn);
        if (book == null)
            return NotFound();
        
        return Ok(book);
    }

    [HttpGet("search/title")]
    public async Task<ActionResult<IEnumerable<BookDto>>> SearchByTitle([FromQuery] string title)
    {
        var books = await _bookService.SearchBooksByTitleAsync(title);
        return Ok(books);
    }

    [HttpGet("search/author")]
    public async Task<ActionResult<IEnumerable<BookDto>>> SearchByAuthor([FromQuery] string author)
    {
        var books = await _bookService.SearchBooksByAuthorAsync(author);
        return Ok(books);
    }

    [HttpPost]
    public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        try
        {
            var book = await _bookService.CreateBookAsync(createBookDto);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookDto>> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
    {
        try
        {
            var book = await _bookService.UpdateBookAsync(id, updateBookDto);
            return Ok(book);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookService.DeleteBookAsync(id);
        return NoContent();
    }
}
