FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["ppsr-registration/ppsr-registration.csproj", "ppsr-registration/"]
RUN dotnet restore "ppsr-registration/ppsr-registration.csproj"
COPY . .
WORKDIR "/src/ppsr-registration"
RUN dotnet build "ppsr-registration.csproj" -c Release -o /app/build
RUN dotnet publish "ppsr-registration.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ppsr-registration.dll"]
