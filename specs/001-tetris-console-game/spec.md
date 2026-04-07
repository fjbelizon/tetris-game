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
