# AxioVital Native — Development Environment Setup

## Prerequisites

1. **Windows 10/11** (Build 19041 or higher)
2. **.NET 9 SDK** (`v9.0.316` or later)
3. **Docker Desktop** (with Docker Compose v5+)
4. **Git**

## Setup Steps

### 1. Clone & Restore
```powershell
git clone <repository-url>
cd native
dotnet restore
```

### 2. Start Local Infrastructure
```powershell
docker compose up -d
```
This starts:
- **PostgreSQL 16**: `localhost:5432`
- **Redis 7**: `localhost:6379`
- **Redpanda (Kafka)**: `localhost:9092`
- **MinIO**: `localhost:9000` (Console: `localhost:9001`)

### 3. Run the Backend API
```powershell
dotnet run --project src/AxioVital.Api
```
The API will launch at `http://localhost:5000` and Swagger UI will be available at `http://localhost:5000/swagger`.

### 4. Run the WinUI 3 Desktop Application
```powershell
dotnet run --project src/AxioVital.Desktop
```

### 5. Run Tests
```powershell
dotnet test
```
