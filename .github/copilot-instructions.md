# Copilot instructions for PointsPerGame

This file helps Copilot-based agents understand and operate on this repository.

## Workspace layout & important paths
- Solution file (entrypoint): PointsPerGame/PointsPerGame/PointsPerGame.sln (note the nested `PointsPerGame/PointsPerGame` path).
- Projects:
  - PointsPerGame.Core — .NET Framework 4.8 class library (models, scraping, utilities)
  - PointsPerGame.UI — ASP.NET MVC (.NET Framework 4.8) web app (Views, Controllers)
  - PointsPerGame.UnitTests — NUnit tests (net48)
- Packages: legacy NuGet `packages/` + `packages.config` and optional `.nuget\NuGet.targets` may be referenced by projects.

## Build, test, and (no) lint commands
These projects target .NET Framework (packages.config style). Preferred environment: Windows with Visual Studio / MSBuild and NuGet.

Common commands (run from the repository root):
- Restore packages: `nuget restore PointsPerGame/PointsPerGame/PointsPerGame.sln`
- Build (MSBuild): `msbuild /p:Configuration=Debug PointsPerGame/PointsPerGame/PointsPerGame.sln`
  - Alternative: open the solution in Visual Studio and build.

Running tests (UnitTests uses NUnit):
- Build first (see above). Test assembly path: `PointsPerGame/PointsPerGame/PointsPerGame.UnitTests/bin/Debug/PointsPerGame.UnitTests.dll`.
- Run with NUnit console (if available):
  - `nunit3-console path\to\PointsPerGame.UnitTests.dll --test=Namespace.Class.Method` (replace with fully qualified test name).
- Alternatively use Visual Studio Test Explorer or vstest.console (Visual Studio):
  - `vstest.console.exe path\to\PointsPerGame.UnitTests.dll /TestCaseFilter:"FullyQualifiedName=Namespace.Class.Method"`
- Single-test example (NUnit): `nunit3-console ... --test=PointsPerGame.UnitTests.CoreClassesTests.TeamResults_Sorting_Tests.SomeTestMethod`

Notes:
- `dotnet test` is not reliable for these older (non-SDK) .NET Framework projects; prefer Visual Studio / MSBuild + NUnit/vstest.
- No linting/formatting tool configured in the repo (no StyleCop/EditorConfig enforcement found).

## High-level architecture (big picture)
- Data ingestion/scraping lives in PointsPerGame.Core (GuardianScraper, Scraper utilities, mapping to domain models). Core exposes model types and scraping helpers.
- The UI (PointsPerGame.UI) is an ASP.NET MVC front-end that references Core and renders tables/views (Controllers under Controllers/, Views under Views/). Publish profiles present under Properties/PublishProfiles for deployment.
- Unit tests exercise Core behaviour (PointsPerGame.UnitTests). Tests use NUnit + FluentAssertions and expect packages restored under `../packages` paths.
- Projects are classic (non-SDK) MSBuild style and rely on relative `HintPath` references into the solution-level `packages` folder — keep solution-relative paths when running scripts or edits.

## Key repository conventions and gotchas
- Solution is nested: many build/test scripts and csproj HintPaths expect to be executed relative to the solution file at `PointsPerGame/PointsPerGame/PointsPerGame.sln`. Use that as the working directory for build/test automation unless explicitly rewriting paths.
- Legacy NuGet (packages.config + `packages/`) pattern: package restore must populate `packages/` before building; some projects import `$(SolutionDir)\.nuget\NuGet.targets` if present.
- Tests use NUnit (3.x) and FluentAssertions. Test file naming uses descriptive/BDD-like names (underscored test file names). Look for test classes inside PointsPerGame.UnitTests/CoreClassesTests/ and similar.
- UI is an ASP.NET MVC 5 app (Web.config, Views, App_Start) — running locally normally occurs under Visual Studio / IIS Express.

## Files checked for AI assistant configs
- Searched for common AI assistant files (CLAUDE.md, .cursorrules, AGENTS.md, CONVENTIONS.md, AIDER_CONVENTIONS.md, .windsurfrules, .clinerules, .github/**). None contained repository-specific guidance to merge.

## Guidance for Copilot sessions
- Prefer editing the solution while keeping solution-relative HintPaths intact. When suggesting automated scripts, target msbuild/nuget or Visual Studio workflows, not `dotnet` SDK commands for test/run.
- For changes touching packages or references, prefer adding instructions to update `packages.config` and run `nuget restore`.
- When proposing test runs or CI steps, include explicit package-restore and msbuild steps and reference the nested solution path.

---
If you want this file adjusted (more CI examples, exact single-test command examples for a specific test, or addition of linting/refactor rules), say which area to expand.
