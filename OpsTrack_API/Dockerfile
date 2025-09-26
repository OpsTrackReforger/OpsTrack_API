# ---- BUILD STAGE ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copy solution + csproj files for caching
COPY OpsTrack_API.sln .
COPY OpsTrack_API/OpsTrack_API.csproj OpsTrack_API/
COPY Domain/Domain.csproj Domain/
COPY Application/Application.csproj Application/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

# 2. Restore dependencies
RUN dotnet restore

# 3. Copy rest of the source code
COPY . .

# 4. Publish the API project
RUN dotnet publish OpsTrack_API/OpsTrack_API.csproj -c Release -o /app/publish

# ---- RUNTIME STAGE ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Create folder for SQLite
RUN mkdir -p /app/Data

EXPOSE 8080

# Start the API
ENTRYPOINT ["dotnet", "OpsTrack_API.dll"]
