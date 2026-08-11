# AxioVital Native — Healthcare Platform

Production-grade healthcare desktop solution built with **WinUI 3**, **ASP.NET Core 9**, and **PostgreSQL 16**.

## Technology Stack

- **Desktop**: WinUI 3, Windows App SDK, C#, .NET 9, XAML, MVVM
- **Backend API**: ASP.NET Core 9, REST API, Entity Framework Core 9
- **Database**: PostgreSQL 16
- **Security**: Argon2id, JWT, WebAuthn, RBAC, Multi-tenancy
- **Infrastructure**: Redis 7, Redpanda (Kafka), MinIO (S3-compatible)
- **Healthcare Interoperability**: FHIR R4, HL7 v2.x, DICOM

## Quick Start

### 1. Start Infrastructure
```powershell
docker compose up -d
```

### 2. Build Solution
```powershell
$env:Path = "$env:LOCALAPPDATA\Microsoft\dotnet;$env:Path"
dotnet build
```

### 3. Run API & Desktop App
```powershell
# In terminal 1:
dotnet run --project src/AxioVital.Api

# In terminal 2:
dotnet run --project src/AxioVital.Desktop
```

### 4. Run Tests
```powershell
dotnet test
```

## Documentation
- [Architecture Overview](docs/architecture.md)
- [Development Setup](docs/development-setup.md)
- [Security & Multi-Tenancy](docs/security.md)
- [Database & EF Core](docs/database.md)
- [Healthcare Interoperability](docs/interoperability.md)
- [Legacy Migration Assessment](docs/migration-assessment.md)
