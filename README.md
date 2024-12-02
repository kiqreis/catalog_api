
# Product Catalog API

The **Product Catalog API** is a project developed in C# using **ASP.NET Core**, offering robust services for managing products, including creation, updating, deletion, and catalog querying. It implements authentication with **ASP.NET Core Identity** and uses **Entity Framework Core (EF Core)** as the ORM for database interactions.

## Key Technologies

- **C# and .NET**: Primary language and framework.
- **ASP.NET Core**: Framework for fast, secure, and scalable APIs.
- **ASP.NET Identity**: User management and JWT-based authentication.
- **Entity Framework Core (EF Core)**: Simplifies secure data manipulation with relational databases.
- **SQL Server**: Default database, with support for PostgreSQL and MySQL.
- **FluentValidation**: Robust input validation.
- **AutoMapper**: Maps between DTOs and domain models.

## Main Features

- **Product Management**: CRUD operations with search and filtering support.
- **Pagination**: Efficient handling of large datasets using **PagedList**.
- **API Versioning**: Maintains backward compatibility as the system evolves.
- **Rate Limiting**: Controls request volume to prevent abuse.
- **Unit of Work Pattern**: Ensures consistent database transactions.
- **Category Management**: Organize and filter products by categories.
- **Inventory Control**: Tracks quantities and low-stock alerts.
- **Logging and Auditing**: Logs sensitive operations like product creation, deletion, and updates.
- **Database Migrations**: Managed with **EF Core Migrations** for version control.

## Security

- **JWT Authentication**: Restricts access to authenticated users.
- **Role-Based Authorization**: Specific permissions for different access levels.
- **CSRF and XSS Protection**: Integrated tools to guard against common attacks.

## Prerequisites

- **.NET 6.0 or higher**
- **SQL Server** or another compatible database
- **EF Core Migrations**
- **FluentValidation**

## How to Use

1. Clone the repository:
   ```bash
   git clone https://github.com/kiqreis/catalog_api.git
   ```

2. Install dependencies and update the database:
   ```bash
   dotnet restore
   dotnet ef database update
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

The API will be available at `http://localhost:5000`.
