# Script ?? ??y code lên GitHub l?n ??u
# Ch?y script này trong PowerShell

param(
    [Parameter(Mandatory=$false)]
    [string]$RepoUrl = ""
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Lumina Learning - Push to GitHub" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Check if git is installed
$gitInstalled = Get-Command git -ErrorAction SilentlyContinue
if (-not $gitInstalled) {
    Write-Host "? Git is not installed!" -ForegroundColor Red
    Write-Host "Download from: https://git-scm.com/download/win" -ForegroundColor Yellow
    exit 1
}

Write-Host "? Git is installed" -ForegroundColor Green

# Check if .git folder exists
if (Test-Path ".git") {
    Write-Host "? Git repository already initialized" -ForegroundColor Yellow
    $continue = Read-Host "Continue anyway? (y/n)"
    if ($continue -ne "y") {
        exit 0
    }
} else {
    Write-Host "Initializing Git repository..." -ForegroundColor Yellow
    git init
    Write-Host "? Git initialized" -ForegroundColor Green
}

# Check if appsettings.json is protected
if (-not (Test-Path ".gitignore")) {
    Write-Host "? .gitignore not found!" -ForegroundColor Red
    Write-Host "Creating .gitignore..." -ForegroundColor Yellow
    # .gitignore should already be created by previous step
}

Write-Host ""
Write-Host "Checking for sensitive files..." -ForegroundColor Yellow

# Warn about sensitive files
if (Test-Path "appsettings.json") {
    Write-Host "? WARNING: appsettings.json contains sensitive data!" -ForegroundColor Yellow
    Write-Host "  Make sure it's in .gitignore!" -ForegroundColor Yellow
}

if (Test-Path ".env") {
    Write-Host "? WARNING: .env contains sensitive data!" -ForegroundColor Yellow
    Write-Host "  Make sure it's in .gitignore!" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Files to be committed:" -ForegroundColor Cyan
git status --short

Write-Host ""
$confirm = Read-Host "Do you want to continue? (y/n)"
if ($confirm -ne "y") {
    Write-Host "Cancelled." -ForegroundColor Yellow
    exit 0
}

# Add all files
Write-Host ""
Write-Host "Adding files to Git..." -ForegroundColor Yellow
git add .

# Show what will be committed
Write-Host ""
Write-Host "Files staged for commit:" -ForegroundColor Cyan
git status --short

# Commit
Write-Host ""
$commitMessage = Read-Host "Enter commit message (default: 'Initial commit')"
if ([string]::IsNullOrWhiteSpace($commitMessage)) {
    $commitMessage = "Initial commit - Lumina Learning API"
}

git commit -m "$commitMessage"
Write-Host "? Files committed" -ForegroundColor Green

# Set main branch
Write-Host ""
Write-Host "Setting main branch..." -ForegroundColor Yellow
git branch -M main
Write-Host "? Branch set to 'main'" -ForegroundColor Green

# Add remote
Write-Host ""
if ([string]::IsNullOrWhiteSpace($RepoUrl)) {
    Write-Host "Please create a new repository on GitHub:" -ForegroundColor Cyan
    Write-Host "1. Go to https://github.com/new" -ForegroundColor White
    Write-Host "2. Create repository named 'lumina-learning'" -ForegroundColor White
    Write-Host "3. DO NOT initialize with README, .gitignore, or license" -ForegroundColor Yellow
    Write-Host ""
    $RepoUrl = Read-Host "Enter your GitHub repository URL (e.g., https://github.com/username/lumina-learning.git)"
}

if ([string]::IsNullOrWhiteSpace($RepoUrl)) {
    Write-Host "? Repository URL is required!" -ForegroundColor Red
    exit 1
}

# Check if remote already exists
$remoteExists = git remote | Where-Object { $_ -eq "origin" }
if ($remoteExists) {
    Write-Host "Updating remote origin..." -ForegroundColor Yellow
    git remote set-url origin $RepoUrl
} else {
    Write-Host "Adding remote origin..." -ForegroundColor Yellow
    git remote add origin $RepoUrl
}

Write-Host "? Remote added: $RepoUrl" -ForegroundColor Green

# Push
Write-Host ""
Write-Host "Pushing to GitHub..." -ForegroundColor Yellow
Write-Host "You may need to login to GitHub..." -ForegroundColor Cyan

try {
    git push -u origin main
    Write-Host ""
    Write-Host "============================================" -ForegroundColor Green
    Write-Host "  ? Successfully pushed to GitHub!" -ForegroundColor Green
    Write-Host "============================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Repository: $RepoUrl" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Yellow
    Write-Host "1. Visit your repository on GitHub" -ForegroundColor White
    Write-Host "2. Add repository secrets for CI/CD" -ForegroundColor White
    Write-Host "3. Configure deployment settings" -ForegroundColor White
} catch {
    Write-Host ""
    Write-Host "? Failed to push to GitHub" -ForegroundColor Red
    Write-Host "Error: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Common issues:" -ForegroundColor Yellow
    Write-Host "1. Make sure you have access to the repository" -ForegroundColor White
    Write-Host "2. Check if you're logged in to GitHub" -ForegroundColor White
    Write-Host "3. Verify the repository URL is correct" -ForegroundColor White
    Write-Host ""
    Write-Host "Try manual push:" -ForegroundColor Cyan
    Write-Host "  git push -u origin main" -ForegroundColor White
}
