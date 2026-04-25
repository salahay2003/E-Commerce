# 🛒 E-Commerce REST API

A full-featured backend E-Commerce REST API built with **ASP.NET Core 9**, following clean architecture principles across a 3-tier structure. The system supports user authentication, product browsing, cart management, and order processing — with no UI required.

---

## 📌 Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Design Patterns & Practices](#design-patterns--practices)
- [Authentication & Authorization](#authentication--authorization)
- [API Endpoints](#api-endpoints)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Project Structure](#project-structure)

---

## ✨ Features

- 🔐 User registration and JWT-based login
- 📦 Product browsing with **filtering, search, and pagination**
- 🗂️ Category management with image support
- 🛒 Persistent cart management per user
- 📋 Order placement and order history
- 🖼️ Image upload for products and categories
- ✅ Full input validation on all endpoints
- 🔒 Role and policy-based access control

---

## 🏗️ Architecture

The project follows an **N-Tier Architecture** split into 4 projects:

```
ECommerce.API        → Controllers, middleware, HTTP layer
ECommerce.BLL        → Business logic, managers, validators, mappers
ECommerce.DAL        → DbContext, repositories, Unit of Work, EF Core
ECommerce.Common     → Shared models: DTOs, result wrappers, interfaces
```

Dependencies flow in one direction only:

```
API → BLL → DAL → Common
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core 9 |
| Database | SQL Server |
| Authentication | JWT Bearer + Microsoft Identity |
| Validation | FluentValidation |
| Language | C# 13 |

---

## 🧱 Design Patterns & Practices

### Repository Pattern
Both generic and non-generic repositories are used. The generic repository handles standard CRUD operations, while entity-specific repositories extend it with custom queries (e.g. filtered product search, cart operations).

### Unit of Work
All repositories are accessed through a single `IUnitOfWork` interface, ensuring all changes within a request are committed in one transaction.

### Result Pattern — General Response Wrapper
Every manager method returns a `GeneralResult<T>` — a consistent response wrapper that carries the data, success status, and any error information. Controllers never throw exceptions to the client; all outcomes flow through this wrapper.

```csharp
GeneralResult<ProductReadDto>.SuccessResult(data);
GeneralResult<ProductReadDto>.NotFound();
GeneralResult<ProductReadDto>.FailResult(errors);
```

### DTOs & Manual Mappers
Domain models are never exposed directly. Each entity has dedicated DTOs for different operations (Create, Edit, Read). Custom mapper classes handle all conversions between domain models and DTOs, keeping the mapping logic centralized and testable.

### External Class Configuration (Fluent API)
EF Core entity configurations are defined in **separate configuration classes** using `IEntityTypeConfiguration<T>`, rather than cluttering `OnModelCreating`. Each entity has its own config file with constraints, relationships, and column settings defined using the Fluent API.

### FluentValidation
All incoming DTOs are validated using FluentValidation validators before any business logic runs. Validators include async rules (e.g. checking name uniqueness and category existence against the database).

### Error Auto-Mapping
Validation errors from FluentValidation are automatically mapped to a structured error response using a dedicated `IErrorMapper`. This ensures consistent error formatting across all endpoints without duplicating error-handling code in every manager.

### Async Programming
All database operations, validations, and service calls are fully async throughout the entire stack.

---

## 🔐 Authentication & Authorization

### Authentication
Users must **register and login** to receive a JWT token. The token must be included in the `Authorization` header as a Bearer token for all protected endpoints.

```
Authorization: Bearer <your_token_here>
```

### Authorization Rules

| Action | Requirement |
|---|---|
| Browse products & categories | Public — no login required |
| Access cart | Must be **logged in** |
| Place & view orders | Must be **logged in** |
| Create / Edit / Delete products | Must have **Admin** account |
| Create / Edit / Delete categories | Must have **Admin** account |
| Upload images | Must be **logged in** |

> ⚠️ The `UserId` is **never passed in the request body**. It is always extracted securely from the JWT claims on the server side.

---

## 📡 API Endpoints

### Authentication
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/auth/register` | Register a new user | Public |
| POST | `/api/auth/login` | Login and receive JWT token | Public |

### Products
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/products` | Get all products | Public |
| GET | `/api/products/pagination?categoryId=&name=&pageNumber=&pageSize=` | Filter, search & paginate | Public |
| GET | `/api/products/{id}` | Get product details | Public |
| POST | `/api/products` | Create a product | Admin |
| PUT | `/api/products/{id}` | Update a product | Admin |
| DELETE | `/api/products/{id}` | Delete a product | Admin |
| POST | `/api/products/{id}/image` | Upload product image | Admin |

### Categories
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/categories` | Get all categories | Public |
| GET | `/api/categories/{id}` | Get category details | Public |
| POST | `/api/categories` | Create a category | Admin |
| PUT | `/api/categories/{id}` | Update a category | Admin |
| DELETE | `/api/categories/{id}` | Delete a category | Admin |
| POST | `/api/categories/{id}/image` | Upload category image | Admin |

### Cart
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/cart` | Get current user's cart | 🔒 User |
| POST | `/api/cart` | Add item to cart | 🔒 User |
| PUT | `/api/cart` | Update item quantity | 🔒 User |
| DELETE | `/api/cart/{productId}` | Remove item from cart | 🔒 User |

### Orders
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/orders` | Place an order from cart | 🔒 User |
| GET | `/api/orders` | View order history | 🔒 User |
| GET | `/api/orders/{id}` | Get order details | 🔒 User |

### Images
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/image/upload` | Upload an image | 🔒 User |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- [Postman](https://www.postman.com/) for testing

### Steps

1. **Clone the repository**
```bash
git clone https://github.com/your-username/ecommerce-api.git
cd ecommerce-api
```

2. **Configure the connection string**

Update `appsettings.json` in `ECommerce.API`:
```json
"ConnectionStrings": {
  "ECommerceProject": "Server=.;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. **Configure JWT settings**
```json
"JwtSettings": {
  "Issuer": "ECommerceAPI",
  "Audience": "ECommerceClient",
  "SecretKey": "your-base64-encoded-secret-key-here",
  "ExpiryMinutes": 60
}
```

4. **Apply migrations**

In Package Manager Console, set Default Project to `ECommerce.DAL`, then run:
```
Update-Database
```

5. **Run the API**
```bash
dotnet run --project ECommerce.API
```

6. **Test with Postman**

Register a user → Login → Copy the JWT token → Use it as a Bearer token in subsequent requests.

---

## ⚙️ Configuration

| Setting | Location | Description |
|---|---|---|
| Connection String | `appsettings.json` | SQL Server connection |
| JWT Secret | `appsettings.json` | Token signing key (use a long random base64 string) |
| JWT Expiry | `appsettings.json` | Token lifetime in minutes |
| Seeding | `DALServiceExtension.cs` | Auto-seeds categories and products on first run |
| File Storage | `wwwroot/Files/` | Uploaded images stored here and served statically |

---

## 📁 Project Structure

```
ECommerce/
├── ECommerce.API/
│   ├── Controllers/
│   └── Program.cs
├── ECommerce.BLL/
│   ├── Managers/
│   ├── Validators/
│   ├── Mappers/
│   └── BLLServiceExtension.cs
├── ECommerce.DAL/
│   ├── Context/
│   ├── Models/
│   ├── Configurations/
│   ├── Repositories/
│   ├── UnitOfWork/
│   └── DALServiceExtension.cs
└── ECommerce.Common/
    ├── DTOs/
    ├── Results/
    └── Interfaces/
```

---

## 🎬 Demo

> 📹 Watch the full Postman walkthrough here: **[Demo Video Link](https://youtu.be/rV4vZW1iHFs)**

