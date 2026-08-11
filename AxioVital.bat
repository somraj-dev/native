@echo off
set "DOTNET_ROOT=%LOCALAPPDATA%\Microsoft\dotnet"
set "PATH=%LOCALAPPDATA%\Microsoft\dotnet;%PATH%"
start "" "%~dp0axiovital-frontend\AxioVital.Desktop\bin\publish\AxioVital.Desktop.exe"
