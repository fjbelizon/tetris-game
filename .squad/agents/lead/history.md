

<!-- enmarchia:specs:start -->
## Enmarchia Project Specifications

> The following specifications define the work I am expected to implement. Each task corresponds to a GitHub Issue I should work on.

*Last synced: 2026-04-07T12:57:54.867Z*

### Feature: Tetris Console Game

**What to build:**

# Feature Specification: Tetris Console Game

**Feature Branch**: `001-tetris-console-game`
**Created**: 2026-04-07
**Status**: Draft
**Input**: User description: "Implementar un juego de Tetris básico funcional como aplicación de consola para .NET 10 y C# 14."

## Clarifications

### Session 2026-04-07

- Q: How should the game choose the next tetromino? → A: 7-bag randomizer
- Q: How should fall speed scale by level? → A: Subtract 100 ms per level from 1000 ms, minimum 100 ms

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Play a Complete Game of Tetris (Priority: P1)

A player launches the application, watches pieces fall, controls the active piece with the keyboard, and plays until reaching game over.

**Why this priority**: This is the core gameplay loop — without it, the game does not exist. All other stories depend on this foundation.

**Independent Test**: Can be fully tested by launching the application and playing until game over, verifying that pieces fall, controls respond, lines clear, and the game ends correctly.

**Acceptance Scenarios**:

1. **Given** the game is launched, **When** it starts, **Then** the board (10 columns × 20 rows) is shown empty, the first piece from a shuffled 7-bag appears at the top, and the next-piece preview is displayed.
2. **Given** a piece is falling, **When** the player presses the left arrow, **Then** the piece moves one column to the left (if not blocked by the wall or a settled piece).
3. **Given** a piece is falling, **When** the player presses the right arrow, **Then** the piece moves one column to the right (if not blocked).
4. **Given** a piece is falling, **When** the player presses the down arrow, **Then** the piece drops one additional row immediately (accelerated fall).
5. **Given** a piece is falling, **When** the player presses the space bar, **Then** the piece rotates 90 degrees clockwise (if the rotated position is not blocked; otherwise the rotation is rejected and the piece stays as-is).
6. **Given** one second has elapsed since the last automatic drop, **When** the game tick fires, **Then** the active piece moves down one row automatically.
7. **Given** an active piece cannot move further down (it has hit the floor or a settled piece), **When** the piece locks, **Then** it becomes part of the board and the next piece from the 7-bag appears at the top.
8. **Given** a new piece spawns at the top, **When** its spawn position overlaps with already-settled blocks, **Then** the game ends and the game-over screen is displayed.

---

### User Story 2 - Clear Lines and Score Points (Priority: P2)

A player fills one or more horizontal rows completely, triggering line clears and receiving points.

**Why this priority**: Line clearing is the primary scoring mechanic and the game's strategic challenge. Without it, pieces would stack indefinitely with no objective.

**Independent Test**: Can be tested independently by filling rows until one or more complete, verifying they are removed, above rows descend, and the score updates correctly.

**Acceptance Scenarios**:

1. **Given** a horizontal row on the board is completely filled, **When** a piece locks, **Then** that row is removed, all rows above it shift down by one, and the score increases.
2. **Given** exactly 1 line is cleared at once, **When** the clear happens, **Then** the score increases by 100 points.
3. **Given** exactly 2 lines are cleared simultaneously, **When** the clear happens, **Then** the score increases by 300 points.
4. **Given** exactly 3 lines are cleared simultaneously, **When** the clear happens, **Then** the score increases by 500 points.
5. **Given** exactly 4 lines are cleared simultaneously (Tetris), **When** the clear happens, **Then** the score increases by 800 points.
6. **Given** the current score and level are displayed on screen, **When** a line is cleared, **Then** the score and level update immediately on screen without redrawing the whole board unnecessarily.

---

### User Story 3 - Level Progression and Speed Increase (Priority: P3)

A player accumulates cleared lines and sees the game speed increase as levels advance.

**Why this priority**: Levels add long-term challenge and replayability. The core loop is complete without it, but progression is a key motivational element.

**Independent Test**: Can be tested independently by clearing 10 lines and observing the level counter increment and the automatic fall cadence visibly increase.

**Acceptance Scenarios**:

1. **Given** the player has cleared a total of 10 lines, **When** the 10th line is cleared, **Then** the level increments by 1 and the automatic fall speed increases.
2. **Given** the level has increased, **When** the next pieces fall, **Then** they fall noticeably faster than in the previous level.
3. **Given** the current level is displayed on screen, **When** a level-up occurs, **Then** the displayed level number updates immediately.

---

### User Story 4 - Game Over and Replay (Priority: P4)

A player reaches game over, sees their final score, and decides whether to play again.

**Why this priority**: A clean game-over flow and replay option complete the game session lifecycle and are essential for user retention.

**Independent Test**: Can be tested by stacking pieces until the board fills to the top, verifying the game-over screen appears with the final score and a replay prompt.

**Acceptance Scenarios**:

1. **Given** a new piece cannot be placed because spawn cells are occupied, **When** the game detects this, **Then** all controls are disabled, the board freezes, and the game-over screen appears.
2. **Given** the game-over screen is shown, **When** it appears, **Then** it displays the final score and the prompt "¿Deseas jugar de nuevo? (S/N)".
3. **Given** the game-over screen is shown, **When** the player presses 'S', **Then** the board resets, score resets to 0, level resets to 1, and a new game begins.
4. **Given** the game-over screen is shown, **When** the player presses 'N', **Then** the application exits cleanly.

---

### Edge Cases

- What happens when a rotation would move the piece outside the board boundary? → Rotation is rejected; the piece keeps its current orientation.
- What happens when a rotation would overlap a settled block? → Rotation is rejected; the piece keeps its current orientation.
- What happens when the down-arrow is pressed and the piece is already on the floor or touching a settled piece? → The piece locks immediately.
- How does the system handle multiple simultaneous key presses? → Each key event is processed individually in the order received; simultaneous inputs are serialized.
- What happens when 4 lines are cleared simultaneously (Tetris)? → 800 points are awarded; all four rows are removed and above rows descend by 4.
- What happens at very high levels where fall speed could become near-instant? → Fall interval is capped at a minimum of 100 ms per row to keep the game playable.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The system MUST display a game board of exactly 10 columns × 20 rows in the console.
- **FR-002**: The system MUST support all 7 standard Tetris pieces: I, O, T, S, Z, J, and L.
- **FR-003**: The active piece MUST fall automatically at a rate of one row per second at level 1.
- **FR-004**: The player MUST be able to move the active piece left and right using the left/right arrow keys.
- **FR-005**: The player MUST be able to accelerate the active piece's fall using the down arrow key.
- **FR-006**: The player MUST be able to rotate the active piece 90 degrees clockwise using the space bar.
- **FR-007**: The system MUST reject any move or rotation that would place blocks outside the board or overlapping settled blocks.
- **FR-008**: The system MUST lock an active piece in place when it can no longer move downward.
- **FR-009**: After locking a piece, the system MUST spawn the next tetromino from a shuffled 7-bag queue and refill the queue by reshuffling all 7 tetromino types when the bag is exhausted.
- **FR-010**: The system MUST detect and clear any fully filled horizontal row after each piece lock.
- **FR-011**: The system MUST shift all rows above a cleared row down by one row for each row cleared.
- **FR-012**: The system MUST score cleared lines as follows: 1 line = 100 pts, 2 lines = 300 pts, 3 lines = 500 pts, 4 lines = 800 pts.
- **FR-013**: The system MUST increment the level by 1 for every 10 lines cleared.
- **FR-014**: The system MUST compute fall interval as `max(1000 ms - ((level - 1) * 100 ms), 100 ms)` and apply it immediately after each level-up.
- **FR-015**: The game display MUST always show: the board, the active piece, the next-piece preview, current score, and current level.
- **FR-016**: The system MUST detect game over when a newly spawned piece cannot be placed on the board.
- **FR-017**: On game over, the system MUST display the final score and ask the player if they want to play again.
- **FR-018**: If the player chooses to replay, the system MUST reset the board, score, and level and start a new game.
- **FR-019**: If the player chooses not to replay, the application MUST exit cleanly.
- **FR-020**: The automatic fall interval MUST never be lower than 100 ms per row at any level.

### Key Entities

- **Board**: A 10 × 20 grid that records which cells are occupied by settled blocks.
- **Tetromino (Piece)**: One of the 7 standard shapes (I, O, T, S, Z, J, L), defined by a set of block coordinates relative to a pivot. Has a type, current rotation state, and current position on the board.
- **Active Piece**: The currently falling tetromino controlled by the player.
- **Next Piece**: The upcoming tetromino shown in the preview area.
- **Game State**: Encapsulates the board, active piece, next piece, score, level, total lines cleared, and game-over flag.
- **Piece Bag**: A shuffled collection containing exactly one instance of each of the 7 tetromino types, used to determine piece order until exhausted and then regenerated.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: A player can launch the game and reach the first line-clear event without any crash or rendering artifact.
- **SC-002**: All 7 piece types appear during normal play with observable visual distinction on the board.
- **SC-003**: Keyboard input (move, rotate, accelerate) is reflected on screen within one game cycle (no perceptible lag).
- **SC-004**: Score increments match the defined point table for 1, 2, 3, and 4 simultaneous line clears — verifiable by manual play or input simulation.
- **SC-005**: Level increments every 10 cumulative lines cleared, and measured fall interval follows `max(1000 ms - ((level - 1) * 100 ms), 100 ms)`.
- **SC-006**: Game-over detection is reliable — the game never continues after spawn overlap occurs.
- **SC-007**: Replay resets score to 0 and level to 1 cleanly, with no leftover blocks from the previous game.
- **SC-008**: The application exits without errors when the player declines to replay.

## Assumptions

- The application targets .NET 10 and C# 14, compiled and run as a console application.
- The console must support standard rendering sufficient to redraw the board each frame without excessive flicker; no third-party TUI library is required.
- A single player controls the game; no multiplayer or networked play is in scope.
- High-score persistence to disk is out of scope for this version.
- Sound effects are out of scope for this version.
- Color support per piece type (using console color APIs) is desirable but not mandatory; pieces can fall back to a single color if the terminal does not support it.
- Wall-kick or advanced SRS rotation rules are out of scope; basic rotation with rejection is sufficient.
- The next-piece preview shows only the single upcoming piece; a queue of multiple upcoming pieces is out of scope.
- Piece generation uses the standard 7-bag randomizer rather than independent per-piece random selection.
- The game runs in a game-loop with a configurable tick interval; concurrency implementation details are left to the implementor.

**How to build it:**

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

**Pending work:**

- [ ] T001 Create .NET solution and projects in TetrisGame.sln (Issue #43)
- [ ] T002 Create architecture folders and placeholders in TetrisGame/Domain/.gitkeep (Issue #44)
- [ ] T003 [P] Configure xUnit project reference and test runner in TetrisGame.Tests/TetrisGame.Tests.csproj (Issue #45)
- [ ] T004 [P] Set C# 14 language version in TetrisGame/TetrisGame.csproj (Issue #46)
- [ ] T005 [P] Add board invariants tests (10x20, bounds) in TetrisGame.Tests/Domain/BoardTests.cs (Issue #47)
- [ ] T006 Implement Board dimensions and bounds checks in TetrisGame/Domain/Board.cs (Issue #48)
- [ ] T007 [P] Add piece state tests (type, rotation, origin) in TetrisGame.Tests/Domain/PieceTests.cs (Issue #49)
- [ ] T008 Implement Piece state and movement operations in TetrisGame/Domain/Piece.cs (Issue #50)
- [ ] T009 [P] Add 7-bag distribution tests for all tetrominoes in TetrisGame.Tests/Domain/PieceFactoryTests.cs (Issue #51)
- [ ] T010 Implement PieceFactory with shuffled 7-bag generator in TetrisGame/Domain/PieceFactory.cs (Issue #52)
- [ ] T011 [P] Add clockwise rotation behavior tests by piece type in TetrisGame.Tests/Domain/PieceRotationTests.cs (Issue #53)
- [ ] T012 Implement clockwise rotation rules in TetrisGame/Domain/Piece.cs (Issue #54)
- [ ] T013 [P] Add collision rejection tests (walls and settled cells) in TetrisGame.Tests/Domain/BoardCollisionTests.cs (Issue #55)
- [ ] T014 Implement collision validation (`CanPlace`) in TetrisGame/Domain/Board.cs (Issue #56)
- [ ] T015 [P] Add lock/fixation tests when downward move is blocked in TetrisGame.Tests/Domain/BoardLockTests.cs (Issue #57)
- [ ] T016 Implement piece lock/fixation behavior in TetrisGame/Domain/Board.cs (Issue #58)
- [ ] T017 [P] Add line clear tests (single and multi-line) in TetrisGame.Tests/Domain/BoardLineClearTests.cs (Issue #59)
- [ ] T018 Implement complete-line cleanup and row shift in TetrisGame/Domain/Board.cs (Issue #60)
- [ ] T019 [P] Add scoring table tests (1/2/3/4 lines -> 100/300/500/800) in TetrisGame.Tests/Domain/ScoreSystemTests.cs (Issue #61)
- [ ] T020 Implement score update policy in TetrisGame/Domain/ScoreSystem.cs (Issue #62)
- [ ] T021 [P] Add level and gravity interval tests (`max(1000-((level-1)*100),100)`) in TetrisGame.Tests/Domain/LevelProgressionTests.cs (Issue #63)
- [ ] T022 Implement level progression and interval calculation in TetrisGame/Domain/ScoreSystem.cs (Issue #64)
- [ ] T023 [P] [US1] Add game loop cadence tests in TetrisGame.Tests/Loop/GameTickTests.cs (Issue #65)
- [ ] T024 [US1] Implement synchronous tick loop and gravity accumulator in TetrisGame/Domain/GameEngine.cs (Issue #66)
- [ ] T025 [US1] Implement input command enum and mapping contract in TetrisGame/Domain/InputCommand.cs (Issue #67)
- [ ] T026 [US1] Implement non-blocking keyboard polling (`Console.KeyAvailable`) in TetrisGame/Input/KeyboardInputProvider.cs (Issue #68)
- [ ] T027 [US1] Integrate input + tick orchestration in TetrisGame/Program.cs (Issue #69)
- [ ] T028 [US1] Integrate lock-and-spawn flow into runtime state in TetrisGame/Domain/GameState.cs (Issue #70)
- [ ] T029 [US2] Implement frame rendering for board + active piece overlay in TetrisGame/Rendering/ConsoleRenderer.cs (Issue #71)
- [ ] T030 [US2] Implement next-piece preview panel in TetrisGame/Rendering/ConsoleRenderer.cs (Issue #72)
- [ ] T031 [US2] Render current score in info panel in TetrisGame/Rendering/ConsoleRenderer.cs (Issue #73)
- [ ] T032 [US3] Render current level in info panel in TetrisGame/Rendering/ConsoleRenderer.cs (Issue #74)
- [ ] T033 [US3] Apply updated gravity interval after level-up in TetrisGame/Domain/GameEngine.cs (Issue #75)
- [ ] T034 [US2] Wire renderer update cycle in main runtime loop in TetrisGame/Program.cs (Issue #76)
- [ ] T035 [P] [US4] Add game-over and replay flow tests in TetrisGame.Tests/Domain/GameStateGameOverTests.cs (Issue #77)
- [ ] T036 [US4] Implement spawn-collision game-over transition in TetrisGame/Domain/GameState.cs (Issue #78)
- [ ] T037 [US4] Implement game-over screen with final score and replay prompt in TetrisGame/Rendering/ConsoleRenderer.cs (Issue #79)
- [ ] T038 [US4] Implement replay (`S`) and exit (`N`) control handling in TetrisGame/Program.cs (Issue #80)
- [ ] T039 [US4] Implement full reset behavior (board, score, level, bag) in TetrisGame/Domain/GameState.cs (Issue #81)
- [ ] T040 Execute full regression test suite in TetrisGame.Tests/ (Issue #82)
- [ ] T041 Align run/test steps with final implementation in specs/001-tetris-console-game/quickstart.md (Issue #83)
- [ ] T042 Add MVP smoke checklist for manual validation in specs/001-tetris-console-game/checklists/mvp-smoke.md (Issue #84)

<!-- enmarchia:specs:end -->
