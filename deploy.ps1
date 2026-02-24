# Quick Deploy Script for Lumina Learning API

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet('Azure', 'Docker', 'Local')]
    [string]$Target = 'Local'
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Lumina Learning API - Quick Deploy" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

switch ($Target) {
    'Local' {
        Write-Host "Building and running locally..." -ForegroundColor Green
        
        # Restore dependencies
        Write-Host "1. Restoring dependencies..." -ForegroundColor Yellow
        dotnet restore
        
        # Build
        Write-Host "2. Building project..." -ForegroundColor Yellow
        dotnet build -c Release
        
        # Run
        Write-Host "3. Running application..." -ForegroundColor Yellow
        Write-Host "API will be available at: http://localhost:5000" -ForegroundColor Green
        dotnet run --launch-profile http
    }
    
    'Docker' {
        Write-Host "Building Docker image..." -ForegroundColor Green
        
        # Build Docker image
        Write-Host "1. Building Docker image..." -ForegroundColor Yellow
        docker build -t lumina-learning-api:latest .
        
        # Check if container is already running
        $containerExists = docker ps -a --filter "name=lumina-api" --format "{{.Names}}"
        if ($containerExists) {
            Write-Host "2. Stopping existing container..." -ForegroundColor Yellow
            docker stop lumina-api
            docker rm lumina-api
        }
        
        # Run container
        Write-Host "3. Starting Docker container..." -ForegroundColor Yellow
        docker run -d -p 8080:8080 -p 8081:8081 `
            --name lumina-api `
            -e ASPNETCORE_ENVIRONMENT=Production `
            -e Supabase__Url=$env:SUPABASE_URL `
            -e Supabase__Key=$env:SUPABASE_KEY `
            -e "ConnectionStrings__DefaultConnection=$env:DATABASE_CONNECTION_STRING" `
            lumina-learning-api:latest
        
        Write-Host "? Docker container started!" -ForegroundColor Green
        Write-Host "API is running at: http://localhost:8080" -ForegroundColor Green
        Write-Host "View logs: docker logs -f lumina-api" -ForegroundColor Cyan
    }
    
    'Azure' {
        Write-Host "Deploying to Azure..." -ForegroundColor Green
        
        # Check if Azure CLI is installed
        $azInstalled = Get-Command az -ErrorAction SilentlyContinue
        if (-not $azInstalled) {
            Write-Host "? Azure CLI is not installed!" -ForegroundColor Red
            Write-Host "Install from: https://aka.ms/installazurecliwindows" -ForegroundColor Yellow
            exit 1
        }
        
        # Login check
        Write-Host "1. Checking Azure login..." -ForegroundColor Yellow
        $account = az account show 2>$null | ConvertFrom-Json
        if (-not $account) {
            Write-Host "Not logged in. Please login to Azure..." -ForegroundColor Yellow
            az login
        }
        
        # Variables
        $resourceGroup = "lumina-learning-rg"
        $webAppName = "lumina-learning-api"
        $location = "southeastasia"
        
        # Publish
        Write-Host "2. Publishing application..." -ForegroundColor Yellow
        dotnet publish -c Release -o ./publish
        
        # Create zip
        Write-Host "3. Creating deployment package..." -ForegroundColor Yellow
        Compress-Archive -Path ./publish/* -DestinationPath ./publish.zip -Force
        
        # Deploy
        Write-Host "4. Deploying to Azure..." -ForegroundColor Yellow
        az webapp deploy --name $webAppName --resource-group $resourceGroup --src-path ./publish.zip --type zip
        
        # Cleanup
        Remove-Item ./publish.zip
        Remove-Item ./publish -Recurse -Force
        
        Write-Host "? Deployed to Azure!" -ForegroundColor Green
        Write-Host "API URL: https://$webAppName.azurewebsites.net" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Deployment completed!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
