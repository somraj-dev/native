# AxioVital Native — Architecture Documentation & Subfolder Hierarchy

## Modular Directory Hierarchy

To keep a clean separation of concerns and avoid mixing desktop UI code with backend domain/API services, `src/` is cleanly divided into `frontend/` and `backend/` subfolders:

```text
AxioVital/
├── src/
│   ├── frontend/
│   │   └── AxioVital.Desktop/
│   │       ├── Views/
│   │       ├── ViewModels/
│   │       ├── Models/
│   │       ├── Services/
│   │       ├── Navigation/
│   │       ├── Controls/
│   │       ├── Resources/
│   │       ├── Assets/
│   │       └── App.xaml
│   │
│   └── backend/
│       ├── AxioVital.Api/
│       │   ├── Controllers/
│       │   ├── Middleware/
│       │   ├── Authentication/
│       │   ├── Authorization/
│       │   └── Program.cs
│       │
│       ├── AxioVital.Application/
│       │   ├── Services/
│       │   ├── Interfaces/
│       │   ├── DTOs/
│       │   ├── Commands/
│       │   └── Queries/
│       │
│       ├── AxioVital.Domain/
│       │   ├── Entities/
│       │   ├── ValueObjects/
│       │   ├── Enums/
│       │   └── Interfaces/
│       │
│       ├── AxioVital.Infrastructure/
│       │   ├── Persistence/
│       │   ├── Repositories/
│       │   ├── Authentication/
│       │   ├── Storage/
│       │   ├── Messaging/
│       │   ├── Caching/
│       │   └── Interoperability/
│       │
│       └── AxioVital.Contracts/
│           ├── Requests/
│           ├── Responses/
│           └── DTOs/
│
├── tests/
│   ├── AxioVital.UnitTests/
│   ├── AxioVital.IntegrationTests/
│   └── AxioVital.ApiTests/
│
├── infrastructure/
│   ├── docker/
│   ├── kubernetes/
│   ├── terraform/
│   └── nginx/
│
├── database/
│   ├── migrations/
│   └── scripts/
│
├── docs/
├── tools/
├── docker-compose.yml
├── Directory.Build.props
├── Directory.Packages.props
└── README.md
```

## Solution Projects Overview

| Category | Project | Target | Description |
|---|---|---|---|
| **Frontend** | `src/frontend/AxioVital.Desktop` | `net9.0-windows10.0.19041.0` | WinUI 3 + XAML + MVVM native Windows desktop client |
| **Backend** | `src/backend/AxioVital.Api` | `net9.0` | ASP.NET Core 9 Web API host |
| **Backend** | `src/backend/AxioVital.Application` | `net9.0` | Application services & CQRS business logic abstractions |
| **Backend** | `src/backend/AxioVital.Domain` | `net9.0` | Enterprise entities, value objects, domain rules |
| **Backend** | `src/backend/AxioVital.Infrastructure` | `net9.0` | EF Core 9, Security, Redis, Messaging, Storage, Interop |
| **Backend** | `src/backend/AxioVital.Contracts` | `net9.0` | Shared DTO requests/responses |
| **Tests** | `tests/AxioVital.UnitTests` | `net9.0` | Executable smoke & unit test suite |
| **Tests** | `tests/AxioVital.IntegrationTests` | `net9.0` | EF Core database integration tests |
| **Tests** | `tests/AxioVital.ApiTests` | `net9.0` | API endpoint integration tests (WebApplicationFactory) |
