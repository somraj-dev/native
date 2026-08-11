@echo off
set "DOTNET_ROOT=%LOCALAPPDATA%\Microsoft\dotnet"
set "PATH=%LOCALAPPDATA%\Microsoft\dotnet;%PATH%"
cd /d "%~dp0axiovital-frontend\AxioVital.Desktop\bin\publish"
start "" "%~dp0axiovital-frontend\AxioVital.Desktop\bin\publish\AxioVital.Desktop.exe"
