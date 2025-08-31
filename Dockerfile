# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file and restore
COPY Scholarship-Plaatform-Backend.sln ./
COPY Scholarship-Plaatform-Backend/*.csproj Scholarship-Plaatform-Backend/

RUN dotnet restore Scholarship-Plaatform-Backend.sln

# Copy the rest of the files
COPY . .

# Build the application
RUN dotnet publish Scholarship-Plaatform-Backend -c Release -o /app

# Use a smaller runtime image for production
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Expose port (match with your appsettings.json or Program.cs if different)
EXPOSE 8080

ENTRYPOINT ["dotnet", "Scholarship-Plaatform-Backend.dll"]
