# ?? DEPLOY LÊN RAILWAY - H??NG D?N CHI TI?T

## ? ?u ?i?m Railway
- ?? FREE tier: $5 credit/tháng
- ? Deploy c?c nhanh t? GitHub
- ?? Auto-deploy khi push code
- ?? Free SSL/HTTPS
- ?? Monitoring dashboard

---

## ?? B??C 1: T?O TÀI KHO?N

### 1.1 Truy c?p Railway
```
https://railway.app
```

### 1.2 Sign up
- Click **"Login"**
- Ch?n **"Login with GitHub"**
- Authorize Railway app

---

## ?? B??C 2: T?O PROJECT M?I

### 2.1 New Project
- Click **"New Project"**
- Ch?n **"Deploy from GitHub repo"**

### 2.2 Ch?n Repository
- Tìm và ch?n: **`luanhua8888-dev/LuminaLearning`**
- Click **"Deploy Now"**

### 2.3 Railway t? ??ng:
- ? Detect .NET 10 project
- ? Build project
- ? Deploy lên server
- ? Generate URL

?? Build time: ~2-3 phút

---

## ?? B??C 3: C?U HÌNH ENVIRONMENT VARIABLES

### 3.1 Vào Settings
- Click vào project v?a t?o
- Tab **"Variables"**

### 3.2 Add Variables
Click **"New Variable"** và thêm t?ng bi?n sau:

#### Variable 1: Supabase URL
```
Name:  Supabase__Url
Value: https://jbvtftlooctnfjkkosms.supabase.co
```

#### Variable 2: Supabase Key
```
Name:  Supabase__Key
Value: sb_publishable_SjGLScja0ByYPJlYZnWKpw_xFyB55ne
```

#### Variable 3: Connection String
```
Name:  ConnectionStrings__DefaultConnection
Value: postgresql://postgres.jbvtftlooctnfjkkosms:Test123456zaZ2122@aws-0-ap-southeast-1.pooler.supabase.com:6543/postgres?sslmode=require
```

### 3.3 L?u và Redeploy
- Click **"Save"**
- Railway s? t? ??ng redeploy v?i config m?i

---

## ?? B??C 4: L?Y URL VÀ TEST

### 4.1 Generate Domain
- Tab **"Settings"**
- Section **"Domains"**
- Click **"Generate Domain"**

Railway s? t?o URL nh?:
```
https://luminalearning-production.up.railway.app
```

### 4.2 Test API
```bash
# Health check
curl https://your-app.up.railway.app/health

# Get classrooms
curl https://your-app.up.railway.app/api/classrooms

# Get students
curl https://your-app.up.railway.app/api/students
```

---

## ?? B??C 5: MONITORING

### 5.1 View Logs
- Tab **"Deployments"**
- Click vào deployment g?n nh?t
- Xem **"Deploy Logs"** và **"Build Logs"**

### 5.2 Metrics
- Tab **"Metrics"**
- Xem CPU, Memory, Network usage

---

## ?? AUTO-DEPLOY KHI PUSH CODE

Railway ?ã t? ??ng setup CI/CD:

```bash
# M?i khi b?n push code lên GitHub
git add .
git commit -m "Update features"
git push origin main

# Railway s? t? ??ng:
# 1. Detect changes
# 2. Build l?i project
# 3. Deploy version m?i
```

---

## ?? CUSTOM DOMAIN (Optional)

### B??c 1: Add Custom Domain
- Settings ? Domains
- Click **"Custom Domain"**
- Nh?p domain c?a b?n: `api.yourdomain.com`

### B??c 2: C?u hình DNS
Railway s? cung c?p CNAME record:
```
CNAME: api.yourdomain.com ? your-app.up.railway.app
```

Add vào DNS provider c?a b?n (Cloudflare, Namecheap, etc.)

---

## ?? PRICING

### FREE Tier
- $5 credit/month (free)
- ~500 hours runtime
- ?? cho side project, MVP

### Paid Plans
- Developer: $20/month
- Team: $50/month
- Unlimited executions

---

## ?? TROUBLESHOOTING

### L?i: Build failed
?? Ki?m tra logs trong "Deploy Logs"
?? Verify .NET 10 SDK ???c detect

### L?i: 500 Internal Server Error
?? Ki?m tra Environment Variables ?ã ?úng ch?a
?? Xem logs ?? bi?t l?i c? th?

### L?i: Cannot connect to database
?? Verify Connection String
?? Check Supabase IP whitelist

### App sleep sau 1 th?i gian không dùng
?? FREE tier có sleep mode
?? Upgrade plan ?? keep alive 24/7

---

## ? CHECKLIST SAU KHI DEPLOY

- [ ] ? API có URL public
- [ ] ? Health check: `/health` returns 200
- [ ] ? Endpoints ho?t ??ng: `/api/classrooms`, `/api/students`
- [ ] ? Database connection thành công
- [ ] ? Auto-deploy t? GitHub ho?t ??ng
- [ ] ? Logs có th? xem ???c
- [ ] ? Domain ?ã ???c setup (optional)

---

## ?? NEXT STEPS

### 1. Setup Frontend
Dùng URL Railway trong frontend app

### 2. Add Authentication
Implement JWT ho?c OAuth

### 3. Monitoring
Setup monitoring v?i Railway metrics

### 4. Backup
Setup database backup routine

---

## ?? SUPPORT

- Railway Docs: https://docs.railway.app
- Discord: https://discord.gg/railway
- GitHub Issues: https://github.com/railwayapp/railway-feedback

---

**? Chúc b?n deploy thành công!**
