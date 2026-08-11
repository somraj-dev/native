<#
.SYNOPSIS
    Builds, publishes, and packages AxioVital.Desktop for standalone execution.
    Generates resources.pri manually using makepri.exe since dotnet CLI lacks
    the Visual Studio MSBuild tasks required for automatic PRI generation.

.DESCRIPTION
    This script:
    1. Cleans previous build artifacts
    2. Runs dotnet publish (self-contained, Release, win-x64)
    3. Copies compiled XBF (XAML Binary Format) files to the publish directory
    4. Generates resources.pri using Windows SDK makepri.exe
    Without resources.pri and XBF files, WinUI 3 crashes with 0xc000027b
    (STATUS_STOWED_EXCEPTION) because it cannot locate XAML resource definitions.
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
    Write-Host "[1/4] Cleaning previous build artifacts..." -ForegroundColor Yellow
    $objDir = Join-Path $projectDir "obj"
    $binDir = Join-Path $projectDir "bin"
    if (Test-Path $objDir) { Remove-Item $objDir -Recurse -Force }
    if (Test-Path $binDir) { Remove-Item $binDir -Recurse -Force }
    Write-Host "       Clean complete." -ForegroundColor Green
} else {
    Write-Host "[1/4] Skipping clean (--SkipClean)" -ForegroundColor DarkGray
}

# --- Step 2: Publish ---
Write-Host "[2/4] Publishing AxioVital.Desktop ($Configuration | $RuntimeId | self-contained)..." -ForegroundColor Yellow
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

# --- Step 3: Copy XBF files ---
Write-Host "[3/4] Copying compiled XAML (XBF) files to publish directory..." -ForegroundColor Yellow

# Find the obj directory containing XBF files for this configuration
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

if ($null -eq $xbfSourceDir) {
    Write-Host "WARNING: No XBF files found in obj directories. XAML may not load correctly." -ForegroundColor Red
    Write-Host "         Searched: $($xbfSearchPaths -join ', ')" -ForegroundColor DarkGray
} else {
    Write-Host "       Found XBF source: $xbfSourceDir" -ForegroundColor DarkGray
    $xbfFiles = Get-ChildItem $xbfSourceDir -Filter "*.xbf" -Recurse
    foreach ($xbf in $xbfFiles) {
        # Compute relative path from the xbfSourceDir
        $relativePath = $xbf.FullName.Substring($xbfSourceDir.Length).TrimStart('\', '/')
        $destPath = Join-Path $publishDir $relativePath
        $destDir = Split-Path $destPath -Parent
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        Copy-Item $xbf.FullName $destPath -Force
        Write-Host "       Copied: $relativePath" -ForegroundColor DarkGray
    }
    Write-Host "       Copied $($xbfFiles.Count) XBF file(s)." -ForegroundColor Green
}

# --- Step 4: Generate resources.pri ---
Write-Host "[4/4] Generating resources.pri using makepri.exe..." -ForegroundColor Yellow

# Find makepri.exe from Windows SDK
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

if ($null -eq $makePriPath) {
    Write-Host "WARNING: makepri.exe not found in Windows SDK. resources.pri not generated." -ForegroundColor Red
    Write-Host "         Install Windows SDK (Desktop development with C++ workload) to enable PRI generation." -ForegroundColor DarkGray
} else {
    Write-Host "       Using: $makePriPath" -ForegroundColor DarkGray

    # Create a staging directory with ONLY our XBF files to avoid
    # duplicate entry errors from NuGet PRI files in the publish dir
    $priStagingDir = Join-Path $projectDir "bin\_pri_staging"
    if (Test-Path $priStagingDir) { Remove-Item $priStagingDir -Recurse -Force }
    New-Item -ItemType Directory -Path $priStagingDir -Force | Out-Null

    # Copy only our app's XBF files to staging
    if ($null -ne $xbfSourceDir) {
        $xbfFiles = Get-ChildItem $xbfSourceDir -Filter "*.xbf" -Recurse
        foreach ($xbf in $xbfFiles) {
            $relativePath = $xbf.FullName.Substring($xbfSourceDir.Length).TrimStart('\', '/')
            $destPath = Join-Path $priStagingDir $relativePath
            $destDir = Split-Path $destPath -Parent
            if (-not (Test-Path $destDir)) {
                New-Item -ItemType Directory -Path $destDir -Force | Out-Null
            }
            Copy-Item $xbf.FullName $destPath -Force
        }
    }

    # Write a minimal priconfig.xml that works with makepri
    $priConfigPath = Join-Path $priStagingDir "priconfig.xml"

    # Use makepri createconfig to generate a valid config in staging dir
    Push-Location $priStagingDir
    & $makePriPath createconfig /cf $priConfigPath /dq en-US /o 2>&1 | Out-Null
    Pop-Location

    if (-not (Test-Path $priConfigPath)) {
        Write-Host "WARNING: Failed to create priconfig.xml via makepri createconfig" -ForegroundColor Red
    } else {
        # Run makepri new on staging directory
        $priOutputPath = Join-Path $publishDir "resources.pri"
        Push-Location $priStagingDir
        & $makePriPath new /pr $priStagingDir /cf $priConfigPath /of $priOutputPath /o 2>&1 | ForEach-Object {
            $line = "$_" -replace "`0", ''
            if ($line.Trim()) {
                Write-Host "       $line" -ForegroundColor DarkGray
            }
        }
        Pop-Location

        if (Test-Path $priOutputPath) {
            $priSize = (Get-Item $priOutputPath).Length
            Write-Host "       Generated resources.pri ($priSize bytes)" -ForegroundColor Green
        } else {
            Write-Host "WARNING: resources.pri was not generated." -ForegroundColor Red
        }
    }

    # Clean up staging
    Remove-Item $priStagingDir -Recurse -Force -ErrorAction SilentlyContinue
}

# --- Done ---
Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "  Build Complete!" -ForegroundColor Green
Write-Host "  Output: $publishDir" -ForegroundColor Cyan
Write-Host "  Run:    $publishDir\AxioVital.Desktop.exe" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
