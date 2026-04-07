# Data Model: Tetris Console Game

## Entity: Cell
- Purpose: Represent a board coordinate.
- Fields:
  - `X` (int, 0-9)
  - `Y` (int, 0-19)
- Validation:
  - `X` must be within board width.
  - `Y` must be within board height for in-board checks.

## Entity: Board
- Purpose: Store settled blocks on a 10x20 playfield.
- Fields:
  - `Width` (int, fixed 10)
  - `Height` (int, fixed 20)
  - `Settled` (bool[20,10] or equivalent immutable representation)
- Behaviors:
  - `IsInside(Cell)`
  - `IsOccupied(Cell)`
  - `CanPlace(Piece)`
  - `Lock(Piece)`
  - `ClearCompleteLines()` -> returns lines cleared count
- Invariants:
  - Dimensions are always 10x20.
  - Only settled blocks are persisted in board state.

## Entity: Piece
- Purpose: Represent current tetromino state.
- Fields:
  - `Type` (enum: I, O, T, S, Z, J, L)
  - `Rotation` (int, 0-3)
  - `OriginX` (int)
  - `OriginY` (int)
  - `ShapeOffsets` (derived from type + rotation)
- Behaviors:
  - `Move(dx, dy)` -> new Piece
  - `RotateClockwise()` -> new Piece
  - `GetAbsoluteCells()` -> list of board coordinates
- Invariants:
  - Rotation wraps modulo 4.
  - O piece rotation is idempotent.

## Entity: PieceBag
- Purpose: Maintain 7-bag sequence.
- Fields:
  - `Queue<TetrominoType>`
- Behaviors:
  - `TakeNext()`
  - `RefillAndShuffleIfEmpty()`
- Invariants:
  - Every refill contains each tetromino type exactly once.

## Entity: ScoreState
- Purpose: Track progression metrics.
- Fields:
  - `Score` (int, >= 0)
  - `TotalLines` (int, >= 0)
  - `Level` (int, >= 1)
- Behaviors:
  - `ApplyLineClear(count)`
  - `CurrentFallIntervalMs()`
- Invariants:
  - Level equals `1 + (TotalLines / 10)` (integer division).
  - Fall interval is bounded by minimum 100 ms.

## Entity: GameState
- Purpose: Aggregate all runtime game information.
- Fields:
  - `Board`
  - `ActivePiece`
  - `NextPiece`
  - `PieceBag`
  - `ScoreState`
  - `IsGameOver` (bool)
- Behaviors:
  - `TryMoveLeft/Right/Down()`
  - `TryRotateClockwise()`
  - `TickGravity()`
  - `LockAndSpawnNext()`
  - `Reset()`
- State transitions:
  - `Running` -> `Running` on valid input/tick
  - `Running` -> `Running` on piece lock + spawn success
  - `Running` -> `GameOver` on spawn collision
  - `GameOver` -> `Running` on replay

## Entity: InputCommand
- Purpose: Normalize key presses into domain actions.
- Values:
  - `MoveLeft`, `MoveRight`, `SoftDrop`, `RotateClockwise`, `ReplayYes`, `ReplayNo`, `None`
- Validation:
  - Non-mapped keys become `None`.
