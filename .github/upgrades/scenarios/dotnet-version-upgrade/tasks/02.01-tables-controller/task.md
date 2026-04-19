# 02.01-tables-controller: Migrate TablesController to Blazor Server

## Objective
Migrate the TablesController (PointsPerGame.UI.Controllers.TablesController) to the new Blazor Server project (side-by-side).

## Scope
- Controller: /PointsPerGame/PointsPerGame.UI/Controllers/TablesController.cs
- Views: Views/Tables/* (port List, Missing, Index as Blazor components or partials)
- Dependencies: PointsPerGame.Core (GuardianScraper, League, ITeamResults, MissingTable, GuardianLeagueMappings)
- Register any required services in PointsPerGame.UI.Blazor (use DI for core services)

## Steps
1. Re-run dependency discovery (get_code_dependencies was run) and list all dependent files.
2. Create a lightweight service adapter in PointsPerGame.UI.Blazor that wraps GuardianScraper calls and returns POCOs from PointsPerGame.Core.
3. Add a ProjectReference to PointsPerGame.Core in PointsPerGame.UI.Blazor if not present.
4. Port the simplest view (List) to a Blazor component that calls the adapter service and renders the table.
5. Map HttpNotFoundResult to a Missing component or show a friendly message.
6. Build and fix compile/runtime issues; run unit tests focusing on core logic.
7. Document changes in tasks/02-migrate-ui-to-blazor/02.01-tables-controller/progress-details.md and update parent task progress.

## Done when
- New Blazor component(s) for Tables page compile and render with data from PointsPerGame.Core.
- PointsPerGame.UI.Blazor references PointsPerGame.Core and builds without errors or migration warnings.
- Progress notes added: .github/upgrades/scenarios/dotnet-version-upgrade/tasks/02-migrate-ui-to-blazor/02.01-tables-controller/progress-details.md

### Findings (discovered)
- GuardianScraper is implemented at: /Users/ai/code/PointsPerGame/PointsPerGame.Core/Web/GuardianScraper.cs (provides GetResults/GetMultipleLeagueResults returning List<ITeamResults>). 
- Adapter present: PointsPerGame.UI.Blazor/Services/TablesService.cs (returns league description links).
- Blazor UI stub: PointsPerGame.UI.Blazor/Pages/Tables.razor exists and uses the adapter to render league links.
- ProjectReference: PointsPerGame.UI.Blazor references PointsPerGame.Core (ProjectReference added).

### Next steps
1. Build PointsPerGame.UI.Blazor and fix any compile/runtime issues (targeted project build).
2. Implement table rendering: call GuardianScraper.GetResults(league) from the adapter (asynchronous) and map ITeamResults to UI POCOs.
3. Add a "Missing" component or friendly message for HttpNotFoundResult cases.
4. Validate by running targeted build and relevant unit tests in PointsPerGame.Core.
5. Update this task's progress-details.md with results and call complete_task when all gates are satisfied.
