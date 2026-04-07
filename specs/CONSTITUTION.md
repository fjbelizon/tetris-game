# Tetris Game Constitution

> Canonical source: `.specify/memory/constitution.md`

## Core Principles

### I. Technology Stack
C# 14 and .NET 10 exclusively. No third-party NuGet packages beyond the .NET SDK.
All projects MUST target `net10.0` with `<LangVersion>14</LangVersion>`.

### II. Clean Architecture
Strict layer separation: **Domain** (game logic) | **Renderer** (console output) |
**Input** (keyboard control). Domain has zero presentation or I/O code. Layer
isolation enforced via separate projects and project references.

### III. Code Quality
Single responsibility per class/module. Descriptive English identifiers
throughout. Redundant comments forbidden.

### IV. No Global Mutable State
Game state passed explicitly — no static mutable fields, no global singletons.
Immutable value types preferred for state snapshots.

### V. Build for Extension
New piece types, difficulty levels, or game modes MUST be addable via new
implementations only. Open/Closed Principle on all Domain entities. Extension
points expressed as interfaces or abstract base types.

### VI. Test Discipline (NON-NEGOTIABLE)
All Domain logic — movement, rotation, collision, line clearing, scoring —
MUST be covered by `xunit` unit tests without `System.Console` or real-time
clock dependencies. A stub clock interface MUST be provided. Tests MUST pass
with `dotnet test` before any PR merges.

### VII. Console Application Target
Deliverable is a .NET console app. Visualization via ASCII/Unicode block
characters in a standard terminal using `System.Console`. No GUI frameworks.
Cross-platform (Windows, macOS, Linux).

## Tech Stack

- **Language**: C# 14
- **Runtime**: .NET 10 (`net10.0`)
- **Dependencies**: .NET SDK only (no external NuGet packages)
- **Testing**: `xunit` via `dotnet test`
- **Platform**: Cross-platform terminal

## Definition of Done

- [ ] Domain behavior implemented with unit tests passing (`dotnet test`)
- [ ] Layer separation maintained: no `Console` in Domain, no circular project refs
- [ ] No static mutable fields introduced
- [ ] No third-party NuGet packages added
- [ ] All identifiers in English, single responsibility per class
- [ ] PR references spec issue and includes Constitution Check (Principles I–VII)

**Version**: 1.0.0 | **Ratified**: 2026-04-07 | **Last Amended**: 2026-04-07
