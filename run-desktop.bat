@echo off
powershell -NoProfile -ExecutionPolicy Bypass -Command "Get-ChildItem -Path '%~dp0axiovital-frontend\AxioVital.Desktop\bin\publish' -Recurse | Unblock-File" 2>nul
set "DOTNET_ROOT=%LOCALAPPDATA%\Microsoft\dotnet"
set "PATH=%LOCALAPPDATA%\Microsoft\dotnet;%PATH%"
cd /d "%~dp0axiovital-frontend\AxioVital.Desktop\bin\publish"
start "" "AxioVital.Desktop.exe"
