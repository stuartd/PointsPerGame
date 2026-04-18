# 01.01-update-core: Update packages in PointsPerGame.Core

## Objective
Update NuGet packages in PointsPerGame.Core to versions compatible with net10.0 as recommended by assessment.md.

## Steps
1. Inspect assessment.md for recommended versions for packages referenced by PointsPerGame.Core.
2. Update PackageReference versions in PointsPerGame.Core.csproj.
3. Run `dotnet restore` and `dotnet build` for the project and for the solution.
4. Run unit tests to validate.

**Done when**: project builds and tests pass after changes.
