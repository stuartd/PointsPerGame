## Progress: Scaffold Blazor Server project

- Action: Scaffolded a new Blazor Server project `PointsPerGame.UI.Blazor` targeting net7.0 (temporary) and added it to the solution at `PointsPerGame/PointsPerGame.sln`.
- Why: Host does not currently have the .NET 10 SDK; scaffolding to net7.0 allows side-by-side migration work to proceed locally. The project will be retargeted to net10.0 when the .NET 10 SDK is available and verified.
- Validation: Project created, added to solution, and built successfully (dotnet build succeeded). Build emitted warnings about net7.0 being out of support — will retarget later.

Next steps:
1. Verify Blazor project runs locally (`dotnet run`) and add DI and minimal Program.cs mapping for legacy services.
2. Inventory controllers/views for prioritised migration units.
3. Start migration unit for the simplest controller (TablesController).

Recent actions:

- Implemented TablesService adapter (PointsPerGame.UI.Blazor/Services/TablesService.cs) that wraps GuardianScraper and returns UI DTOs.
- Added Pages/Tables.razor to list leagues and enhanced with sorting documentation and GitHub links.
- Added Pages/TableDetails.razor to render full league tables and integrated Missing component handling.
- Added Pages/Missing.razor component for friendly missing-page messaging.
- Built PointsPerGame.UI.Blazor successfully (initially net7.0 target); retargeted to net10.0 and rebuilt — build succeeded without the earlier TFM/NU1702 warnings.

Validation:
- Targeted and solution builds succeeded; unit tests build succeeded. Remaining warnings are documented and tracked in the task notes.

Next actions (Automatic):
- Run a local smoke run (`dotnet run`) for PointsPerGame.UI.Blazor and exercise /tables and /tables/{leagueId} — SUCCESS: /tables and /tables/1 returned rendered pages with data from GuardianScraper.
- Continue migrating remaining views and controllers in prioritized order.
