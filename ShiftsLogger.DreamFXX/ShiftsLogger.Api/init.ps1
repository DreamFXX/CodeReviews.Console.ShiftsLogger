# Povýšení privilege pro PowerShell skript
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Warning "Spusťte tento skript jako správce!"
    break
}

# Kontrola instalace .NET SDK
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -ne 0) {
    Write-Error ".NET SDK není nainstalován. Prosím, nainstalujte .NET 9.0 SDK."
    exit 1
}

Write-Host "Nalezen .NET SDK verze: $dotnetVersion" -ForegroundColor Green

# Kontrola existence databáze LocalDB
$dbExists = $false
try {
    $localDbProcess = Get-Process "SqlLocalDB" -ErrorAction SilentlyContinue
    if ($localDbProcess) {
        $dbExists = $true
    }
} catch {
    # Nic nedělat
}

if (-not $dbExists) {
    Write-Host "Kontrola SQL Server LocalDB..." -ForegroundColor Yellow
    
    # Pokus o spuštění LocalDB
    try {
        & sqllocaldb start mssqllocaldb
        Write-Host "SQL Server LocalDB je připraven." -ForegroundColor Green
    } catch {
        Write-Warning "SQL Server LocalDB není nainstalován nebo nemůže být spuštěn."
        Write-Host "Prosím, nainstalujte SQL Server Express LocalDB nebo použijte Docker." -ForegroundColor Yellow
    }
}

# Obnovení balíčků
Write-Host "Obnova NuGet balíčků..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "Chyba při obnovování NuGet balíčků!"
    exit 1
}
Write-Host "NuGet balíčky byly úspěšně obnoveny." -ForegroundColor Green

# Migrace databáze
Write-Host "Migrace databáze..." -ForegroundColor Yellow
dotnet ef database update
if ($LASTEXITCODE -ne 0) {
    Write-Error "Chyba při migraci databáze!"
    exit 1
}
Write-Host "Databáze byla úspěšně aktualizována." -ForegroundColor Green

# Spuštění aplikace
Write-Host "Spouštění aplikace..." -ForegroundColor Yellow
Start-Process "https://localhost:5001/swagger"
dotnet run 