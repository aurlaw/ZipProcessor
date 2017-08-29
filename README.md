# Zip Processor

### Requirements
.NET Core 2

### Run
```
dotnet run
```

For zip file:
```
dotnet run -- PATH-TO-ZIP
```

## Publish Self-Contained

Configured for Windows 7/10  & OSX 10.11 & 10.12

```
dotnet publish -r win7-x64 -c Release
dotnet publish -r win10-x64 -c Release
dotnet publish -r osx.10.11-x64 -c Release
dotnet publish -r osx.10.12-x64 -c Release
```
