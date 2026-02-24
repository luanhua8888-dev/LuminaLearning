# Test Railway Deployment Locally
Write-Host "=== Testing Railway Configuration Locally ===" -ForegroundColor Cyan

# Set environment variables
$env:ASPNETCORE_ENVIRONMENT = "Production"
$env:PORT = "8080"

Write-Host "`nEnvironment variables set:" -ForegroundColor Green
Write-Host "  ASPNETCORE_ENVIRONMENT = $env:ASPNETCORE_ENVIRONMENT"
Write-Host "  PORT = $env:PORT"

# Build the project
Write-Host "`n=== Building project ===" -ForegroundColor Cyan
dotnet build "Lumina Learning.csproj" -c Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nBuild failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`nBuild successful!" -ForegroundColor Green

# Publish
Write-Host "`n=== Publishing project ===" -ForegroundColor Cyan
dotnet publish "Lumina Learning.csproj" -c Release -o ./publish

if ($LASTEXITCODE -ne 0) {
    Write-Host "`nPublish failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`nPublish successful!" -ForegroundColor Green

# Test if DLL exists
$dllPath = "./publish/Lumina Learning.dll"
if (Test-Path $dllPath) {
    Write-Host "`nDLL found: $dllPath" -ForegroundColor Green
} else {
    Write-Host "`nDLL not found: $dllPath" -ForegroundColor Red
    exit 1
}

# Start the application
Write-Host "`n=== Starting application ===" -ForegroundColor Cyan
Write-Host "App will run on http://localhost:8080" -ForegroundColor Yellow
Write-Host "Press Ctrl+C to stop" -ForegroundColor Yellow
Write-Host "`nOpen browser and test:" -ForegroundColor Yellow
Write-Host "  - http://localhost:8080/ping" -ForegroundColor Cyan
Write-Host "  - http://localhost:8080/health" -ForegroundColor Cyan
Write-Host ""

cd publish
dotnet "Lumina Learning.dll"
