# PointsPerGame Agent Guide

## Project overview

PointsPerGame is a .NET 10 Blazor Server application that retrieves football
league tables from The Guardian and ranks teams by points per game.

The solution is under `src/`:

- `PointsPerGame.Core` contains league definitions, Guardian scraping and
  parsing, domain models, and table sorting.
- `PointsPerGame.UI.Blazor` contains the Blazor UI and application wiring.
- `PointsPerGame.UnitTests` contains NUnit tests using Shouldly.

Keep domain and data-source behaviour in `PointsPerGame.Core`. UI-specific
formatting and interaction belong in `PointsPerGame.UI.Blazor`.

## Build and test

Run commands from the repository root:

```bash
dotnet restore ./src/PointsPerGame.slnx
dotnet build ./src/PointsPerGame.slnx --no-restore
dotnet test ./src/PointsPerGame.slnx --no-restore
```

Run the site locally with:

```bash
dotnet run --project ./src/PointsPerGame.UI.Blazor/PointsPerGame.UI.Blazor.csproj --launch-profile https
```

The tests in `When_Retrieving_Tables_From_Guardian_Website.cs` are marked
`[Explicit]`. They make live network requests, deliberately rate-limit link
checks, and are not part of routine validation. Run them only when a task
specifically requires live Guardian verification.

## Sorting invariants

`TeamResultsExtensions.SortTeams` defines the product's central behaviour.
Preserve these rules unless the task explicitly changes them:

1. Sort by points per game, highest first.
2. Among teams with the maximum possible points per game, sort by games played,
   highest first. This is the "points in the bag" rule.
3. For every other points-per-game tie, sort by games played, lowest first.
4. Resolve remaining ties by goal difference descending, points descending,
   then team name ascending.
5. A team that has played no games has zero points per game.

Maximum points per game must use the configured points awarded for a win; do
not hard-code that value inside the sorting extension. Add or update focused
tests in `TeamResults_Sorting_Tests.cs` whenever sorting behaviour changes.
If the user-visible rules change, update both `README.md` and the sorting text
on `Pages/Tables.razor`.

## League and scraper changes

- A concrete `TableSelection` should have a Guardian URL mapping and should be
  included in the appropriate league groups.
- Aggregate selections such as `AllLeagues` do not have Guardian table URLs.
- Keep parsing isolated in `GuardianTableParser` and HTTP/cache behaviour in
  `GuardianScraper`.
- Prefer parser tests with representative inline HTML over live network tests.
- Preserve helpful parsing exceptions when Guardian markup is missing or
  malformed.

## Code and repository conventions

- Follow the style of the surrounding file. The codebase uses nullable
  reference types, implicit usings, file-scoped namespaces, collection
  expressions, and modern C# syntax.
- Use NUnit and Shouldly for tests. Test names should describe the observable
  behaviour.
- Do not commit generated `bin/`, `obj/`, IDE, or user-specific files.
- Preserve `.gitattributes`: C#, project, Razor, JSON, and CSS files use CRLF;
  Markdown files use LF.
- Keep changes focused and do not overwrite unrelated working-tree edits.

Before handing off a code change, run the relevant focused tests followed by
the full solution tests and build. Also run `git diff --check`.
