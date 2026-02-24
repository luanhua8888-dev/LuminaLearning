# ?? Railway Deployment - Troubleshooting Guide

## ? V?n ??: "Build thành công nh?ng không lên API"

### Nguyên nhân có th?:
1. App crash ngay sau khi start
2. Port configuration sai
3. Thi?u environment variables
4. Health check timeout
5. Dependency injection errors

---

## ? GI?I PHÁP - Làm theo th? t?

### B??C 1: Test local tr??c
```powershell
.\test-railway-local.ps1
```

N?u ch?y local ???c ? V?n ?? là Railway config
N?u local c?ng l?i ? V?n ?? là code

---

### B??C 2: S? d?ng Dockerfile minimal

File ?ã t?o: `Dockerfile.railway` (minimal, không có health check ph?c t?p)

**railway.toml**:
```toml
[build]
builder = "dockerfile"
dockerfilePath = "Dockerfile.railway"

[deploy]
healthcheckPath = "/ping"
healthcheckTimeout = 300
```

---

### B??C 3: Thêm Environment Variables trên Railway

**T?i thi?u:**
```
ASPNETCORE_ENVIRONMENT=Production
PORT=8080
```

**??y ?? (khi c?n database):**
```
ASPNETCORE_ENVIRONMENT=Production
PORT=8080
ConnectionStrings__DefaultConnection=postgresql://...
Supabase__Url=https://...
Supabase__Key=...
```

---

### B??C 4: Commit và Push

```powershell
git add .
git commit -m "Fix Railway deployment - use minimal config"
git push origin main
```

---

### B??C 5: Xem Deploy Logs

1. Vào Railway Dashboard
2. Click vào deployment
3. Tab **Deploy Logs**
4. Tìm các dòng:
   - `=== Starting Lumina Learning API ===`
   - `PORT environment variable: 8080`
   - `Listening on: http://0.0.0.0:8080`
   - `Application configured successfully`

**N?u th?y các dòng trên ? App ?ã start thành công!**

---

### B??C 6: Test endpoints

Sau khi deploy thành công, Railway s? cho b?n URL d?ng:
`https://luminalearning-production.up.railway.app`

Test các endpoints:
```bash
# Ping endpoint (simple)
curl https://your-app.railway.app/ping

# Health endpoint
curl https://your-app.railway.app/health

# API endpoints
curl https://your-app.railway.app/api/classrooms
```

---

## ?? Debug Checklist

### N?u app v?n không start:

- [ ] **Deploy Logs có error gì?** ? Copy paste error message
- [ ] **Environment variables ?ã set ?úng?** ? Check Railway Variables tab
- [ ] **PORT = 8080?** ? Ph?i set PORT environment variable
- [ ] **Dockerfile ?úng ch?a?** ? Dùng `Dockerfile.railway` thay vì `Dockerfile.simple`
- [ ] **railway.toml ?úng ch?a?** ? Ph?i point t?i `Dockerfile.railway`

### L?i th??ng g?p:

#### 1. "Could not load file or assembly"
```
Gi?i pháp: Rebuild và push l?i
git add .
git commit -m "Rebuild"
git push
```

#### 2. "Failed to bind to address"
```
Gi?i pháp: Set PORT=8080 trong Railway Variables
```

#### 3. "Supabase configuration is missing"
```
Gi?i pháp: App v?n ch?y ???c, ch? warning thôi.
Thêm Supabase config n?u c?n.
```

#### 4. "Database connection failed"
```
Gi?i pháp: App v?n ch?y ???c, ch? warning thôi.
Thêm database config n?u c?n.
```

---

## ?? Quick Deploy Steps

```powershell
# 1. Test local
.\test-railway-local.ps1

# 2. If working, commit
git add railway.toml Dockerfile.railway Controllers/PingController.cs
git commit -m "Railway minimal config"

# 3. Push
git push origin main

# 4. Watch Railway deploy
# Railway s? t? ??ng detect và deploy
```

---

## ?? Expected Result

**Railway Deploy Logs should show:**
```
=== Starting Lumina Learning API ===
PORT environment variable: 8080
Listening on: http://0.0.0.0:8080
Environment: Production
WARNING: Supabase not configured
WARNING: Database not configured
=== Application configured successfully ===
Starting web server...
```

**Health check:**
```bash
$ curl https://your-app.railway.app/ping
{
  "status": "alive",
  "message": "Lumina Learning API is running!",
  "timestamp": "2024-02-24T12:34:56Z",
  "environment": "Production"
}
```

---

## ?? Need More Help?

Copy và g?i cho tôi:
1. **Full Deploy Logs** t? Railway (copy text)
2. **Environment Variables** b?n ?ã set (?n sensitive data)
3. **Error message** c? th?

Tôi s? giúp debug chi ti?t! ??
