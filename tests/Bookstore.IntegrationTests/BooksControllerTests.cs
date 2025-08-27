using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using Xunit;
using Bookstore.Application.DTOs;

namespace Bookstore.IntegrationTests;

public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BooksControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllBooks_ShouldReturnEmptyList_Initially()
    {
        // Act
        var response = await _client.GetAsync("/api/books");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var books = JsonSerializer.Deserialize<BookDto[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.NotNull(books);
        Assert.Empty(books);
    }

    [Fact]
    public async Task CreateBook_ShouldReturnCreatedBook()
    {
        // Arrange
        var createBookDto = new CreateBookDto(
            "Test Book",
            "Test Author",
            "978-0123456789",
            29.99m,
            10,
            DateTime.UtcNow
        );

        var json = JsonSerializer.Serialize(createBookDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/books", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var book = JsonSerializer.Deserialize<BookDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(book);
        Assert.Equal(createBookDto.Title, book.Title);
        Assert.Equal(createBookDto.Author, book.Author);
        Assert.Equal(createBookDto.ISBN, book.ISBN);
        Assert.Equal(createBookDto.Price, book.Price);
    }
}
