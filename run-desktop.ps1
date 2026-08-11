$env:DOTNET_ROOT = "$env:LOCALAPPDATA\Microsoft\dotnet"
$env:Path = "$env:LOCALAPPDATA\Microsoft\dotnet;$env:Path"
Start-Process "$PSScriptRoot\axiovital-frontend\AxioVital.Desktop\bin\publish\AxioVital.Desktop.exe"
