# E-Commerce System API

## Overview

E-Commerce System is a RESTful Web API built using ASP.NET Core. The system allows users to browse products, manage shopping carts, place orders, and manage categories and products through a secure authentication and authorization system.

The project follows Clean Architecture principles and uses N-Tier Architecture to ensure maintainability, scalability, and separation of concerns.

---

## Architecture

The solution is organized into four layers:

### ECommerceSystem.APIs

Presentation Layer containing Controllers and API configuration.

### ECommerceSystem.BLL

Business Logic Layer containing:

* DTOs
* Managers
* Services
* Business Rules

### ECommerceSystem.DAL

Data Access Layer containing:

* DbContext
* Repositories
* Unit Of Work
* Entities

### ECommerceSystem.Common

Shared utilities, constants, and common models.

---

## Design Patterns

The project implements:

* Repository Pattern
* Generic Repository
* Unit Of Work Pattern
* DTO Pattern
* Result Wrapper Pattern

---

## Technologies Used

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT Authentication
* Policy-Based Authorization
* AutoMapper
* CORS
* LINQ
* Async/Await

---

## Features

### Authentication

* User Registration
* User Login
* JWT Token Generation
* Role-Based Access Control

### Categories

* Create Category
* Update Category
* Delete Category
* Get Category Details
* Get All Categories

### Products

* Create Product
* Update Product
* Delete Product
* Product Search
* Product Filtering
* Pagination

### Cart

* Add Product To Cart
* Update Quantity
* Remove Product
* View User Cart

### Orders

* Place Order
* View Order History
* Get Order Details

### Images

* Upload Product Images
* Upload Category Images

---

## Authentication

The API uses JWT Bearer Authentication.

After login, include the generated token in the request header:

Authorization: Bearer {token}

---

## API Endpoints

### Authentication

POST /api/auth/register

POST /api/auth/login

### Categories

GET /api/categories

GET /api/categories/{id}

POST /api/categories

PUT /api/categories/{id}

DELETE /api/categories/{id}

### Products

GET /api/products

GET /api/products/{id}

POST /api/products

PUT /api/products/{id}

DELETE /api/products/{id}

### Cart

POST /api/cart

GET /api/cart

PUT /api/cart

DELETE /api/cart/{productId}

### Orders

POST /api/orders

GET /api/orders

GET /api/orders/{id}

---

## Running the Project

1. Clone the repository.
2. Update the connection string in appsettings.json.
3. Apply migrations.
4. Update the database.
5. Run the project.

```bash
Update-Database
```

```bash
dotnet run
```

---





---

## Author

Karin Usama
