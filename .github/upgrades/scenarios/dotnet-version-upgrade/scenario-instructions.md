# .NET Version Upgrade: dotnet-version-upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0

## Source Control
- **Source Branch**: main
- **Working Branch**: upgrade-dotnet-10
- **Commit Strategy**: After Each Task

## Notes
- This file records the confirmed pre-initialization parameters for the dotnet-version-upgrade scenario.
- The agent will create assessment.md and plan.md under `.github/upgrades/scenarios/dotnet-version-upgrade/`.

## Decisions
- PR created by user for branch `upgrade-dotnet-10`. PR URL: https://github.com/stuartd/PointsPerGame/compare/upgrade-dotnet-10?expand=1 (2026-04-19)
- UI scaffold target: Scaffold PointsPerGame.UI.Blazor targeting net7.0 now (host SDK unavailable for net10.0). Will retarget to net10.0 when .NET 10 SDK is installed. (User choice: 1)
