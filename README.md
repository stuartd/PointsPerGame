# PointsPerGame

A small Blazor Server site for viewing football tables sorted by points per game.

## Run locally

```powershell
cd .\PointsPerGame
dotnet run --project .\PointsPerGame.UI.Blazor\PointsPerGame.UI.Blazor.csproj --launch-profile https
```

Then open `https://localhost:7097`.

## Test

```powershell
dotnet test .\PointsPerGame.Tests\PointsPerGame.Tests.csproj
dotnet test .\PointsPerGame.UnitTests\PointsPerGame.UnitTests.csproj
```

## Project layout

- `PointsPerGame.Core`: league names, Guardian scraping/parsing, and sorting.
- `PointsPerGame.UI.Blazor`: the website.
- `PointsPerGame.Tests`: parser and service tests.
- `PointsPerGame.UnitTests`: older explicit Guardian smoke test and duplicated sorting coverage.
