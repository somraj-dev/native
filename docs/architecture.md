# AxioVital Native — Architecture Documentation & File Hierarchy

## Folder Hierarchy

```text
AxioVital/
├── src/
│   ├── AxioVital.Desktop/
│   │   ├── Views/
│   │   ├── ViewModels/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── Navigation/
│   │   ├── Controls/
│   │   ├── Resources/
│   │   ├── Assets/
│   │   └── App.xaml
│   │
│   ├── AxioVital.Api/
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── Authentication/
│   │   ├── Authorization/
│   │   └── Program.cs
│   │
│   ├── AxioVital.Application/
│   │   ├── Services/
│   │   ├── Interfaces/
│   │   ├── DTOs/
│   │   ├── Commands/
│   │   └── Queries/
│   │
│   ├── AxioVital.Domain/
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Enums/
│   │   └── Interfaces/
│   │
│   ├── AxioVital.Infrastructure/
│   │   ├── Persistence/
│   │   ├── Repositories/
│   │   ├── Authentication/
│   │   ├── Storage/
│   │   ├── Messaging/
│   │   ├── Caching/
│   │   └── Interoperability/
│   │
│   └── AxioVital.Contracts/
│       ├── Requests/
│       ├── Responses/
│       └── DTOs/
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

| Project | Target | Description |
|---|---|---|
| `AxioVital.Desktop` | `net9.0-windows10.0.19041.0` | WinUI 3 + XAML + MVVM native Windows desktop client |
| `AxioVital.Api` | `net9.0` | ASP.NET Core 9 Web API host |
| `AxioVital.Application` | `net9.0` | Application services & CQRS business logic abstractions |
| `AxioVital.Domain` | `net9.0` | Enterprise entities, value objects, domain rules |
| `AxioVital.Infrastructure` | `net9.0` | EF Core 9, Security, Redis, Messaging, Storage, Interop |
| `AxioVital.Contracts` | `net9.0` | Shared DTO requests/responses |
| `AxioVital.UnitTests` | `net9.0` | Executable smoke & unit test suite |
| `AxioVital.IntegrationTests` | `net9.0` | EF Core database integration tests |
| `AxioVital.ApiTests` | `net9.0` | API endpoint integration tests (WebApplicationFactory) |
