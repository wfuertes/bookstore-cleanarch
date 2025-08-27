using Bookstore.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;
using Xunit;

namespace Bookstore.IntegrationTests.Infrastructure;

public class BookstoreWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithDatabase("bookstore_test")
        .WithUsername("root")
        .WithPassword("password")
        .WithPortBinding(3307, 3306) // Map to different port to avoid conflicts
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            services.RemoveAll(typeof(DbContextOptions<BookstoreDbContext>));
            services.RemoveAll(typeof(BookstoreDbContext));

            // Add DbContext with test container connection string
            services.AddDbContext<BookstoreDbContext>(options =>
            {
                options.UseMySql(_mySqlContainer.GetConnectionString(), 
                    new MySqlServerVersion(new Version(8, 0, 21)));
            });

            // Ensure the database is created and migrations are applied
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BookstoreDbContext>();
            context.Database.Migrate();
        });

        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
        await base.DisposeAsync();
    }
}
