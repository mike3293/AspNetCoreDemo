# implicit --self-contained false
dotnet publish -c Release
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r osx-x64
dotnet publish -c Release -r linux-x64 --self-contained false