# 02-migrate-ui-to-blazor: Migrate PointsPerGame.UI to Blazor Server


Migrate the legacy ASP.NET UI project to an ASP.NET Core Blazor Server application targeting net10.0. This task focuses on a feature-preserving migration (no UI redesign), porting controllers/views to Blazor components where feasible, and preserving server-side behavior.

**Steps (high level)**:
1. Create new Blazor Server project in solution (PointsPerGame.UI.Blazor) targeting net10.0.
2. Move static assets, views, and relevant server-side logic into the new project as components or services.
3. Replace legacy System.Web-dependent code with ASP.NET Core equivalents or abstractions.
4. Wire up dependency injection and configuration using Microsoft.Extensions.*
5. Build and run the Blazor Server app; validate functionality and behavior matches legacy UI.
6. Update solution and CI to include the new project; keep legacy project until smoke validation completes.

**Done when**:
- New Blazor Server project added and builds targeting net10.0
- Critical UI flows exerciseable and behavior validated manually or with automated tests
- No regressions in unit tests focusing on core logic
- Migration notes documented in tasks/02-migrate-ui-to-blazor/progress-details.md

## Research findings

- Legacy System.Web usages found in PointsPerGame.UI:
  - Global.asax.cs (application start, routing, bundles)
  - Controllers/TablesController.cs and other MVC controllers using System.Web.Mvc
  - App_Start: RouteConfig.cs, FilterConfig.cs, BundleConfig.cs
  - Views/Web.config and root Web.config reference System.Web.Mvc, Razor, WebPages, System.Web.Optimization
  - Project references to System.Web, System.Web.Abstractions, System.Web.Routing in the csproj
- Implications:
  - MVC controllers and Razor views must be ported to Blazor components or server-side APIs.
  - Bundling/optimization and Web.config settings need replacement (use WebOptimizer or static files + appsettings.json).
  - Global.asax logic (startup) must be migrated to Program.cs/Startup with middleware and DI.
  - Some libraries (System.Web.*) have no direct equivalents; refactor to ASP.NET Core abstractions or keep logic in Core project services.

**Recommendation**: Use side-by-side migration with a new PointsPerGame.UI.Blazor Blazor Server project targeting net10.0. Port features incrementally: expose server logic via services in PointsPerGame.Core, reimplement views as Blazor components, and keep original project until validation completes.

---


