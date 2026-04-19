# 01-update-nuget-packages: Upgrade NuGet packages for net10.0


Upgrade NuGet package references across projects to versions compatible with net10.0 as recommended by the assessment. Focus on packages flagged as "Upgrade Recommended" in assessment.md (Microsoft.* packages, System.* compatibility packages, etc.).

**Done when**:
- All targeted NuGet packages updated in project files or package config
- Solution builds successfully
- All unit tests pass
- Changes documented in tasks/01-update-nuget-packages/progress-details.md

---

## Findings (discovered)

- Central Package Management: Not detected (no Directory.Packages.props found). Will update project PackageReference entries directly.

- PointsPerGame.Core (net48) packages of interest:
  - Microsoft.* packages at 10.0.6 (DependencyInjection, Logging, Http, Options, Primitives, etc.)
  - System.* compatibility packages: System.Configuration.ConfigurationManager (10.0.6), System.Runtime.Caching (10.0.6), System.Security.Permissions (10.0.6)

- PointsPerGame.UI (legacy ASP.NET MVC) packages:
  - Microsoft.AspNet.Mvc (5.2.9), Microsoft.AspNet.Razor (3.2.9), WebGrease, Respond, and compatibility packages.
  - Also references Microsoft.Extensions.* packages (10.0.6) already present.

- PointsPerGame.UI.Blazor (net7.0 temporary target): references PointsPerGame.Core via ProjectReference. Will need to ensure package updates do not break compatibility; NU1702 warnings currently present due to net48 → net7.0 reference.

## Recommended next steps

1. Update package versions for Microsoft.* and System.* packages in PointsPerGame.Core and PointsPerGame.UI projects to the versions recommended by the assessment (e.g., 10.0.6 where applicable).
2. For PointsPerGame.UI (net48/legacy), proceed cautiously — updating some Microsoft.Extensions packages is acceptable but verify compatibility; prefer leaving legacy ASP.NET packages (System.Web, Microsoft.AspNet.*) unchanged unless assessment advises.
3. Run targeted builds per project after each update (use `dotnet build` for SDK-style projects; legacy UI may need msbuild or remain untouched).
4. Record changes in progress-details.md and run full solution build and unit tests before calling complete_task.

