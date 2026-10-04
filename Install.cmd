@echo off
setlocal
cd /d "%~dp0"
echo =======================================================
echo          StartPageNG - 1-Click Installer
echo =======================================================
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0install.ps1"
if %ERRORLEVEL% neq 0 (
    echo.
    echo Installation encountered an error.
    pause
) else (
    timeout /t 3 >nul
)
