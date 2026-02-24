# Lumina Learning API - Deployment Guide

## ?? Prerequisites
- .NET 10 SDK installed
- Supabase account with database configured
- Git installed (for version control)

---

## ?? Method 1: Deploy to Azure App Service

### Step 1: Install Azure CLI
```bash
# Download and install from: https://aka.ms/installazurecliwindows
# Or use winget
winget install Microsoft.AzureCLI
```

### Step 2: Login to Azure
```bash
az login
```

### Step 3: Create Resource Group
```bash
az group create --name lumina-learning-rg --location southeastasia
```

### Step 4: Create App Service Plan
```bash
az appservice plan create --name lumina-learning-plan --resource-group lumina-learning-rg --sku B1 --is-linux
```

### Step 5: Create Web App
```bash
az webapp create --name lumina-learning-api --resource-group lumina-learning-rg --plan lumina-learning-plan --runtime "DOTNET:10.0"
```

### Step 6: Configure Environment Variables
```bash
az webapp config appsettings set --name lumina-learning-api --resource-group lumina-learning-rg --settings Supabase__Url="https://jbvtftlooctnfjkkosms.supabase.co" Supabase__Key="YOUR_SUPABASE_KEY" ConnectionStrings__DefaultConnection="YOUR_CONNECTION_STRING"
```

### Step 7: Deploy
```bash
# Publish locally first
dotnet publish -c Release -o ./publish

# Deploy using Azure CLI
az webapp deploy --name lumina-learning-api --resource-group lumina-learning-rg --src-path ./publish.zip --type zip
```

### Step 8: Enable HTTPS Only
```bash
az webapp update --name lumina-learning-api --resource-group lumina-learning-rg --https-only true
```

**Your API will be available at:** `https://lumina-learning-api.azurewebsites.net`

---

## ?? Method 2: Deploy using Docker

### Step 1: Build Docker Image
```bash
docker build -t lumina-learning-api:latest .
```

### Step 2: Run Locally (Test)
```bash
docker run -d -p 8080:8080 --name lumina-api -e Supabase__Url="YOUR_URL" -e Supabase__Key="YOUR_KEY" lumina-learning-api:latest
```

### Step 3: Push to Docker Hub
```bash
docker login
docker tag lumina-learning-api:latest yourusername/lumina-learning-api:latest
docker push yourusername/lumina-learning-api:latest
```

### Step 4: Deploy to Cloud Platforms
- **Azure Container Instances**
- **AWS ECS**
- **Google Cloud Run**
- **Railway**
- **Render**

---

## ?? Method 3: Deploy to Railway (Free Tier)

### Step 1: Install Railway CLI
```bash
# Using PowerShell
iwr https://railway.app/install.ps1 | iex
```

### Step 2: Login
```bash
railway login
```

### Step 3: Initialize Project
```bash
railway init
```

### Step 4: Set Environment Variables
```bash
railway variables set Supabase__Url="YOUR_URL"
railway variables set Supabase__Key="YOUR_KEY"
railway variables set ConnectionStrings__DefaultConnection="YOUR_CONNECTION_STRING"
```

### Step 5: Deploy
```bash
railway up
```

**Railway will provide you a URL like:** `https://your-app.up.railway.app`

---

## ??? Method 4: Deploy to IIS (Windows Server)

### Step 1: Publish Application
```bash
dotnet publish -c Release -o C:\inetpub\wwwroot\lumina-learning
```

### Step 2: Install IIS and .NET Hosting Bundle
- Install IIS via Windows Features
- Download .NET 10 Hosting Bundle from Microsoft
- Restart IIS: `iisreset`

### Step 3: Create IIS Application Pool
- Open IIS Manager
- Create new Application Pool: "LuminaLearningPool"
- Set .NET CLR Version: "No Managed Code"

### Step 4: Create Website
- Right-click Sites ? Add Website
- Site Name: "Lumina Learning API"
- Physical Path: `C:\inetpub\wwwroot\lumina-learning`
- Binding: Port 80 or 443 (HTTPS)

### Step 5: Configure web.config
IIS will auto-generate web.config, ensure it has:
```xml
<aspNetCore processPath="dotnet" arguments=".\Lumina Learning.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="inprocess" />
```

---

## ?? Security Best Practices

### 1. Use Environment Variables (Never commit secrets)
```bash
# Set in production environment
export Supabase__Url="your-url"
export Supabase__Key="your-key"
```

### 2. Enable CORS for Frontend
Add to Program.cs before app.Build():
```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://your-frontend-domain.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### 3. Add Rate Limiting
```csharp
builder.Services.AddRateLimiter(options => { ... });
```

---

## ?? Monitoring & Logging

### Application Insights (Azure)
```bash
az monitor app-insights component create --app lumina-insights --location southeastasia --resource-group lumina-learning-rg
```

### Health Checks
Your API already has `/api/health` endpoint - configure monitoring to ping this.

---

## ?? Testing After Deployment

```bash
# Test Health Endpoint
curl https://your-api-url.com/api/health

# Test Classrooms
curl https://your-api-url.com/api/classrooms

# Test Students
curl https://your-api-url.com/api/students
```

---

## ?? Troubleshooting

### Common Issues:
1. **500 Internal Server Error**
   - Check environment variables are set correctly
   - Check logs: `az webapp log tail --name lumina-learning-api --resource-group lumina-learning-rg`

2. **Database Connection Failed**
   - Verify connection string
   - Ensure Supabase allows connections from your deployment IP

3. **CORS Errors**
   - Add CORS policy in Program.cs
   - Configure allowed origins

---

## ?? CI/CD Setup (GitHub Actions)

Create `.github/workflows/deploy.yml` for automatic deployments on push to main branch.

---

## ?? Support

For issues, check:
- Application logs
- Supabase dashboard
- Azure Portal (if using Azure)
