# Implementation Plan: Tetris Console Game

**Branch**: `001-tetris-console-game` | **Date**: 2026-04-07 | **Spec**: `specs/001-tetris-console-game/spec.md`
**Input**: Feature specification from `specs/001-tetris-console-game/spec.md`

## Summary

Implement a fully playable Tetris console game for .NET 10 / C# 14 with strict layer separation: pure Domain logic, console Rendering, and non-blocking Input. Gameplay includes 7-bag piece generation, collision-safe movement/rotation, line clear scoring, level-based speed scaling, game over/replay flow, and xUnit coverage for all domain invariants.

## Technical Context

**Language/Version**: C# 14, .NET 10 (`net10.0`)  
**Primary Dependencies**: .NET SDK runtime libraries only; `xunit` for test project  
**Storage**: N/A (in-memory game state only)  
**Testing**: xUnit (`dotnet test`), no third-party mocking libraries  
**Target Platform**: Cross-platform terminal (Windows/macOS/Linux) via `System.Console`  
**Project Type**: Console application + unit test project  
**Performance Goals**: Stable game loop cadence with configurable tick; input reflected within one tick; minimal render flicker  
**Constraints**: No external NuGet packages except xUnit for tests; synchronous main loop; keyboard polling must not block (`Console.KeyAvailable`)  
**Scale/Scope**: Single-player local game, 10x20 board, one active piece + one next preview

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Principle I (Technology Stack): PASS. Plan uses only C# 14 / .NET 10; tests with xUnit.
- Principle II (Clean Architecture): PASS. Domain is pure logic; Rendering/Input are adapters around console APIs.
- Principle III (Code Quality): PASS. Single-responsibility domain classes and descriptive English identifiers are enforced.
- Principle IV (No Global Mutable State): PASS. `GameState` is passed explicitly; no static mutable singletons.
- Principle V (Build for Extension): PASS. Piece/scoring/level behavior modeled through replaceable strategies/factories.
- Principle VI (Test Discipline): PASS. Domain movement, rotation, collision, line clear, scoring, and level progression planned with unit tests.
- Principle VII (Console Target): PASS. Delivery is a .NET console app using `System.Console`.

Post-design re-check: PASS (no violations introduced in Phase 1 artifacts).

## Project Structure

### Documentation (this feature)

```text
specs/001-tetris-console-game/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── game-behavior-contract.md
└── tasks.md
```

### Source Code (repository root)

```text
TetrisGame/
├── Program.cs
├── Domain/
│   ├── Board.cs
│   ├── Piece.cs
│   ├── PieceFactory.cs
│   ├── GameState.cs
│   └── ScoreSystem.cs
├── Rendering/
│   └── ConsoleRenderer.cs
└── Input/
    └── KeyboardInputProvider.cs

TetrisGame.Tests/
├── Domain/
│   ├── BoardTests.cs
│   ├── PieceFactoryTests.cs
│   ├── GameStateTests.cs
│   └── ScoreSystemTests.cs
└── Loop/
    └── GameTickTests.cs
```

**Structure Decision**: Two-project solution (`TetrisGame`, `TetrisGame.Tests`) with folder-level architecture boundaries inside the app project. This keeps delivery simple while preserving domain purity and testability.

## Complexity Tracking

No constitution violations requiring justification.
