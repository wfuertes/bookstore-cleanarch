using Bookstore.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;
using Xunit;
using System.Linq;

namespace Bookstore.IntegrationTests.Infrastructure;

public class BookstoreWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithDatabase("bookstore_test")
        .WithUsername("root")
        .WithPassword("password")
        .Build();

    private readonly SemaphoreSlim _initializationSemaphore = new(1, 1);
    private bool _isInitialized = false;

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
        });

        builder.UseEnvironment("Testing");
        
        // Override UseInMemoryDatabase setting to ensure MySQL is used
        builder.UseSetting("UseInMemoryDatabase", "false");
    }

    public async Task InitializeAsync()
    {
        await _initializationSemaphore.WaitAsync();
        try
        {
            if (!_isInitialized)
            {
                await _mySqlContainer.StartAsync();
                
                // Ensure database schema is created
                await EnsureDatabaseCreatedAsync();
                _isInitialized = true;
            }
        }
        finally
        {
            _initializationSemaphore.Release();
        }
    }

    private async Task EnsureDatabaseCreatedAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BookstoreDbContext>();
        await context.Database.MigrateAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BookstoreDbContext>();
        
        // Recreate database from scratch to ensure clean state
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
        await base.DisposeAsync();
        _initializationSemaphore.Dispose();
    }
}
