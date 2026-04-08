# MVP Smoke Checklist: Tetris Console Game

**Purpose**: Manual validation checklist for end-to-end smoke testing of the MVP build before release
**Created**: 2026-04-08
**Feature**: [spec.md](../spec.md)
**Quickstart**: [quickstart.md](../quickstart.md)

## How to Use

Run each check manually by launching the game (`dotnet run --project TetrisGame`) and exercising the described scenario. Mark each item `[x]` as it passes. All items must pass for the build to be considered smoke-ready.

---

## 1. Launch and Initial State (SC-001)

- [ ] SMK-001 Running `dotnet run --project TetrisGame` starts the application without errors or exceptions.
- [ ] SMK-002 A 10-column × 20-row board is displayed in the console, initially empty.
- [ ] SMK-003 A tetromino piece appears at the top of the board immediately on start.
- [ ] SMK-004 A next-piece preview panel is visible alongside the board.
- [ ] SMK-005 The current score (starting at 0) is displayed in the info panel.
- [ ] SMK-006 The current level (starting at 1) is displayed in the info panel.

---

## 2. Piece Movement and Controls (US1 — SC-003)

- [ ] SMK-007 Pressing the **left arrow key** moves the active piece one column to the left.
- [ ] SMK-008 Pressing the **right arrow key** moves the active piece one column to the right.
- [ ] SMK-009 Pressing the **down arrow key** drops the active piece one additional row immediately.
- [ ] SMK-010 Pressing the **space bar** rotates the active piece 90 degrees clockwise.
- [ ] SMK-011 Keyboard input is reflected on screen within one game cycle (no perceptible lag).
- [ ] SMK-012 Holding the **down arrow** accelerates falling for consecutive rows.

---

## 3. Collision and Boundary Rules (FR-007)

- [ ] SMK-013 The active piece cannot be moved left past the left wall (movement is rejected at column 0).
- [ ] SMK-014 The active piece cannot be moved right past the right wall (movement is rejected at column 9).
- [ ] SMK-015 The active piece cannot move through settled blocks.
- [ ] SMK-016 A rotation that would place the piece outside the board is rejected; the piece keeps its current orientation.
- [ ] SMK-017 A rotation that would overlap a settled block is rejected; the piece keeps its current orientation.

---

## 4. Gravity and Automatic Fall (US1 — FR-003)

- [ ] SMK-018 At level 1, the active piece automatically drops one row approximately every 1000 ms with no key presses.
- [ ] SMK-019 The piece locks in place when it can no longer move downward (reaches the floor or a settled block).

---

## 5. Piece Lock and Spawn (FR-008, FR-009 — SC-001)

- [ ] SMK-020 When a piece locks, it becomes part of the board and remains visible as a settled block.
- [ ] SMK-021 After a piece locks, the next piece from the 7-bag appears at the top of the board.
- [ ] SMK-022 The next-piece preview updates to show the upcoming piece after each spawn.

---

## 6. 7-Bag Piece Distribution (FR-009 — SC-002)

- [ ] SMK-023 All 7 tetromino types (I, O, T, S, Z, J, L) appear during a normal play session.
- [ ] SMK-024 Each piece type is visually distinct on the board (different character or color).
- [ ] SMK-025 No single piece type repeats more than once before the full set of 7 has appeared (7-bag guarantee).

---

## 7. Line Clears and Scoring (US2 — FR-010, FR-011, FR-012 — SC-004)

- [ ] SMK-026 When a horizontal row is completely filled, it is removed after the piece locks.
- [ ] SMK-027 All rows above a cleared row shift down by one row for each row cleared.
- [ ] SMK-028 Clearing **1 line** awards exactly **100 points**.
- [ ] SMK-029 Clearing **2 lines** simultaneously awards exactly **300 points**.
- [ ] SMK-030 Clearing **3 lines** simultaneously awards exactly **500 points**.
- [ ] SMK-031 Clearing **4 lines** simultaneously (Tetris) awards exactly **800 points**.
- [ ] SMK-032 The score display updates immediately after each line-clear event.
- [ ] SMK-033 When 4 lines are cleared simultaneously, all four rows are removed and the above rows descend by 4.

---

## 8. Level Progression and Speed Increase (US3 — FR-013, FR-014 — SC-005)

- [ ] SMK-034 The level increments by 1 after every cumulative 10 lines cleared (10 lines → level 2, 20 lines → level 3, etc.).
- [ ] SMK-035 The level display updates immediately after the level-up event.
- [ ] SMK-036 After a level-up, pieces fall noticeably faster than in the previous level.
- [ ] SMK-037 At level 2, the measured fall interval is approximately **900 ms** per row.
- [ ] SMK-038 At level 10, the measured fall interval is approximately **100 ms** per row (minimum).
- [ ] SMK-039 The fall interval never drops below **100 ms** per row, regardless of level.

---

## 9. Game Over Detection and Screen (US4 — FR-016, FR-017 — SC-006)

- [ ] SMK-040 When a newly spawned piece's position overlaps with settled blocks, the game detects game over.
- [ ] SMK-041 On game over, all controls stop responding (board is frozen).
- [ ] SMK-042 The game-over screen displays the final score.
- [ ] SMK-043 The game-over screen shows the prompt `¿Deseas jugar de nuevo? (S/N)`.

---

## 10. Replay and Exit (US4 — FR-018, FR-019 — SC-007, SC-008)

- [ ] SMK-044 Pressing **S** at the game-over screen resets the board to empty.
- [ ] SMK-045 Pressing **S** resets the score to **0**.
- [ ] SMK-046 Pressing **S** resets the level to **1**.
- [ ] SMK-047 Pressing **S** starts a new game immediately with no leftover blocks from the previous session.
- [ ] SMK-048 Pressing **N** at the game-over screen exits the application cleanly (no exception or error output).

---

## 11. Edge Cases (spec.md — Edge Cases section)

- [ ] SMK-049 Pressing the **down arrow** when the piece is already touching the floor or a settled piece causes it to lock immediately.
- [ ] SMK-050 Pressing **S/N** during active gameplay does not trigger game-over replay/exit behavior (those keys only apply on the game-over screen).

---

## Notes

- All items must be marked `[x]` before the build is considered smoke-ready.
- If any item fails, open a bug report referencing the SMK ID (e.g., `Bug: SMK-028 — 1-line clear awards wrong score`).
- Test both Windows (PowerShell) and Unix (bash) terminals if cross-platform coverage is required.
