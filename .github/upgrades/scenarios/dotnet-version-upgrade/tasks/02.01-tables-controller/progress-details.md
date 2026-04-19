## Progress: TablesController migration - initial work

- Action: Added a lightweight TablesService adapter in PointsPerGame.UI.Blazor to provide league links and registered it in Program.cs.
- Action: Added a Blazor page at Pages/Tables.razor to render the league list using the adapter.
- Action: Added a ProjectReference from PointsPerGame.UI.Blazor to PointsPerGame.Core so the Blazor project can use core types.
- Validation: Building PointsPerGame.UI.Blazor now to verify changes.

Next steps:
1. Build and fix any compile issues.
2. If build succeeds, implement additional components to render full tables based on GuardianScraper results.
3. Document further progress and run manual smoke test for the Tables page.

Actions taken:
- Implemented TablesService.GetLeagueTableAsync that delegates to PointsPerGame.Core.Web.GuardianScraper.GetResults and maps ITeamResults to UI DTOs.
- Added Pages/TableDetails.razor to render league tables at route `/tables/{leagueId}`.

Validation:
- Will run targeted build and unit tests next; if successful will update progress and call complete_task.
