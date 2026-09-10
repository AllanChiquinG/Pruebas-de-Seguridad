FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY src/VulnerableApi/VulnerableApi.csproj ./VulnerableApi/
RUN dotnet restore ./VulnerableApi/VulnerableApi.csproj

COPY src/VulnerableApi/ ./VulnerableApi/
RUN dotnet publish ./VulnerableApi/VulnerableApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VulnerableApi.dll"]
