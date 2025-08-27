using Bookstore.Application.Interfaces;
using Bookstore.Application.Handlers;
using Bookstore.Domain.Interfaces;
using Bookstore.Infrastructure.Data;
using Bookstore.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register dependencies
builder.Services.AddScoped<IBookRepository, InMemoryBookRepository>();
builder.Services.AddScoped<BookQueryHandler>();
builder.Services.AddScoped<BookCommandHandler>();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

// Make the Program class accessible for integration tests
public partial class Program { }
