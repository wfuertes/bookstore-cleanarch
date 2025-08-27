# Bookstore API - Clean Architecture

[![Build Status](https://github.com/wfuertes/bookstore-cleanarch/workflows/Build%20Status/badge.svg)](https://github.com/wfuertes/bookstore-cleanarch/actions)
[![CI/CD Pipeline](https://github.com/wfuertes/bookstore-cleanarch/workflows/CI/CD%20Pipeline/badge.svg)](https://github.com/wfuertes/bookstore-cleanarch/actions)

This project demonstrates Clean Architecture principles using .NET 8, ASP.NET Core, Entity Framework Core with MySQL, and Testcontainers for integration testing.

## Project Structure

```
bookstore-dotnet.sln
├── /src
│   ├── /Bookstore.Domain           # Core business logic and entities
│   │   ├── /Entities              # Domain entities (Book)
│   │   ├── /Enums                 # Domain enumerations
│   │   ├── /Exceptions            # Domain-specific exceptions
│   │   └── /Interfaces            # Domain interfaces (IBookRepository)
│   ├── /Bookstore.Application     # Application business logic
│   │   ├── /Commands              # CQRS commands (CreateBook, UpdateBook, etc.)
│   │   ├── /Queries               # CQRS queries (GetBook, SearchBooks, etc.)
│   │   ├── /Handlers              # Command and query handlers
│   │   ├── /Interfaces            # Application service interfaces
│   │   ├── /DTOs                  # Data Transfer Objects
│   │   └── /Validators            # Input validation logic
│   ├── /Bookstore.Infrastructure  # External concerns (data access, services)
│   │   ├── /Data                  # Repository implementations, DbContext, Migrations
│   │   ├── /Entities              # EF Core entities (separate from domain entities)
│   │   ├── /Mappings              # Mapping between domain and EF entities
│   │   └── /Services              # Application service implementations
│   └── /Bookstore.Web             # Presentation layer (API controllers)
│       ├── /Controllers           # REST API controllers
│       ├── /Program.cs            # Application startup and configuration
│       └── /appsettings.json      # Configuration files
└── /tests
    ├── /Bookstore.UnitTests       # Unit tests for Domain and Application layers
    └── /Bookstore.IntegrationTests # Integration tests with Testcontainers
        ├── /Database              # Database-specific integration tests
        └── /Infrastructure        # Test infrastructure (WebApplicationFactory)
```

## Clean Architecture Improvements

### 1. Separation of EF Entities from Domain Entities
- **Domain Entities**: Pure business objects with private setters and business logic
- **EF Entities**: Database-focused entities with public setters for ORM mapping
- **Mapping Layer**: Extensions to convert between domain and EF entities

### 2. Database Strategy
- **Development**: In-memory database for fast development and testing
- **Production**: MySQL database with full EF Core migrations
- **Integration Tests**: Real MySQL database using Testcontainers

### 3. Testcontainers Integration
- Real database testing without external dependencies
- Automatic container lifecycle management
- Parallel test execution with isolated databases

## Getting Started

### Prerequisites
- .NET 8 SDK
- Docker (for Testcontainers and local MySQL)
- Visual Studio 2022 or VS Code

### Database Setup

#### Option 1: Local Development (In-Memory)
The application uses in-memory database by default in Development environment. No setup required.

#### Option 2: MySQL with Docker Compose
```bash
# Start MySQL and Adminer
docker-compose up -d

# Apply migrations
cd src/Bookstore.Web
dotnet ef database update
```

#### Option 3: Connect to existing MySQL
Update `appsettings.json` with your connection string and set `UseInMemoryDatabase: false`.

### Running the Application

1. **Build the solution:**
   ```bash
   dotnet build
   ```

2. **Run the Web API:**
   ```bash
   cd src/Bookstore.Web
   dotnet run
   ```

3. **Access the API:**
   - Swagger UI: `https://localhost:7xxx/swagger`
   - API Base URL: `https://localhost:7xxx/api`
   - Database Admin (if using Docker): `http://localhost:8080` (Adminer)

### Database Configuration

The application supports multiple database configurations:

#### Development (appsettings.Development.json)
```json
{
  "UseInMemoryDatabase": true
}
```

#### Production (appsettings.json)
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=bookstore;Uid=root;Pwd=password;"
  }
}
```

### Running Tests

```bash
# Run all tests (includes Testcontainers tests)
dotnet test

# Run unit tests only
dotnet test tests/Bookstore.UnitTests

# Run integration tests only (requires Docker)
dotnet test tests/Bookstore.IntegrationTests
```

### Database Migrations

```bash
# Add a new migration
cd src/Bookstore.Web
dotnet ef migrations add MigrationName --output-dir ../Bookstore.Infrastructure/Migrations

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

## API Endpoints

### Books
- `GET /api/books` - Get all books
- `GET /api/books/{id}` - Get book by ID
- `GET /api/books/isbn/{isbn}` - Get book by ISBN
- `GET /api/books/search/title?title={title}` - Search books by title
- `GET /api/books/search/author?author={author}` - Search books by author
- `POST /api/books` - Create a new book
- `PUT /api/books/{id}` - Update an existing book
- `DELETE /api/books/{id}` - Delete a book

## Example Usage

### Create a Book
```json
POST /api/books
{
  "title": "Clean Architecture",
  "author": "Robert C. Martin",
  "isbn": "978-0134494166",
  "price": 34.99,
  "stockQuantity": 50,
  "publishedDate": "2017-09-20T00:00:00Z"
}
```

### Response
```json
{
  "id": 1,
  "title": "Clean Architecture",
  "author": "Robert C. Martin",
  "isbn": "978-0134494166",
  "price": 34.99,
  "stockQuantity": 50,
  "publishedDate": "2017-09-20T00:00:00Z",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": null
}
```

## Testing Strategy

### Unit Tests
- Test domain entities and business logic
- Test application services and handlers
- Mock external dependencies

### Integration Tests with Testcontainers
- **Database Tests**: Real MySQL database operations
- **API Tests**: Full HTTP request/response cycle
- **Repository Tests**: EF Core with real database

### Benefits of Testcontainers
- ✅ Real database behavior in tests
- ✅ Automatic container lifecycle management
- ✅ Parallel test execution
- ✅ No external dependencies required
- ✅ Consistent test environment

## Architecture Benefits

### 1. Clean Separation of Concerns
- **Domain**: Pure business logic, no infrastructure dependencies
- **Application**: Orchestrates business operations
- **Infrastructure**: EF entities separate from domain entities
- **Web**: HTTP concerns only

### 2. Database Independence
- Domain entities are ORM-agnostic
- Easy to switch between database providers
- In-memory database for fast development

### 3. Comprehensive Testing
- Unit tests for business logic
- Integration tests with real database
- API tests with Testcontainers

### 4. Production-Ready Features
- Database migrations
- Connection string configuration
- Environment-specific settings
- Docker support

## Docker Support

### Development Environment
```bash
# Start MySQL for development
docker-compose up -d mysql

# Start with Adminer for database management
docker-compose up -d
```

### Testing
Testcontainers automatically manages MySQL containers for integration tests.

## Next Steps

To further enhance this project, consider adding:

1. **Authentication & Authorization** (JWT, Identity)
2. **FluentValidation** for comprehensive input validation
3. **MediatR** for better CQRS implementation
4. **Caching** (Redis, In-Memory)
5. **Logging** (Serilog)
6. **Health Checks**
7. **API Versioning**
8. **OpenAPI documentation enhancements**
9. **Performance monitoring**
10. **CI/CD pipeline with database migrations**

## Benefits of This Clean Architecture Implementation

- **Maintainability**: Clear separation between domain and infrastructure
- **Testability**: Real database testing with Testcontainers
- **Flexibility**: Easy to switch between in-memory and MySQL
- **Scalability**: Proper entity separation supports growth
- **Production-Ready**: Full database migration support
- **Developer Experience**: Fast development with in-memory database
