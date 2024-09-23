
# Product Catalog API

The **Product Catalog API** is a project developed in C# using the .NET ecosystem, built with ASP.NET Core for creating RESTful APIs. It provides a robust service for managing products, including features such as creation, updating, deletion, and querying of products in the catalog. The API implements authentication and authorization using **ASP.NET Core Identity** to ensure data security, while **Entity Framework Core (EF Core)** serves as the ORM (Object-Relational Mapper) for database interaction.

## Key Technologies
- **C# and .NET**: The main language and framework used for API development.
- **ASP.NET Core**: Framework for building fast, secure, and scalable APIs.
- **ASP.NET Identity**: Used for user management, roles, and JWT (JSON Web Tokens) authentication to ensure only authenticated users can access resources.
- **Entity Framework Core (EF Core)**: ORM that simplifies the communication between C# code and a relational database, enabling efficient and secure data manipulation.
- **EF Core Migrations**: For database schema versioning, controlling structural changes and ensuring the database is always synchronized with the code versions.
- **SQL Server**: The default relational database, though the API supports other databases such as PostgreSQL or MySQL via EF Core.
- **FluentValidation**: Used for robust input validation.
- **AutoMapper**: Used to map objects, simplifying transformations between data transfer objects (DTOs) and domain models.

## Additional Features
- **Product Management**: Enables CRUD (Create, Read, Update, Delete) operations for products, with support for search and filters.
- **Pagination**: Implements paged results for queries using **PagedList**, allowing efficient handling of large datasets.
- **API Versioning**: The API supports versioning, making it easier to maintain backward compatibility as the system evolves.
- **Rate Limiting**: Prevents abuse by limiting the number of requests a user can make to the API within a specific time window.
- **Unit of Work Pattern**: Ensures all database operations are performed within a single transaction, improving data consistency and scalability.
- **Category Management**: Products can be organized into categories, making navigation and filtering easier.
- **Inventory Control**: Manages product stock, including quantity tracking and low stock alerts.
- **Logging and Auditing**: Logs all sensitive operations such as product creation, deletion, and updates, ensuring traceability.
- **Database Versioning**: Managed through **EF Core Migrations**, ensuring the database evolves alongside the application code.

## Security
The API adopts a robust security model with the following features:
- **JWT Authentication**: Ensures that only authenticated users can access the resources.
- **Role-based Authorization**: Defines specific permissions for users with different levels of access (administrators, managers, etc.).
- **Protection against CSRF and XSS**: Integrated tools to safeguard the API against common web attacks.

## Prerequisites
- **.NET 6.0 or higher**
- **SQL Server or another compatible database**
- **Migrations for database versioning**
- **FluentValidation for input validation**

## How to Use
1. Clone the repository:
   ```bash
   git clone https://github.com/kiqreis/catalog_api.git
   ```

2. Install dependencies and restore the database:
   ```bash
   dotnet restore
   dotnet ef database update
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

The API will be available at `http://localhost:5000`.

## Features in Detail:
- **Paged Results**: Supports large result sets by paginating the output.
- **API Versioning**: Allows for maintaining backward compatibility as the API evolves.
- **Rate Limiting**: Prevents abuse by controlling the number of requests over a time window.
- **Unit of Work Pattern**: Ensures that operations are managed within a single transaction.
