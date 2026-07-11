# PointsPerGame

View league tables order by points per game. Includes all UK leagues (including WSL) and European top divisions.

Teams are sorted by points per game, then by the number of games played.
This means if two teams have the same points per game, then the team which has played the fewer games is sorted higher.
If both of those values are equal, teams are sorted by goal difference, and then by team name. 

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

## Test locally

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
- `PointsPerGame.UnitTests`: Parser and service tests.

## Deployment

https://github.com/stuartd/PointsPerGame/issues/30

## Internals

A Blazor server site
