# Contract: Tetris Console Behavior

## Purpose
Define externally observable behavior for the console game loop, controls, scoring, progression, and replay flow.

## Control Contract

### Running state controls
- `LeftArrow` -> Attempt move left by 1 cell.
- `RightArrow` -> Attempt move right by 1 cell.
- `DownArrow` -> Attempt immediate drop by 1 cell; if blocked, lock piece.
- `Spacebar` -> Attempt clockwise rotation by 90 degrees.
- Any other key -> Ignored.

### GameOver state controls
- `S` or `s` -> Reset game to initial state and start new run.
- `N` or `n` -> Exit process cleanly.
- Any other key -> Ignored; remain on prompt.

## Tick Contract
- Main loop runs synchronously with configurable base tick interval (`tickMs`).
- Every tick MUST:
  1. Poll and process all available key events without blocking (`Console.KeyAvailable`).
  2. Update gravity timer and perform one row drop whenever elapsed gravity time reaches current fall interval.
  3. Lock and resolve line clears if active piece can no longer move down.
  4. Spawn next piece from 7-bag; if spawn invalid -> set game over.
  5. Render frame with board, active piece overlay, next preview, score, level.

## Validation Rules Contract
- Any move or rotation that causes out-of-bounds or overlap with settled blocks is rejected.
- Rotation uses clockwise transform and no wall-kick.
- Board dimensions are fixed at 10 columns by 20 rows.

## Piece Generation Contract
- Generator uses refillable 7-bag randomizer.
- Each bag contains exactly one of: I, O, T, S, Z, J, L.
- A new shuffled bag is generated only when current bag is exhausted.

## Scoring and Progression Contract
- Score increments on piece lock line clears:
  - 1 line: +100
  - 2 lines: +300
  - 3 lines: +500
  - 4 lines: +800
- Total lines cleared accumulates globally.
- Level rule: `level = 1 + floor(totalLines / 10)`.
- Fall interval rule: `max(1000 - ((level - 1) * 100), 100)` ms.

## Rendering Contract
- Every frame MUST display:
  - Main board (10x20)
  - Active falling piece
  - Next-piece preview (single upcoming piece)
  - Current score
  - Current level
- On game over, display final score and prompt: `¿Deseas jugar de nuevo? (S/N)`.

## Testability Contract
- Domain logic is deterministic and testable without `System.Console`.
- Input polling and rendering are adapter layers and excluded from pure domain tests.
- xUnit tests cover movement, rotation, collision checks, line clearing, scoring, level updates, and game-over detection.
