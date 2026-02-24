# Railway Deployment Troubleshooting Guide

## Các l?i th??ng g?p và cách kh?c ph?c

### 1. "Deployment failed during the initialization process"

#### Nguyên nhân:
- Thi?u environment variables
- Builder configuration không ?úng
- Port binding issues
- Database connection timeout

#### Gi?i pháp:

### OPTION 1: S? d?ng Dockerfile (Khuy?n ngh?)

**B??c 1:** ??m b?o `railway.toml` ?úng:
```toml
[build]
builder = "dockerfile"
dockerfilePath = "Dockerfile.simple"

[deploy]
startCommand = "dotnet Lumina Learning.dll"
healthcheckPath = "/health"
healthcheckTimeout = 100
restartPolicyType = "on-failure"
restartPolicyMaxRetries = 10
```

**B??c 2:** Thêm Environment Variables trên Railway:

Vào Railway Dashboard ? Service ? Variables tab ? Raw Editor:

```
ASPNETCORE_ENVIRONMENT=Production
PORT=8080
ConnectionStrings__DefaultConnection=postgresql://postgres.[project-ref]:[password]@aws-0-[region].pooler.supabase.com:6543/postgres
Supabase__Url=https://[project-ref].supabase.co
Supabase__Key=your-anon-key-here
```

**L?u ý:** Thay th?:
- `[project-ref]` v?i project reference c?a Supabase
- `[password]` v?i database password
- `[region]` v?i region c?a database (ví d?: `us-east-1`)
- `your-anon-key-here` v?i anon key t? Supabase

**L?y thông tin t? Supabase:**
1. Vào https://supabase.com/dashboard/project/[your-project]/settings/database
2. Copy **Connection string** (Transaction mode)
3. Vào https://supabase.com/dashboard/project/[your-project]/settings/api
4. Copy **URL** và **anon public** key

### OPTION 2: S? d?ng Nixpacks

**B??c 1:** S?a `railway.toml`:
```toml
[build]
builder = "nixpacks"

[deploy]
startCommand = "dotnet Lumina Learning.dll"
healthcheckPath = "/health"
healthcheckTimeout = 100
restartPolicyType = "on-failure"
restartPolicyMaxRetries = 10
```

**B??c 2:** File `nixpacks.toml` ?ã ???c t?o (xem file trong project)

**B??c 3:** Thêm Environment Variables (gi?ng Option 1)

---

## 2. Ki?m tra logs ?? debug

### Xem Build Logs:
1. Vào Railway Dashboard
2. Click vào deployment failed
3. Ch?n tab **Build Logs**
4. Tìm dòng l?i màu ??

### Xem Deploy Logs:
1. Ch?n tab **Deploy Logs**
2. Xem l?i v?:
   - Port binding
   - Database connection
   - Missing environment variables

### Common error messages:

#### "Unable to connect to database"
```
? Gi?i pháp: Ki?m tra ConnectionStrings__DefaultConnection
- ??m b?o format ?úng
- Ki?m tra IP allowlist trên Supabase (allow 0.0.0.0/0 cho Railway)
```

#### "Supabase configuration is missing"
```
? Gi?i pháp: Thêm Supabase__Url và Supabase__Key vào Variables
```

#### "Failed to bind to address"
```
? Gi?i pháp: ??m b?o PORT=8080 trong variables
```

---

## 3. Quick Fix Checklist

- [ ] Railway.toml có builder = "dockerfile" ho?c "nixpacks"
- [ ] Environment variables ?ã ???c thêm ??y ??
- [ ] Supabase connection string ?úng format
- [ ] Supabase IP allowlist cho phép Railway (0.0.0.0/0)
- [ ] Health check endpoint /health ho?t ??ng
- [ ] PORT environment variable = 8080

---

## 4. Test local tr??c khi deploy

```bash
# Set environment variables
$env:ASPNETCORE_ENVIRONMENT="Production"
$env:PORT="8080"
$env:ConnectionStrings__DefaultConnection="your-connection-string"
$env:Supabase__Url="your-supabase-url"
$env:Supabase__Key="your-supabase-key"

# Run
dotnet run

# Test health endpoint
curl http://localhost:8080/health
```

---

## 5. Railway CLI Debug (Optional)

```bash
# Install Railway CLI
npm i -g @railway/cli

# Login
railway login

# Link to project
railway link

# View logs in real-time
railway logs

# Set variables via CLI
railway variables set ASPNETCORE_ENVIRONMENT=Production
railway variables set PORT=8080
```

---

## 6. N?u v?n l?i

Hãy g?i cho tôi:
1. **Build Logs** (copy text t? Railway)
2. **Deploy Logs** (copy text t? Railway)
3. **Environment Variables** (?n sensitive data)

Tôi s? giúp b?n debug chi ti?t h?n! ??
