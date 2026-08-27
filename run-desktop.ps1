$env:DOTNET_ROOT = "$env:LOCALAPPDATA\dotnet"
$env:Path = "$env:LOCALAPPDATA\dotnet;$env:LOCALAPPDATA\Microsoft\dotnet;$env:Path"
$publishDir = "$PSScriptRoot\axiovital-frontend\AxioVital.Desktop\bin\publish"
Start-Process "$publishDir\AxioVital.Desktop.exe" -WorkingDirectory $publishDir
