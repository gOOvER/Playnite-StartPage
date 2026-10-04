<#
.SYNOPSIS
    Installs StartPageNG to Playnite and restarts Playnite if running.
#>
[CmdletBinding()]
param(
    [switch]$NoRestart = $false
)

$ErrorActionPreference = "Stop"

$scriptDir = $PSScriptRoot
$sourceDir = Join-Path $scriptDir "source\StartPage\bin\Release\net462"
$targetDir = Join-Path $env:APPDATA "Playnite\Extensions\goover_StartPageNG_Plugin"
$pextPath = Join-Path $scriptDir "source\StartPage\bin\goover_StartPageNG_Plugin_1_0_0.pext"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "     StartPageNG Extension Installer    " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 1. Build if needed
if (-not (Test-Path (Join-Path $sourceDir "StartPagePlugin.dll"))) {
    Write-Host "Building StartPageNG in Release mode..." -ForegroundColor Yellow
    dotnet build (Join-Path $scriptDir "source\StartPage\StartPage.csproj") -c Release
}

# 2. Check if Playnite is running and stop it
$playniteProcesses = Get-Process -Name "Playnite.DesktopApp", "Playnite.FullscreenApp" -ErrorAction SilentlyContinue
$wasRunning = $false
if ($playniteProcesses) {
    $wasRunning = $true
    Write-Host "Playnite is running. Closing Playnite to update files..." -ForegroundColor Yellow
    $playniteProcesses | Stop-Process -Force
    Start-Sleep -Seconds 1
}

# 3. Clean and deploy to Extensions directory
Write-Host "Installing to: $targetDir" -ForegroundColor Green
if (-not (Test-Path $targetDir)) {
    New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
} else {
    Get-ChildItem -Path $targetDir -Recurse | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
}

Copy-Item -Path "$sourceDir\*" -Destination $targetDir -Recurse -Force
Write-Host "StartPageNG installed successfully to Playnite Extensions!" -ForegroundColor Green

# 3b. Copy .pext installer to dist and root if present
if (Test-Path $pextPath) {
    $distDir = Join-Path $scriptDir "dist"
    if (-not (Test-Path $distDir)) { New-Item -ItemType Directory -Path $distDir -Force | Out-Null }
    Copy-Item $pextPath -Destination (Join-Path $distDir "StartPageNG_1.0.0.pext") -Force
    Copy-Item $pextPath -Destination (Join-Path $scriptDir "StartPageNG_1.0.0.pext") -Force
}

# 4. Restart Playnite if it was running
if ($wasRunning -and -not $NoRestart) {
    Write-Host "Relaunching Playnite..." -ForegroundColor Cyan
    $playniteExe = Join-Path $env:LOCALAPPDATA "Playnite\Playnite.DesktopApp.exe"
    if (Test-Path $playniteExe) {
        Start-Process $playniteExe
    } else {
        Write-Host "Please start Playnite from your shortcut." -ForegroundColor Gray
    }
}

Write-Host "Done!" -ForegroundColor Green
