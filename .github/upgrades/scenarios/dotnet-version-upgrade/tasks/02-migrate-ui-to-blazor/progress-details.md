## Progress: Scaffold Blazor Server project

- Action: Scaffolded a new Blazor Server project `PointsPerGame.UI.Blazor` targeting net7.0 (temporary) and added it to the solution at `PointsPerGame/PointsPerGame.sln`.
- Why: Host does not currently have the .NET 10 SDK; scaffolding to net7.0 allows side-by-side migration work to proceed locally. The project will be retargeted to net10.0 when the .NET 10 SDK is available and verified.
- Validation: Project created, added to solution, and built successfully (dotnet build succeeded). Build emitted warnings about net7.0 being out of support — will retarget later.

Next steps:
1. Verify Blazor project runs locally (`dotnet run`) and add DI and minimal Program.cs mapping for legacy services.
2. Inventory controllers/views for prioritised migration units.
3. Start migration unit for the simplest controller (TablesController).
