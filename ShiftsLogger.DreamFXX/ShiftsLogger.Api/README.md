# ShiftsLogger API

Aplikace pro záznam a sledování směn zaměstnanců.

## Požadavky

- .NET 9.0
- Entity Framework Core
- SQL Server (lokalní nebo Docker)

## Funkce

- Záznam začátku a konce směn
- Přehled všech směn
- Statistiky směn
- Správa aktivních směn
- Ověření, že v jednom okamžiku může být aktivní pouze jedna směna

## Instalace a spuštění

### Pomocí Docker

Nejjednodušší způsob, jak spustit aplikaci, je použít Docker Compose:

```bash
docker-compose up -d
```

Aplikace bude dostupná na adrese http://localhost:5000

### Lokální instalace

1. Naklonujte repozitář
2. Upravte connection string v `appsettings.json` podle vaší instance SQL Serveru
3. Spusťte migraci databáze:

```bash
dotnet ef database update
```

4. Spusťte aplikaci:

```bash
dotnet run
```

## API Endpointy

### Získání všech směn

```
GET /api/Shifts
```

### Získání aktivní směny

```
GET /api/Shifts/active
```

### Získání statistik

```
GET /api/Shifts/stats
```

### Získání konkrétní směny

```
GET /api/Shifts/{id}
```

### Zahájení nové směny

```
POST /api/Shifts
Content-Type: application/json

{
  "employeeName": "Jméno Příjmení"
}
```

### Ukončení směny

```
PUT /api/Shifts/{id}
```

### Smazání směny

```
DELETE /api/Shifts/{id}
``` 