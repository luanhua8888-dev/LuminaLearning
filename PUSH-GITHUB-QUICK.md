# ?? H??NG D?N NHANH - ??Y CODE LÊN GITHUB

## ? 3 B??C ??N GI?N

### 1?? T?o Repository trên GitHub
```
https://github.com/new
```
- Tên: `lumina-learning`
- ? KHÔNG tích các checkbox (README, .gitignore, license)
- Click "Create repository"
- **Copy URL:** `https://github.com/username/lumina-learning.git`

### 2?? Ch?y Script PowerShell
```powershell
# M? PowerShell trong th? m?c project
cd "C:\Users\Software\source\repos\Lumina Learning\LuL\"

# Ch?y script
.\push-to-github.ps1
```

### 3?? Nh?p URL khi ???c h?i
```
Enter your GitHub repository URL: https://github.com/username/lumina-learning.git
```

## ? XONG!

---

## ?? B?O M?T ?Ã ???C X? LÝ

? **appsettings.json** - Ch? ch?a template, không có thông tin th?t
? **appsettings.Local.json** - Ch?a config th?t, KHÔNG ???c push (?ã có trong .gitignore)
? **.env** - Trong .gitignore
? **bin/, obj/, .vs/** - Trong .gitignore

---

## ?? SAU KHI PUSH

### C?u hình GitHub Secrets (cho CI/CD)
1. Vào: `https://github.com/username/lumina-learning/settings/secrets/actions`
2. Add secrets:
   - `SUPABASE_URL`
   - `SUPABASE_KEY`
   - `CONNECTION_STRING`

### Verify
```bash
# Clone v? máy khác ?? test
git clone https://github.com/username/lumina-learning.git
cd lumina-learning

# T?o file config local
copy appsettings.json appsettings.Local.json
# S?a appsettings.Local.json v?i thông tin th?t

# Ch?y
dotnet restore
dotnet run
```

---

## ?? N?U G?P L?I

### "Git not found"
?? Cài Git: https://git-scm.com/download/win

### "Authentication failed"
?? Dùng Personal Access Token:
1. GitHub ? Settings ? Developer settings ? Personal access tokens ? Generate new token
2. Ch?n: `repo`, `workflow`
3. Copy token và dùng làm password khi push

### "Remote already exists"
```powershell
git remote remove origin
git remote add origin YOUR_REPO_URL
git push -u origin main
```

---

## ?? TÀI LI?U CHI TI?T

- `GITHUB-SETUP.md` - H??ng d?n ??y ??
- `DEPLOYMENT.md` - H??ng d?n deploy
- `README.md` - T?ng quan project

---

**Ready to push? Ch?y script ngay! ??**
