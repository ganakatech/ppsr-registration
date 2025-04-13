FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY . .
EXPOSE 5000
ENTRYPOINT ["dotnet", "ppsr-registration.dll"]
