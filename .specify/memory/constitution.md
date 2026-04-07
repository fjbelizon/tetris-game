<!--
SYNC IMPACT REPORT
==================
Version change: (unversioned) → 1.0.0
Added sections: Core Principles (I–VII), Technical Constraints, Development Workflow, Governance
Removed sections: none (initial version)
Templates requiring updates:
  ✅ .specify/memory/constitution.md — this file
  ✅ specs/CONSTITUTION.md — updated with canonical content
  ✅ .specify/templates/plan-template.md — Constitution Check gates align with I–VII
  ✅ .specify/templates/tasks-template.md — test discipline reflects Principle VI
Follow-up TODOs: none
-->

# Tetris Game Constitution

## Core Principles

### I. Technology Stack
C# 14 and .NET 10 MUST be used exclusively. No third-party NuGet packages are
permitted beyond those included in the .NET SDK. The project MUST target `net10.0`
and declare `<LangVersion>14</LangVersion>` in all `.csproj` files.

### II. Clean Architecture
The codebase MUST maintain strict separation across three layers:

- **Domain**: game logic (piece definitions, movement, rotation, collision detection,
  line clearing, scoring). Zero presentation or I/O code.
- **Renderer**: console rendering engine. MAY depend on Domain; MUST NOT be
  referenced by Domain.
- **Input**: keyboard/controller handling. MAY depend on Domain; MUST NOT be
  referenced by Domain.

No Domain class may reference `System.Console`, rendering types, or real-time
timing APIs. Layer isolation MUST be enforced through separate projects and
project references.

### III. Code Quality
Every class and module MUST have a single, clearly stated responsibility.
All identifiers (types, methods, parameters, variables) MUST use descriptive
English names. Redundant comments — those that merely restate what the code
already expresses — are forbidden. Abbreviations that obscure intent are
forbidden.

### IV. No Global Mutable State
Game state MUST be passed explicitly through method parameters or constructors.
Static mutable fields and global singletons are forbidden throughout the entire
codebase. Immutable value types (structs, records) are preferred for representing
game state snapshots.

### V. Build for Extension
The design MUST allow adding new piece types, difficulty levels, or game modes
without modifying existing Domain classes. The Open/Closed Principle applies to
all Domain entities. Extension points MUST be expressed via interfaces or
abstract base types; deep inheritance hierarchies are forbidden. New piece
shapes, scoring strategies, and level progressions MUST be addable through new
implementations only.

### VI. Test Discipline (NON-NEGOTIABLE)
Domain logic — piece movement, rotation, collision detection, line clearing, and
scoring — MUST be covered by unit tests using `xunit` (via .NET SDK tooling).
Tests MUST run without any dependency on `System.Console` or real-time clocks.
A fake/stub clock interface MUST be provided for time-dependent domain behavior.
All domain invariants MUST have test coverage. No domain feature is considered
complete until its tests pass with `dotnet test`.

### VII. Console Application Target
The deliverable MUST be a .NET console application. Gameplay MUST be visualized
using ASCII/Unicode block characters rendered in a standard terminal via
`System.Console`. No GUI frameworks (WinForms, WPF, MAUI, Avalonia, etc.) are
permitted. The application MUST be runnable cross-platform (Windows, macOS,
Linux).

## Technical Constraints

- **Runtime**: .NET 10 (`net10.0`), C# 14 language version.
- **Dependencies**: .NET SDK only. No external NuGet packages.
- **Testing framework**: `xunit` via `dotnet test`. No third-party mocking
  libraries; use hand-written test doubles.
- **Platform target**: Cross-platform terminal (`System.Console`).
- **Output**: Single console executable; no installer or deployment artifact
  required beyond `dotnet run` or a self-contained publish.

## Development Workflow

- All new Domain behavior MUST have unit tests before a PR is merged.
- Layer boundaries are enforced via project references: the Domain project MUST
  have no reference to the Renderer or Input projects.
- Code review MUST verify: no `Console` calls in Domain classes, no static
  mutable fields, English identifiers throughout, no third-party NuGet packages.
- The build MUST succeed with `dotnet build` and all tests MUST pass with
  `dotnet test` before any PR is merged.
- Each PR MUST include a Constitution Check section confirming compliance with
  Principles I–VII.

## Governance

This constitution supersedes all other implicit practices for the project.
Any amendment requires:

1. Explicit justification: which principle is added, modified, or removed and why.
2. Version bump per the versioning policy below.
3. All dependent templates (plan-template.md, spec-template.md, tasks-template.md)
   updated to reflect the change.

**Versioning policy**:
- MAJOR — incompatible principle removals or redefinitions.
- MINOR — new principle or section added, or material expansion of existing guidance.
- PATCH — clarifications, wording fixes, non-semantic refinements.

**Version**: 1.0.0 | **Ratified**: 2026-04-07 | **Last Amended**: 2026-04-07
