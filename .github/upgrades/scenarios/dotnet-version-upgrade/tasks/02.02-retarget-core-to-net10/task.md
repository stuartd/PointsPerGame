# 02.02-retarget-core-to-net10: Retarget PointsPerGame.Core to net10.0

## Objective
Retarget PointsPerGame.Core to net10.0 so all projects can reference a single net10 build.

## Scope
- File: PointsPerGame/PointsPerGame.Core/PointsPerGame.Core.csproj
- Impact: projects referencing Core (PointsPerGame.UI.Blazor, tests)

## Steps
1. Research: run get_project_dependencies for PointsPerGame.Core and dependents to list package definitions and check for net48-only references.
2. Update project file: replace <TargetFrameworks>net48;netstandard2.0</TargetFrameworks> with <TargetFramework>net10.0</TargetFramework>.
3. Remove or convert the net48-only <ItemGroup Condition="'$(TargetFramework)' == 'net48'"> Reference entries. Replace where needed with NuGet PackageReferences that provide equivalent functionality (e.g., System.Configuration.ConfigurationManager is already a PackageReference).
4. Restore and build PointsPerGame.Core: dotnet build PointsPerGame.Core/PointsPerGame.Core.csproj.
5. Fix any compile errors: add package replacements, conditional code, or small API shims as required. Avoid adding #pragma or suppressing warnings; resolve them.
6. Build PointsPerGame.UI.Blazor and run smoke tests to ensure it still runs.
7. Run full solution build and unit tests.
8. Document all changes and validations in tasks/02-migrate-ui-to-blazor/02.02-retarget-core-to-net10/progress-details.md.
9. Call complete_task('02.02-retarget-core-to-net10') with filesModified and summary.

## Done when
- PointsPerGame.Core targets net10.0 and builds clean with no warnings
- All dependent projects (Blazor, tests) build and smoke tests pass
- Progress documented in progress-details.md
