# Bookstore API - Clean Architecture

[![Build Status](https://github.com/YOUR_USERNAME/bookstore-dotnet/workflows/Build%20Status/badge.svg)](https://github.com/YOUR_USERNAME/bookstore-dotnet/actions)
[![CI/CD Pipeline](https://github.com/YOUR_USERNAME/bookstore-dotnet/workflows/CI/CD%20Pipeline/badge.svg)](https://github.com/YOUR_USERNAME/bookstore-dotnet/actions)

This project demonstrates Clean Architecture principles using .NET 8 and ASP.NET Core.

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
│   │   ├── /Data                  # Repository implementations, DbContext
│   │   ├── /Services              # Application service implementations
│   │   └── /ExternalApis          # Third-party API integrations
│   └── /Bookstore.Web             # Presentation layer (API controllers)
│       ├── /Controllers           # REST API controllers
│       ├── /MinimalEndpoints      # Minimal API endpoints (alternative to controllers)
│       ├── /Middleware            # Custom middleware
│       ├── /Program.cs            # Application startup and configuration
│       └── /appsettings.json      # Configuration files
└── /tests
    ├── /Bookstore.UnitTests       # Unit tests for Domain and Application layers
    └── /Bookstore.IntegrationTests # Integration tests for the full stack
```

## Clean Architecture Principles

### 1. Dependency Inversion
- **Domain Layer**: No dependencies on other layers
- **Application Layer**: Depends only on Domain
- **Infrastructure Layer**: Depends on Domain and Application
- **Web Layer**: Depends on all other layers

### 2. Separation of Concerns
- **Domain**: Business entities, business rules, domain services
- **Application**: Application services, CQRS commands/queries, DTOs
- **Infrastructure**: Data access, external services, technical implementations
- **Web**: Controllers, API endpoints, request/response handling

### 3. Testability
- Domain and Application layers are easily unit testable
- Infrastructure can be mocked for testing
- Integration tests cover the full stack

## Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 or VS Code

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

### Running Tests

```bash
# Run all tests
dotnet test

# Run unit tests only
dotnet test tests/Bookstore.UnitTests

# Run integration tests only
dotnet test tests/Bookstore.IntegrationTests
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

## Next Steps

To further develop this project, consider adding:

1. **Entity Framework Core** for database persistence
2. **FluentValidation** for comprehensive input validation
3. **MediatR** for better CQRS implementation
4. **Authentication & Authorization** (JWT, Identity)
5. **Logging** (Serilog, NLog)
6. **Error Handling Middleware**
7. **API Versioning**
8. **Docker containerization**
9. **CI/CD pipeline setup**

## Benefits of This Architecture

- **Maintainability**: Clear separation of concerns makes code easier to maintain
- **Testability**: Business logic is isolated and easily testable
- **Flexibility**: Easy to swap out infrastructure components
- **Scalability**: Architecture supports growth and changing requirements
- **Independence**: UI, database, and frameworks can be changed independently
