<#
.SYNOPSIS
    Builds, generates resources.pri, and publishes AxioVital.Desktop for standalone execution.
#>

param(
    [string]$Configuration = "Release",
    [string]$RuntimeId = "win-x64",
    [switch]$SkipClean
)

$ErrorActionPreference = "Stop"

# --- Paths ---
$repoRoot = Split-Path -Parent $PSScriptRoot
$projectDir = Join-Path $repoRoot "axiovital-frontend\AxioVital.Desktop"
$csprojPath = Join-Path $projectDir "AxioVital.Desktop.csproj"
$publishDir = Join-Path $projectDir "bin\publish"

# Ensure dotnet is on PATH
$env:DOTNET_ROOT = "$env:LOCALAPPDATA\Microsoft\dotnet"
$env:Path = "$env:LOCALAPPDATA\Microsoft\dotnet;$env:Path"

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  AxioVital Desktop Build Script" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# --- Step 1: Clean ---
if (-not $SkipClean) {
    Write-Host "[1/5] Cleaning previous build artifacts..." -ForegroundColor Yellow
    $objDir = Join-Path $projectDir "obj"
    $binDir = Join-Path $projectDir "bin"
    if (Test-Path $objDir) { Remove-Item $objDir -Recurse -Force }
    if (Test-Path $binDir) { Remove-Item $binDir -Recurse -Force }
    Write-Host "       Clean complete." -ForegroundColor Green
} else {
    Write-Host "[1/5] Skipping clean (--SkipClean)" -ForegroundColor DarkGray
}

# --- Step 2: Build Solution ---
Write-Host "[2/5] Building AxioVital.Desktop ($Configuration | $RuntimeId)..." -ForegroundColor Yellow
dotnet build $csprojPath -c $Configuration -r $RuntimeId
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: dotnet build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit 1
}
Write-Host "       Build complete." -ForegroundColor Green

# Locate output directory for build
$buildOutDir = Join-Path $projectDir "bin\$Configuration\net9.0-windows10.0.19041.0\$RuntimeId"
if (-not (Test-Path $buildOutDir)) {
    $buildOutDir = Join-Path $projectDir "bin\x86\$Configuration\net9.0-windows10.0.19041.0\$RuntimeId"
}

# --- Step 3: Generate resources.pri ---
Write-Host "[3/5] Generating resources.pri using makepri.exe..." -ForegroundColor Yellow

$makePriPath = $null
$sdkBinPaths = Get-ChildItem "C:\Program Files (x86)\Windows Kits\10\bin" -Directory -ErrorAction SilentlyContinue |
    Sort-Object Name -Descending
foreach ($sdkBin in $sdkBinPaths) {
    $candidate = Join-Path $sdkBin.FullName "x64\makepri.exe"
    if (Test-Path $candidate) {
        $makePriPath = $candidate
        break
    }
}

# Find XBF files
$xbfSearchPaths = @(
    (Join-Path $projectDir "obj\$Configuration\net9.0-windows10.0.19041.0\$RuntimeId"),
    (Join-Path $projectDir "obj\x86\$Configuration\net9.0-windows10.0.19041.0\$RuntimeId"),
    (Join-Path $projectDir "obj\x64\$Configuration\net9.0-windows10.0.19041.0\$RuntimeId")
)

$xbfSourceDir = $null
foreach ($searchPath in $xbfSearchPaths) {
    if (Test-Path $searchPath) {
        $xbfFiles = Get-ChildItem $searchPath -Filter "*.xbf" -Recurse -ErrorAction SilentlyContinue
        if ($xbfFiles.Count -gt 0) {
            $xbfSourceDir = $searchPath
            break
        }
    }
}

if ($null -ne $makePriPath -and $null -ne $xbfSourceDir) {
    $priStagingDir = Join-Path $projectDir "bin\_pri_staging"
    if (Test-Path $priStagingDir) { Remove-Item $priStagingDir -Recurse -Force }
    New-Item -ItemType Directory -Path $priStagingDir -Force | Out-Null

    $xbfFiles = Get-ChildItem $xbfSourceDir -Filter "*.xbf" -Recurse
    foreach ($xbf in $xbfFiles) {
        $relativePath = $xbf.FullName.Substring($xbfSourceDir.Length).TrimStart('\', '/')
        $destPath = Join-Path $priStagingDir $relativePath
        $destDir = Split-Path $destPath -Parent
        if (-not (Test-Path $destDir)) { New-Item -ItemType Directory -Path $destDir -Force | Out-Null }
        Copy-Item $xbf.FullName $destPath -Force
    }

    $priConfigPath = Join-Path $priStagingDir "priconfig.xml"
    Push-Location $priStagingDir
    & $makePriPath createconfig /cf $priConfigPath /dq en-US /o 2>&1 | Out-Null
    Pop-Location

    $priBuildPath = Join-Path $buildOutDir "resources.pri"
    Push-Location $priStagingDir
    & $makePriPath new /pr $priStagingDir /cf $priConfigPath /of $priBuildPath /in AxioVital.Desktop /o 2>&1 | ForEach-Object {
        $line = "$_" -replace "`0", ''
        if ($line.Trim()) { Write-Host "       $line" -ForegroundColor DarkGray }
    }
    Pop-Location

    Write-Host "       Generated resources.pri in build output directory." -ForegroundColor Green
    Remove-Item $priStagingDir -Recurse -Force -ErrorAction SilentlyContinue
}

# --- Step 4: Publish ---
Write-Host "[4/5] Publishing AxioVital.Desktop ($Configuration | $RuntimeId | self-contained)..." -ForegroundColor Yellow
dotnet publish $csprojPath `
    -c $Configuration `
    -r $RuntimeId `
    --self-contained true `
    -o $publishDir

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: dotnet publish failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit 1
}
Write-Host "       Publish complete." -ForegroundColor Green

# --- Step 5: Copy XBF files to publish directory ---
Write-Host "[5/5] Copying compiled XAML (XBF) files to publish directory..." -ForegroundColor Yellow
if ($null -ne $xbfSourceDir) {
    $xbfFiles = Get-ChildItem $xbfSourceDir -Filter "*.xbf" -Recurse
    foreach ($xbf in $xbfFiles) {
        $relativePath = $xbf.FullName.Substring($xbfSourceDir.Length).TrimStart('\', '/')
        $destPath = Join-Path $publishDir $relativePath
        $destDir = Split-Path $destPath -Parent
        if (-not (Test-Path $destDir)) { New-Item -ItemType Directory -Path $destDir -Force | Out-Null }
        Copy-Item $xbf.FullName $destPath -Force
    }
    Write-Host "       Copied $($xbfFiles.Count) XBF file(s)." -ForegroundColor Green
}

# --- Step 6: Create SmartScreen Unblock Launcher in Publish Folder ---
$launcherPath = Join-Path $publishDir "Run-AxioVital.bat"
$launcherContent = @"
@echo off
title AxioVital Desktop Launcher
powershell -NoProfile -ExecutionPolicy Bypass -Command "Get-ChildItem -Path '%~dp0' -Recurse | Unblock-File" 2>nul
cd /d "%~dp0"
start "" "%~dp0AxioVital.Desktop.exe"
"@
Set-Content -Path $launcherPath -Value $launcherContent -Encoding ASCII
Write-Host "       Created SmartScreen Unblock Launcher (Run-AxioVital.bat)" -ForegroundColor Green

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Build & Publish Complete!" -ForegroundColor Green
Write-Host "  Output: $publishDir" -ForegroundColor Cyan
Write-Host "  Run:    $publishDir\AxioVital.Desktop.exe" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
