using Bookstore.Application.Interfaces;
using Bookstore.Application.Handlers;
using Bookstore.Domain.Interfaces;
using Bookstore.Infrastructure.Data;
using Bookstore.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database
var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

if (useInMemoryDatabase)
{
    // Use in-memory database for development/testing
    builder.Services.AddDbContext<BookstoreDbContext>(options =>
        options.UseInMemoryDatabase("BookstoreInMemory"));
    builder.Services.AddScoped<IBookRepository, EfBookRepository>();
}
else
{
    // Use MySQL for production
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<BookstoreDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    builder.Services.AddScoped<IBookRepository, EfBookRepository>();
}

// Register application services
builder.Services.AddScoped<BookQueryHandler>();
builder.Services.AddScoped<BookCommandHandler>();
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();

// Apply database migrations and seed data for non-in-memory databases
if (!useInMemoryDatabase)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<BookstoreDbContext>();
        try
        {
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}

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
public partial class Program 
{ 
    protected Program() { }
}
