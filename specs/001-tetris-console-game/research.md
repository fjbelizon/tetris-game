# Research: Tetris Console Game

## Decision 1: Layered architecture inside a single console app project
- Decision: Keep one production project (`TetrisGame`) with `Domain/`, `Rendering/`, and `Input/` folders, plus a dedicated test project (`TetrisGame.Tests`).
- Rationale: Matches required structure while preserving clear boundaries and low setup overhead for a console game.
- Alternatives considered: Separate production projects per layer (`TetrisGame.Domain`, `TetrisGame.Rendering`, `TetrisGame.Input`) were rejected for this iteration because they add project-management overhead without immediate runtime benefit.

## Decision 2: Synchronous fixed-tick main loop with elapsed-time accumulation
- Decision: Use a synchronous while-loop with configurable tick (e.g., 16-50 ms), update gravity via accumulated elapsed milliseconds, and process one frame per tick.
- Rationale: Meets requirement for synchronous game loop and deterministic behavior while allowing gravity interval changes by level.
- Alternatives considered: Async loop with timers/tasks was rejected because it adds complexity and can introduce timing drift/races for console apps.

## Decision 3: Non-blocking keyboard polling via `Console.KeyAvailable`
- Decision: Poll keyboard in each tick; if keys are available, drain events in order and dispatch commands.
- Rationale: Prevents input from blocking the loop and satisfies the explicit non-blocking requirement.
- Alternatives considered: `Console.ReadKey(true)` blocking call was rejected because it stalls game progression with no input.

## Decision 4: 7-bag piece generation
- Decision: Implement `PieceFactory` with a refillable shuffled bag containing one of each tetromino type (I, O, T, S, Z, J, L).
- Rationale: Required by spec and yields fairer distribution than independent random picks.
- Alternatives considered: Uniform random piece per spawn was rejected due to drought risk and mismatch with accepted behavior.

## Decision 5: Rotation and collision policy
- Decision: Apply basic clockwise rotation matrices per piece state and reject movement/rotation when target cells are out-of-bounds or occupied.
- Rationale: Explicitly required; no wall-kick/SRS behavior in this scope.
- Alternatives considered: SRS with wall-kicks was rejected as out-of-scope complexity.

## Decision 6: Scoring and level progression strategy
- Decision: Keep scoring and progression rules in `ScoreSystem` with table {1:100, 2:300, 3:500, 4:800}; level increments each 10 lines; gravity interval `max(1000 - ((level-1)*100), 100)` ms.
- Rationale: Centralizes gameplay policy and allows replacing strategy later.
- Alternatives considered: Inline scoring logic in game loop was rejected due to lower testability and poor separation of concerns.

## Decision 7: Rendering approach
- Decision: Redraw a stable console frame per tick with board, active piece overlay, next piece preview, score, and level.
- Rationale: Keeps implementation straightforward and portable under `System.Console`.
- Alternatives considered: Full-screen ANSI diff rendering was rejected for higher complexity and terminal compatibility concerns.

## Decision 8: Test strategy with xUnit only
- Decision: Cover all domain invariants with xUnit tests and hand-written test doubles where needed; avoid external mocking libraries.
- Rationale: Required by constitution and dependency constraints.
- Alternatives considered: Additional assertion/mocking packages were rejected by dependency policy.
