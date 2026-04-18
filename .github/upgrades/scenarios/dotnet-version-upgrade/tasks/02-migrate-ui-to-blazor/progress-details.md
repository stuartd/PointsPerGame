## Progress: Scaffold Blazor Server project

- Action: Scaffolded a new Blazor Server project `PointsPerGame.UI.Blazor` (net10.0) and added it to the solution.
- Why: Enables side-by-side migration of the legacy ASP.NET UI into Blazor Server for incremental porting.
- Validation: Project created and added to PointsPerGame.sln; build attempted.

Next steps:
1. Verify Blazor project builds and runs locally.
2. Add DI and minimal Program.cs mapping for legacy services.
3. Inventory controllers/views for prioritised migration units.
