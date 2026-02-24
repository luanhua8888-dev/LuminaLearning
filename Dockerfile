# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Lumina Learning.csproj", "./"]
RUN dotnet restore "Lumina Learning.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "Lumina Learning.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Lumina Learning.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Use PORT environment variable or default to 8080
ENV PORT=8080
EXPOSE ${PORT}

# Copy published app
COPY --from=publish /app/publish .

# Create non-root user
RUN useradd -m -u 1001 appuser && chown -R appuser /app
USER appuser

# Health check - use PORT env var
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:${PORT}/health || exit 1

ENTRYPOINT ["dotnet", "Lumina Learning.dll"]
