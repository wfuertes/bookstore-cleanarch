using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using Xunit;
using Bookstore.Application.DTOs;
using Bookstore.IntegrationTests.Infrastructure;

namespace Bookstore.IntegrationTests;

public class BooksControllerTests : IClassFixture<BookstoreWebApplicationFactory>, IAsyncLifetime
{
    private readonly BookstoreWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public BooksControllerTests(BookstoreWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        // Reset database before each test
        await _factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
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

    [Fact]
    public async Task CreateBook_WithDuplicateISBN_ShouldReturnConflict()
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
        var content1 = new StringContent(json, Encoding.UTF8, "application/json");
        var content2 = new StringContent(json, Encoding.UTF8, "application/json");

        // Act - Create first book
        var response1 = await _client.PostAsync("/api/books", content1);
        response1.EnsureSuccessStatusCode();

        // Act - Try to create duplicate book
        var response2 = await _client.PostAsync("/api/books", content2);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Conflict, response2.StatusCode);
    }
}
