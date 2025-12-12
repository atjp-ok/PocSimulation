# Poc Tank/Vask/Appservice

Projektet er en del af bacherloropgaven og illustrerer et system med flere services
herunder:
## Services

- **TankService**: Håndterer tankningsforløb.
- **VaskService**: Håndterer vaskforløb.
- **AppService**: Håndterer parkeringsforløb.
- **NewBFF (Backend For Frontend)**: Navigerer i kald til domænerne og samler data fra services til frontend.
- **PSPService**: Simulerer betalinger.
- **HammaqService**: Håndterer standere ude ved stationerne.
- **UserProfile**: Håndterer hentning af bruger information.

## Teknologier

- .NET Core / ASP.NET Core
- Entity Framework Core (InMemory DB)
- REST API (JSON)
- Visual Studio Code

# Service-URLs og lokal kørsel

Alle services kører lokalt via `localhost` og porte, som er defineret i de enkelte serviceprojekters `appsettings.Development.json`.  
Eksempel er fra VaskService:

```json
"ServiceUrls": {
  "PspService": "https://localhost:5004",
  "UserProfileService": "https://localhost:5002",
  "HammaqService": "https://localhost:5005"
}
```
## API-dokumentation og afprøvelse

Alle services har Swagger UI tilgængelig på `https://localhost:[port]/swagger`.

## Sådan kommer du igang med at afprøve 

1. Installer .NET 8 SDK hvis du ikke allerede har det.
2. Installer Visual Studio Code.
3. Følg guiden nedenfor for at starte services.

## Sådan starter du en service manuelt

Du kan starte en enkelt service ved at åbne en terminal, navigere til service-mappen og køre:

```sh
cd Services/VaskService
dotnet run
```
Gør tilsvarende for TankService, AppService osv.
> **OBS:**  
> For at TankService, VaskService og AppService kan fungere korrekt, skal følgende services også være startet:
> - HammaqService
> - PspService
> - UserProfileService

Disse services skal være kørende, da de andre services afhænger af dem for at kunne udføre betalinger, hente brugerdata og håndtere standere.


## Sådan starter du alle services på en gang

Projektet indeholder en `.vscode/tasks.json`, så du kan starte alle services på en gang fra Visual Studio Code.

### Start alle services

1. Åbn projektmappen i Visual Studio Code.
2. Gå til menuen **Terminal > Run Task...**
3. Vælg **"Start All Services"**.
4. Alle services (TankService, VaskService, AppService, PSPService, HammaqService, UserProfile, NewBFF) starter nu automatisk i hver sin terminal.

**Tip:** Du kan også søge efter "Run Task" i Command Palette (`Ctrl+Shift+P`) og vælge **> Tasks:Run Task vælge den og Run all services..** .


