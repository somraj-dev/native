# AxioVital Native — Development Setup Script

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host " AxioVital Native — Environment Diagnostics" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan

$env:Path = "$env:LOCALAPPDATA\Microsoft\dotnet;$env:Path"

Write-Host "1. Checking .NET SDK version..."
dotnet --version

Write-Host "2. Checking Docker Desktop status..."
docker info 2>$null | Out-Null
if ($LASTEXITCODE -eq 0) {
    Write-Host "   [PASS] Docker daemon is running." -ForegroundColor Green
} else {
    Write-Host "   [WARN] Docker daemon is not running." -ForegroundColor Yellow
}

Write-Host "3. Building AxioVital Solution..."
dotnet build AxioVital.sln

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host " Diagnostics Complete" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
