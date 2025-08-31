# -----------------------------
# Build Stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY ScholarSync/*.csproj ./ScholarSync/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /src/ScholarSync
RUN dotnet publish -c Release -o /app

# -----------------------------
# Runtime Stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app .

# Expose port for Render (Render expects 8080)
EXPOSE 8080

# Environment variables (override from Render dashboard)
ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_DOCKER=true

ENTRYPOINT ["dotnet", "ScholarSync.dll"]
