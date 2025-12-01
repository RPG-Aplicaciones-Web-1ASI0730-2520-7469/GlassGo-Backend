# 🔧 GlassGo Backend API

This repository contains the **Web Backend API** of **GlassGo**, a modular service management platform developed by **RPG Startup** as part of the course **🧩 1ASI0730 – Aplicaciones Web** at **Universidad Peruana de Ciencias Aplicadas (UPC)**.

---

## 🧱 Tech Stack

- ⚙️ **ASP.NET Core 9.0** — Modern, high-performance web framework
- 🗄️ **Entity Framework Core 9.0** — ORM for database access with InMemory provider for development
- 🔐 **JWT Authentication** — Secure token-based authentication with 7-day expiration
- 🔒 **BCrypt.NET** — Password hashing for secure credential storage
- 🗂️ **MySQL / InMemory Database** — MySQL for production, InMemory for local development (no setup required)
- 📖 **Swagger / OpenAPI** — Interactive API documentation
- 🌍 **i18n Support** — Multi-language support (EN 🇬🇧, ES 🇪🇸, ES-PE 🇵🇪)
- 🏗️ **Domain-Driven Design (DDD)** — Clean architecture with Bounded Contexts
- 🧩 **Bounded Contexts** — IAM, Profiles, Payments, Service Planning, Analytics

---

## ⚙️ Project Structure

```
GlassGo-Backend/
├── GlassGo.API/                                # Main API project
│   ├── IAM/                                    # Identity & Access Management BC
│   │   ├── Domain/                             # Business logic and entities
│   │   │   ├── Model/
│   │   │   │   ├── Aggregates/
│   │   │   │   │   └── User.cs                 # User aggregate root
│   │   │   │   ├── Commands/                   # Sign-in, Sign-up, Update commands
│   │   │   │   ├── Queries/                    # Get user queries
│   │   │   │   └── ValueObjects/
│   │   │   │       ├── NotificationSettings.cs # Notification preferences
│   │   │   │       └── PaymentMethod.cs        # Payment method details
│   │   │   ├── Repositories/
│   │   │   │   └── IUserRepository.cs
│   │   │   └── Services/
│   │   │       ├── IUserCommandService.cs
│   │   │       └── IUserQueryService.cs
│   │   ├── Application/                        # Use cases and services
│   │   │   └── Internal/
│   │   │       ├── CommandServices/
│   │   │       │   └── UserCommandService.cs   # Handles sign-up, sign-in, profile updates
│   │   │       ├── QueryServices/
│   │   │       │   └── UserQueryService.cs     # Handles user queries
│   │   │       └── OutboundServices/
│   │   │           └── ITokenService.cs        # JWT token generation
│   │   ├── Infrastructure/                     # Technical implementations
│   │   │   ├── Hashing/BCrypt/
│   │   │   │   └── HashingService.cs           # Password hashing with BCrypt
│   │   │   ├── Tokens/JWT/
│   │   │   │   ├── Services/
│   │   │   │   │   └── TokenService.cs         # JWT token generation and validation
│   │   │   │   └── Configuration/
│   │   │   │       └── TokenSettings.cs        # JWT configuration (secret, expiration)
│   │   │   ├── Persistence/EFC/
│   │   │   │   ├── Repositories/
│   │   │   │   │   └── UserRepository.cs       # EF Core user repository
│   │   │   │   └── Configuration/Extensions/
│   │   │   │       └── ModelBuilderExtensions.cs # EF Core entity mappings
│   │   │   └── Pipeline/Middleware/
│   │   │       └── Attributes/
│   │   │           └── AuthorizeAttribute.cs   # Custom authorization
│   │   └── Interfaces/REST/
│   │       ├── AuthenticationController.cs     # POST /sign-in, /sign-up, /validate, /logout, /forgot-password
│   │       ├── Resources/                      # DTOs (Data Transfer Objects)
│   │       │   ├── SignInResource.cs
│   │       │   ├── SignUpResource.cs
│   │       │   ├── AuthenticatedUserResource.cs
│   │       │   ├── UserResource.cs
│   │       │   ├── ValidateTokenResource.cs
│   │       │   └── ForgotPasswordResource.cs
│   │       └── Transform/                      # Resource assemblers
│   │           └── UserResourceFromEntityAssembler.cs
│   │
│   ├── Profiles/                               # User Profile Management BC
│   │   └── Interfaces/REST/
│   │       ├── UsersController.cs              # GET/PUT /users, /settings, /stats, /history
│   │       └── ProfilesController.cs           # Admin-only profile management
│   │
│   ├── Payments/                               # Payments & Subscriptions BC
│   │   ├── Domain/
│   │   │   ├── Model/Aggregates/
│   │   │   │   ├── Payment.cs
│   │   │   │   └── Subscription.cs
│   │   │   ├── Repositories/
│   │   │   └── Services/
│   │   ├── Application/Internal/
│   │   │   ├── CommandServices/
│   │   │   └── QueryServices/
│   │   ├── Infrastructure/Persistence/EFC/
│   │   └── Interfaces/REST/
│   │       └── PaymentsController.cs
│   │
│   ├── ServicePlanning/                        # Orders & Service Planning BC
│   │   ├── Domain/Model/Aggregates/
│   │   │   └── Order.cs
│   │   └── Interfaces/REST/
│   │       └── OrdersController.cs
│   │
│   ├── Analytics/                              # Analytics & Reporting BC
│   │   ├── Domain/
│   │   │   ├── Entities/
│   │   │   │   └── Report.cs
│   │   │   ├── Interfaces/
│   │   │   │   └── IReportRepository.cs
│   │   │   └── Services/
│   │   │       └── ReportService.cs
│   │   ├── Infrastructure/
│   │   │   ├── Data/
│   │   │   │   └── DashboardAnalyticsContext.cs
│   │   │   └── Repositories/
│   │   │       └── ReportRepository.cs
│   │   └── Interfaces/REST/
│   │
│   ├── Shared/                                 # Cross-cutting concerns
│   │   ├── Domain/Repositories/
│   │   │   └── IBaseRepository.cs
│   │   ├── Infrastructure/
│   │   │   ├── Persistence/EFC/
│   │   │   │   ├── Configuration/
│   │   │   │   │   ├── AppDbContext.cs         # Main EF Core DbContext
│   │   │   │   │   └── Extensions/
│   │   │   │   │       ├── ModelBuilderExtensions.cs # Global entity configurations
│   │   │   │   │       └── StringExtensions.cs # snake_case naming convention
│   │   │   │   └── Repositories/
│   │   │   │       └── UnitOfWork.cs           # Transaction management
│   │   │   └── Interfaces/ASP/Configuration/
│   │   │       └── KebabCaseRouteNamingConvention.cs # kebab-case routing
│   │   └── Domain/Repositories/
│   │       └── IUnitOfWork.cs
│   │
│   ├── Resources/                              # Localization files
│   │   ├── SharedResource.en.resx
│   │   ├── SharedResource.es.resx
│   │   └── SharedResource.es-pe.resx
│   │
│   ├── Controllers/
│   │   ├── BaseController.cs
│   │   └── HealthController.cs
│   │
│   ├── appsettings.json                        # Development configuration
│   ├── appsettings.Development.json
│   ├── appsettings.Production.json             # Production configuration (with TokenSettings)
│   ├── Program.cs                              # Application entry point
│   └── GlassGo.API.csproj                      # Project file
│
├── Dockerfile                                  # Docker containerization
├── GlassGo.API.sln                             # Solution file
└── README.md                                   # This file
```

---

## 🚀 Run Locally

### Prerequisites
- **.NET 9.0 SDK** installed
- **(Optional) MySQL** for production database

### Installation

```bash
# Clone repository
git clone https://github.com/RPG-Aplicaciones-Web-1ASI0730-2520-7469/GlassGo-Backend.git
cd GlassGo-Backend

# Restore dependencies
dotnet restore

# Run the application (uses InMemory database by default)
dotnet run --project GlassGo.API
```

### URLs
- **API Swagger** → http://localhost:5129/swagger
- **Base API** → http://localhost:5129/api/v1

---

## 📖 API Documentation

### Authentication Endpoints (IAM)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/authentication/sign-up` | Register a new user |
| `POST` | `/api/v1/authentication/sign-in` | Login with username or email |
| `POST` | `/api/v1/authentication/validate` | Validate JWT token |
| `POST` | `/api/v1/authentication/logout` | Logout (stateless) |
| `POST` | `/api/v1/authentication/forgot-password` | Request password reset |

### User Profile Endpoints (Profiles)
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/v1/users/{id}` | Get user by ID |
| `GET` | `/api/v1/users` | Get all users (Admin only) |
| `PATCH` | `/api/v1/users/{userId}` | Update user role (Admin only) |
| `PUT` | `/api/v1/users/{userId}/profile` | Update user profile |
| `PUT` | `/api/v1/users/{userId}/settings/notifications` | Update notification settings |
| `GET` | `/api/v1/users/{userId}/settings` | Get user settings |
| `PUT` | `/api/v1/users/{userId}/settings` | Update user settings |
| `GET` | `/api/v1/users/{userId}/stats` | Get user statistics |
| `GET` | `/api/v1/users/history` | Get user history |

### Payments Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/payments` | Create payment |
| `GET` | `/api/v1/payments/{id}` | Get payment by ID |

### Orders Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/v1/orders` | Create order |
| `GET` | `/api/v1/orders` | Get all orders |

---

## 🌿 Branching Model (GitFlow)

| Branch | Description |
|--------|-------------|
| `main` | Stable production branch |
| `develop` | Active development branch |
| `feature/*` | Module or feature branches |

See **CONTRIBUTING.md** for full collaboration and commit guidelines.

---

## 🔧 Configuration

### Development (appsettings.Development.json)
```json
{
  "TokenSettings": {
    "Secret": "ThisIsASecretKeyForGlassGoApplicationJWT2024",
    "ExpirationInDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Production (appsettings.Production.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=%DATABASE_URL%;port=%DATABASE_PORT%;user=%DATABASE_USER%;password=%DATABASE_PASSWORD%;database=%DATABASE_SCHEMA%"
  },
  "TokenSettings": {
    "Secret": "GlassGoSuperSecretKeyForJWTTokenGeneration2024MinLength32Chars",
    "ExpirationInDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

---

## 🐳 Docker Deployment

```bash
# Build Docker image
docker build -t glassgo-backend .

# Run container
docker run -p 8080:8080 glassgo-backend
```

---

## 🧠 License

Licensed under the **MIT License** © 2025 **RPG-Aplicaciones-Web-1ASI0730-2520-7469**.

---

## 👥 Authors — RPG Startup Team

| Name | ID |
|------|------|
| Ever Giusephi Carlos Lavado | u202224867 |
| Gerardo Valentín Palacín Lazo | u20211C201 |
| Guillermo Arturo Howard Robles | u202222275 |
| Abraam Bernabe Acosta Elera | u202219199 |
| David Ignacio Vivar Cesar | u202414424 |
| Mike Dylan Guillen Giraldo | u202211881 |

---

## 📞 Support

For questions or issues, please open an issue in the [GitHub repository](https://github.com/RPG-Aplicaciones-Web-1ASI0730-2520-7469/GlassGo-Backend).
