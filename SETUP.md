# Lumina Learning - Setup và Deploy Guide

## ?? Setup Ban ??u

### 1. Clone Repository
```bash
git clone https://github.com/YOUR_USERNAME/lumina-learning.git
cd lumina-learning
```

### 2. Cài ??t Dependencies
```bash
dotnet restore
```

### 3. C?u hình Database
- T?o account trên [Supabase](https://supabase.com)
- T?o project m?i
- Ch?y SQL script trong `Database/create_classrooms_table.sql`

### 4. C?u hình appsettings.json
```json
{
  "Supabase": {
    "Url": "YOUR_SUPABASE_URL",
    "Key": "YOUR_SUPABASE_KEY"
  },
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  }
}
```

?? **KHÔNG commit file này lên Git!**

### 5. Ch?y ?ng d?ng
```bash
dotnet run
```

API s? ch?y t?i: http://localhost:5000

---

## ?? Deploy

Xem chi ti?t trong:
- `QUICK-DEPLOY.md` - H??ng d?n nhanh
- `DEPLOYMENT.md` - H??ng d?n ??y ??

---

## ?? API Documentation

Khi ch?y local, truy c?p:
- Swagger UI: http://localhost:5000/scalar/v1

---

## ?? Contributing

1. Fork repository
2. T?o branch m?i: `git checkout -b feature/your-feature`
3. Commit changes: `git commit -am 'Add new feature'`
4. Push to branch: `git push origin feature/your-feature`
5. T?o Pull Request

---

## ?? License

MIT License
