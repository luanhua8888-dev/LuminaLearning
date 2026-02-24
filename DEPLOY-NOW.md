# ?? H??NG D?N DEPLOY - LUMINA LEARNING API

## ? PACKAGE ?Ã S?N SÀNG!

Folder `publish/` ch?a t?t c? file c?n thi?t ?? deploy.

---

## ?? CH?N PLATFORM ?? DEPLOY

### 1?? **Railway.app (FREE, Nhanh nh?t - 5 phút)**

#### **Cài Railway CLI:**
```powershell
# Ch?y trong PowerShell v?i quy?n Admin
iwr https://railway.app/install.ps1 | iex
```

#### **Deploy:**
```bash
# Login Railway
railway login

# Link v?i GitHub repo (?ã push r?i)
railway link

# Deploy
railway up

# Set environment variables
railway variables set Supabase__Url="https://jbvtftlooctnfjkkosms.supabase.co"
railway variables set Supabase__Key="YOUR_KEY"
railway variables set ConnectionStrings__DefaultConnection="YOUR_CONNECTION_STRING"
```

? Railway s? t? ??ng detect .NET project và deploy!

**URL:** Railway s? cung c?p URL nh? `https://your-app.up.railway.app`

---

### 2?? **Azure App Service (Professional)**

#### **Cài Azure CLI:**
```powershell
winget install Microsoft.AzureCLI
```

#### **Deploy t? ??ng b?ng script:**
```powershell
.\deploy.ps1 -Target Azure
```

Ho?c manual:

```bash
# Login
az login

# Create Resource Group
az group create --name lumina-rg --location southeastasia

# Create App Service Plan
az appservice plan create --name lumina-plan --resource-group lumina-rg --sku B1 --is-linux

# Create Web App
az webapp create --name lumina-learning-api --resource-group lumina-rg --plan lumina-plan --runtime "DOTNET:10.0"

# Set Environment Variables
az webapp config appsettings set --name lumina-learning-api --resource-group lumina-rg --settings `
  Supabase__Url="YOUR_URL" `
  Supabase__Key="YOUR_KEY" `
  ConnectionStrings__DefaultConnection="YOUR_CONNECTION_STRING"

# Deploy t? folder publish
cd publish
Compress-Archive -Path * -DestinationPath ../publish.zip -Force
cd ..
az webapp deploy --name lumina-learning-api --resource-group lumina-rg --src-path publish.zip --type zip
```

**URL:** `https://lumina-learning-api.azurewebsites.net`

---

### 3?? **Render.com (FREE)**

#### **B??c 1:** Truy c?p https://render.com

#### **B??c 2:** New ? Web Service

#### **B??c 3:** Connect GitHub repository
- Ch?n repository: `luanhua8888-dev/LuminaLearning`

#### **B??c 4:** C?u hình:
```
Name: lumina-learning-api
Environment: Docker (ho?c .NET)
Build Command: dotnet publish -c Release -o publish
Start Command: cd publish && dotnet "Lumina Learning.dll"
```

#### **B??c 5:** Add Environment Variables:
```
Supabase__Url = https://jbvtftlooctnfjkkosms.supabase.co
Supabase__Key = YOUR_KEY
ConnectionStrings__DefaultConnection = YOUR_CONNECTION_STRING
```

**URL:** Render cung c?p URL nh? `https://lumina-learning-api.onrender.com`

---

### 4?? **Docker Container (B?t k? cloud nào)**

#### **Cài Docker Desktop:**
Download t?: https://www.docker.com/products/docker-desktop/

#### **Build và Run:**
```bash
# Build image
docker build -t lumina-learning-api .

# Run local ?? test
docker run -d -p 8080:8080 `
  -e Supabase__Url="YOUR_URL" `
  -e Supabase__Key="YOUR_KEY" `
  -e "ConnectionStrings__DefaultConnection=YOUR_CONNECTION_STRING" `
  lumina-learning-api

# Test
curl http://localhost:8080/api/health
```

#### **Push lên Docker Hub:**
```bash
# Login
docker login

# Tag
docker tag lumina-learning-api yourusername/lumina-learning-api:latest

# Push
docker push yourusername/lumina-learning-api:latest
```

Sau ?ó deploy lên:
- **Google Cloud Run**
- **AWS ECS**
- **Azure Container Instances**
- **DigitalOcean App Platform**

---

### 5?? **Vercel (v?i Docker)**

```bash
# Cài Vercel CLI
npm i -g vercel

# Deploy
vercel --docker
```

---

### 6?? **Fly.io (FREE)**

```bash
# Cài Fly CLI
powershell -Command "iwr https://fly.io/install.ps1 -useb | iex"

# Login
fly auth login

# Deploy
fly launch

# Set secrets
fly secrets set Supabase__Url="YOUR_URL"
fly secrets set Supabase__Key="YOUR_KEY"
fly secrets set ConnectionStrings__DefaultConnection="YOUR_CONNECTION_STRING"
```

---

### 7?? **IIS (Windows Server - On-premise)**

#### **B??c 1:** Copy folder `publish` vào server
```
C:\inetpub\wwwroot\lumina-learning\
```

#### **B??c 2:** Cài .NET 10 Hosting Bundle
Download t?: https://dotnet.microsoft.com/download/dotnet/10.0

#### **B??c 3:** T?o Application Pool trong IIS
- Name: LuminaLearningPool
- .NET CLR Version: No Managed Code

#### **B??c 4:** T?o Website
- Physical path: `C:\inetpub\wwwroot\lumina-learning`
- Binding: Port 80 ho?c 443

#### **B??c 5:** Set Environment Variables
Trong IIS ? Configuration Editor ? system.webServer/aspNetCore

---

## ?? KHUY?N NGH? THEO NHU C?U:

| Platform | Use Case | Cost | Speed |
|----------|----------|------|-------|
| **Railway** | Quick test, MVP | Free | ????? |
| **Render** | Side project, portfolio | Free | ???? |
| **Azure** | Enterprise, scale | Paid | ???? |
| **Fly.io** | Global CDN | Free tier | ???? |
| **Docker** | Flexibility | Variable | ??? |
| **IIS** | On-premise | Server cost | ??? |

---

## ? SAU KHI DEPLOY

### 1. Test API:
```bash
# Health check
curl https://your-api-url.com/health

# Get classrooms
curl https://your-api-url.com/api/classrooms

# Get students
curl https://your-api-url.com/api/students
```

### 2. C?u hình Custom Domain (optional)
M?i platform ??u h? tr? custom domain

### 3. Setup CI/CD
GitHub Actions workflows ?ã s?n sàng trong `.github/workflows/`

---

## ?? DEPLOY NHANH NH?T - RAILWAY:

```bash
# 1. Cài Railway
iwr https://railway.app/install.ps1 | iex

# 2. Login
railway login

# 3. Init t? GitHub
railway link

# 4. Deploy
railway up

# 5. Set env vars trên Railway dashboard
# Visit: https://railway.app/dashboard
```

**?? T?ng th?i gian: ~5 phút**

---

B?n mu?n deploy lên platform nào? Tôi s? h??ng d?n chi ti?t h?n! ??
