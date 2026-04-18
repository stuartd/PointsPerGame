# .NET Version Upgrade Plan

## Overview

**Target**: net10.0
**Scope**: 3 projects (PointsPerGame.Core, PointsPerGame.UI, PointsPerGame.UnitTests)

## Tasks

### 01-update-nuget-packages: Upgrade NuGet packages for net10.0

Upgrade NuGet package references across projects to versions compatible with net10.0 as recommended by the assessment. Focus on packages flagged as "Upgrade Recommended" in assessment.md (Microsoft.* packages, System.* compatibility packages, etc.).

**Done when**:
- All targeted NuGet packages updated in project files or package config
- Solution builds successfully
- All unit tests pass
- Changes documented in tasks/01-update-nuget-packages/progress-details.md

---

### 02-migrate-ui-to-blazor: Migrate PointsPerGame.UI to Blazor Server

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

---
