@echo off

set ASPNETCORE_ENVIRONMENT=Development
cd database\server

dotnet run --no-build --configuration Debug
rem dotnet watch run --configuration Debug

pause

