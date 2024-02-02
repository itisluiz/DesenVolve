sqlcmd -S "localhost\SQLEXPRESS" -E -Q "USE MASTER; DROP DATABASE IF EXISTS DesenVolve;"
del /F /Q ".\Migrations\*"
dotnet ef migrations add Inicial
dotnet ef database update
