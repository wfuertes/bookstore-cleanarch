using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bookstore.Infrastructure.Data;

public class BookstoreDbContextFactory : IDesignTimeDbContextFactory<BookstoreDbContext>
{
    public BookstoreDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookstoreDbContext>();
        
        // Use MySQL for design-time (migrations)
        // Note: This will be used only for generating migrations, not at runtime
        var connectionString = "Server=localhost;Port=3306;Database=bookstore_designtime;Uid=root;Pwd=password;";
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21)));

        return new BookstoreDbContext(optionsBuilder.Options);
    }
}
