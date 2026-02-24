# ?? H??NG D?N DEPLOY NHANH

## ? Chu?n b?
1. ? API ?ã build thành công (.NET 10)
2. ? Database Supabase ?ã s?n sàng
3. ? ?ã ch?y SQL script t?o b?ng

---

## ?? CÁCH 1: DEPLOY LÊN AZURE (Khuy?n ngh?)

### B??c 1: Cài Azure CLI
```powershell
winget install Microsoft.AzureCLI
```

### B??c 2: Login và Deploy
```powershell
# Login
az login

# Deploy t? ??ng
.\deploy.ps1 -Target Azure
```

? **Xong!** API s? có t?i: `https://lumina-learning-api.azurewebsites.net`

---

## ?? CÁCH 2: DEPLOY B?NG DOCKER

### Option A: Docker Local
```powershell
.\deploy.ps1 -Target Docker
```
API ch?y t?i: `http://localhost:8080`

### Option B: Docker Compose
```powershell
# T?o file .env tr??c
copy .env.example .env
# S?a thông tin trong .env

# Ch?y
docker-compose up -d
```

---

## ?? CÁCH 3: DEPLOY LÊN RAILWAY (FREE)

### B??c 1: Cài Railway CLI
```powershell
iwr https://railway.app/install.ps1 | iex
```

### B??c 2: Deploy
```bash
railway login
railway init
railway up
```

### B??c 3: Set Environment Variables trên Railway Dashboard
- `Supabase__Url`
- `Supabase__Key`
- `ConnectionStrings__DefaultConnection`

---

## ?? CÁCH 4: TEST LOCAL

```powershell
.\deploy.ps1 -Target Local
```

API ch?y t?i: `http://localhost:5000`

---

## ?? KI?M TRA SAU KHI DEPLOY

```bash
# Test health endpoint
curl https://your-api-url.com/health

# Test API
curl https://your-api-url.com/api/classrooms
```

---

## ?? QUAN TR?NG: C?u hình Environment Variables

### Trên Azure
```bash
az webapp config appsettings set --name lumina-learning-api --resource-group lumina-learning-rg --settings \
  Supabase__Url="YOUR_URL" \
  Supabase__Key="YOUR_KEY" \
  ConnectionStrings__DefaultConnection="YOUR_DB"
```

### Trên Railway
Vào Settings ? Variables ? Add t?ng bi?n

### Docker
S?a file `.env` ho?c truy?n vào l?nh `-e`

---

## ?? TROUBLESHOOTING

### L?i 500 Internal Server Error
?? Ki?m tra environment variables ?ã set ch?a

### Không k?t n?i ???c database
?? Ki?m tra connection string và Supabase settings

### CORS Error
?? ?ã ???c c?u hình s?n trong Program.cs

---

## ?? FILES QUAN TR?NG

- `deploy.ps1` - Script deploy t? ??ng
- `Dockerfile` - Docker configuration
- `docker-compose.yml` - Docker Compose setup
- `.env.example` - Template cho environment variables
- `DEPLOYMENT.md` - H??ng d?n chi ti?t

---

**Chúc b?n deploy thành công! ??**
