$env:DOTNET_ROOT = "$env:LOCALAPPDATA\Microsoft\dotnet"
$env:Path = "$env:LOCALAPPDATA\Microsoft\dotnet;$env:Path"
Start-Process "$PSScriptRoot\AxioVital-App\AxioVital.Desktop.exe"
