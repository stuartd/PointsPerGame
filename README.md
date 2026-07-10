# PointsPerGame

Blazor Server site for viewing football tables sorted by points per game.



## Run locally

```powershell
cd .\src
dotnet run --project .\PointsPerGame.UI.Blazor\PointsPerGame.UI.Blazor.csproj --launch-profile https
```

```bash
cd ./src
dotnet run --project ./PointsPerGame.UI.Blazor/PointsPerGame.UI.Blazor.csproj --launch-profile https
```
Then open `https://localhost:7097`.

## Test

```powershell
dotnet test .\PointsPerGame.Tests\PointsPerGame.Tests.csproj
dotnet test .\PointsPerGame.UnitTests\PointsPerGame.UnitTests.csproj
```

```bash
dotnet test ./PointsPerGame.Tests/PointsPerGame.Tests.csproj
dotnet test ./PointsPerGame.UnitTests/PointsPerGame.UnitTests.csproj
```

## Project layout

- `PointsPerGame.Core`: League names, Guardian scraping/parsing, and sorting.
- `PointsPerGame.UI.Blazor`: The website.
- `PointsPerGame.Tests`: Parser and service tests.
- `PointsPerGame.UnitTests`: older explicit Guardian smoke test and duplicated sorting coverage.
