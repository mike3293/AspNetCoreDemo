# implicit --self-contained false
dotnet publish -c Release
dotnet publish -r win-x64
dotnet publish -r osx-x64
dotnet publish -r linux-x64 --self-contained false

cd AspNetCoreDemo
dotnet restore
dotnet build --no-restore
dotnet run --no-build
cd ..