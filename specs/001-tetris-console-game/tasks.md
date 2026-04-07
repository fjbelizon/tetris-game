# Tasks: Tetris Console Game MVP

**Input**: Design documents from `/specs/001-tetris-console-game/`
**Prerequisites**: plan.md (required), spec.md (required), research.md, data-model.md, contracts/

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: User story label (`[US1]`, `[US2]`, `[US3]`, `[US4]`) for story-phase tasks
- Every task includes an explicit file path

## Phase 1: Fundacion - Scaffolding y Test Setup

**Purpose**: Initialize solution structure and xUnit configuration.

- [ ] T001 Create .NET solution and projects in TetrisGame.sln (#127)
- [ ] T002 Create architecture folders and placeholders in TetrisGame/Domain/.gitkeep (#128)
- [ ] T003 [P] Configure xUnit project reference and test runner in TetrisGame.Tests/TetrisGame.Tests.csproj (#129)
- [ ] T004 [P] Set C# 14 language version in TetrisGame/TetrisGame.csproj (#130)

---

## Phase 2: Dominio Puro - Board, Piece, Colisiones, Lineas, Puntuacion, Niveles

**Purpose**: Build all pure domain rules before loop, input, and rendering.

**Critical rule**: Each domain implementation task has a corresponding unit test task.

- [ ] T005 [P] Add board invariants tests (10x20, bounds) in TetrisGame.Tests/Domain/BoardTests.cs (#131)
- [ ] T006 Implement Board dimensions and bounds checks in TetrisGame/Domain/Board.cs (#132)
- [ ] T007 [P] Add piece state tests (type, rotation, origin) in TetrisGame.Tests/Domain/PieceTests.cs (#133)
- [ ] T008 Implement Piece state and movement operations in TetrisGame/Domain/Piece.cs (#134)
- [ ] T009 [P] Add 7-bag distribution tests for all tetrominoes in TetrisGame.Tests/Domain/PieceFactoryTests.cs (#135)
- [ ] T010 Implement PieceFactory with shuffled 7-bag generator in TetrisGame/Domain/PieceFactory.cs (#136)
- [ ] T011 [P] Add clockwise rotation behavior tests by piece type in TetrisGame.Tests/Domain/PieceRotationTests.cs (#137)
- [ ] T012 Implement clockwise rotation rules in TetrisGame/Domain/Piece.cs (#138)
- [ ] T013 [P] Add collision rejection tests (walls and settled cells) in TetrisGame.Tests/Domain/BoardCollisionTests.cs (#139)
- [ ] T014 Implement collision validation (`CanPlace`) in TetrisGame/Domain/Board.cs (#140)
- [ ] T015 [P] Add lock/fixation tests when downward move is blocked in TetrisGame.Tests/Domain/BoardLockTests.cs (#141)
- [ ] T016 Implement piece lock/fixation behavior in TetrisGame/Domain/Board.cs (#142)
- [ ] T017 [P] Add line clear tests (single and multi-line) in TetrisGame.Tests/Domain/BoardLineClearTests.cs (#143)
- [ ] T018 Implement complete-line cleanup and row shift in TetrisGame/Domain/Board.cs (#144)
- [ ] T019 [P] Add scoring table tests (1/2/3/4 lines -> 100/300/500/800) in TetrisGame.Tests/Domain/ScoreSystemTests.cs (#145)
- [ ] T020 Implement score update policy in TetrisGame/Domain/ScoreSystem.cs (#146)
- [ ] T021 [P] Add level and gravity interval tests (`max(1000-((level-1)*100),100)`) in TetrisGame.Tests/Domain/LevelProgressionTests.cs (#147)
- [ ] T022 Implement level progression and interval calculation in TetrisGame/Domain/ScoreSystem.cs (#148)

**Checkpoint**: Domain core is complete and fully unit-tested.

---

## Phase 3: User Story 1 - Game Loop e Input No Bloqueante (Priority: P1) MVP

**Goal**: Make gameplay advance by tick and react to non-blocking keyboard input.

**Independent Test**: Start the app and verify automatic gravity tick, left/right/down/rotate input response, and no blocking while idle.

- [ ] T023 [P] [US1] Add game loop cadence tests in TetrisGame.Tests/Loop/GameTickTests.cs (#149)
- [ ] T024 [US1] Implement synchronous tick loop and gravity accumulator in TetrisGame/Domain/GameEngine.cs (#150)
- [ ] T025 [US1] Implement input command enum and mapping contract in TetrisGame/Domain/InputCommand.cs (#151)
- [ ] T026 [US1] Implement non-blocking keyboard polling (`Console.KeyAvailable`) in TetrisGame/Input/KeyboardInputProvider.cs (#152)
- [ ] T027 [US1] Integrate input + tick orchestration in TetrisGame/Program.cs (#153)
- [ ] T028 [US1] Integrate lock-and-spawn flow into runtime state in TetrisGame/Domain/GameState.cs (#154)

**Checkpoint**: US1 fully playable with core loop and controls.

---

## Phase 4: User Story 2 y 3 - Renderizado en Consola + Info Panel (Priorities: P2, P3)

**Goal**: Render board state every frame and show next piece, score, and level updates.

**Independent Test (US2)**: Clear lines and verify score changes appear immediately in the info panel.
**Independent Test (US3)**: Reach every 10 cleared lines and verify level and fall speed updates are reflected.

- [ ] T029 [US2] Implement frame rendering for board + active piece overlay in TetrisGame/Rendering/ConsoleRenderer.cs (#155)
- [ ] T030 [US2] Implement next-piece preview panel in TetrisGame/Rendering/ConsoleRenderer.cs (#156)
- [ ] T031 [US2] Render current score in info panel in TetrisGame/Rendering/ConsoleRenderer.cs (#157)
- [ ] T032 [US3] Render current level in info panel in TetrisGame/Rendering/ConsoleRenderer.cs (#158)
- [ ] T033 [US3] Apply updated gravity interval after level-up in TetrisGame/Domain/GameEngine.cs (#159)
- [ ] T034 [US2] Wire renderer update cycle in main runtime loop in TetrisGame/Program.cs (#160)

**Checkpoint**: US2 and US3 are visible and verifiable through console UI updates.

---

## Phase 5: User Story 4 - Game Over y Reinicio (Priority: P4)

**Goal**: End session correctly on spawn collision and support replay/exit prompt.

**Independent Test**: Force spawn overlap, verify game-over screen + final score, then validate `S` resets state and `N` exits cleanly.

- [ ] T035 [P] [US4] Add game-over and replay flow tests in TetrisGame.Tests/Domain/GameStateGameOverTests.cs (#161)
- [ ] T036 [US4] Implement spawn-collision game-over transition in TetrisGame/Domain/GameState.cs (#162)
- [ ] T037 [US4] Implement game-over screen with final score and replay prompt in TetrisGame/Rendering/ConsoleRenderer.cs (#163)
- [ ] T038 [US4] Implement replay (`S`) and exit (`N`) control handling in TetrisGame/Program.cs (#164)
- [ ] T039 [US4] Implement full reset behavior (board, score, level, bag) in TetrisGame/Domain/GameState.cs (#165)

**Checkpoint**: Full game lifecycle (play -> game over -> restart/exit) is complete.

---

## Phase 6: Polish y Validacion Cruzada

**Purpose**: Final verification and handoff quality.

- [ ] T040 Execute full regression test suite in TetrisGame.Tests/ (#166)
- [ ] T041 Align run/test steps with final implementation in specs/001-tetris-console-game/quickstart.md (#167)
- [ ] T042 Add MVP smoke checklist for manual validation in specs/001-tetris-console-game/checklists/mvp-smoke.md (#168)

---

## Dependencies & Execution Order

### Phase Dependencies

- Phase 1 -> no dependencies
- Phase 2 -> depends on Phase 1; blocks all user-story work
- Phase 3 -> depends on Phase 2 (MVP core)
- Phase 4 -> depends on Phase 3
- Phase 5 -> depends on Phase 3 (and consumes rendering from Phase 4)
- Phase 6 -> depends on Phases 3, 4, and 5

### User Story Dependencies

- US1 (P1): starts after Phase 2; no dependency on other stories
- US2 (P2): depends on US1 runtime loop integration
- US3 (P3): depends on US1 runtime and score/line updates from domain
- US4 (P4): depends on US1 runtime and state transitions

### Parallel Opportunities

- Setup: T003 and T004 can run in parallel
- Domain tests in Phase 2 marked [P] can run in parallel before their paired implementation tasks
- US1: T023 can run in parallel with T025/T026
- US4: T035 can run in parallel with T037 once state contract is stable

---

## Parallel Example per User Story

### US1

- Task: `T023 [P] [US1] Add game loop cadence tests in TetrisGame.Tests/Loop/GameTickTests.cs`
- Task: `T025 [US1] Implement input command enum and mapping contract in TetrisGame/Domain/InputCommand.cs`
- Task: `T026 [US1] Implement non-blocking keyboard polling (Console.KeyAvailable) in TetrisGame/Input/KeyboardInputProvider.cs`

### US2

- Task: `T030 [US2] Implement next-piece preview panel in TetrisGame/Rendering/ConsoleRenderer.cs`
- Task: `T031 [US2] Render current score in info panel in TetrisGame/Rendering/ConsoleRenderer.cs`

### US3

- Task: `T032 [US3] Render current level in info panel in TetrisGame/Rendering/ConsoleRenderer.cs`
- Task: `T033 [US3] Apply updated gravity interval after level-up in TetrisGame/Domain/GameEngine.cs`

### US4

- Task: `T035 [P] [US4] Add game-over and replay flow tests in TetrisGame.Tests/Domain/GameStateGameOverTests.cs`
- Task: `T037 [US4] Implement game-over screen with final score and replay prompt in TetrisGame/Rendering/ConsoleRenderer.cs`

---

## Implementation Strategy

### Suggested MVP Scope

1. Complete Phase 1
2. Complete Phase 2
3. Complete Phase 3 (US1)
4. Validate playable loop end-to-end

### Incremental Delivery

1. Add Phase 4 for UI completeness (US2/US3 visibility)
2. Add Phase 5 for complete lifecycle (US4)
3. Run Phase 6 for release readiness

---

## Notes

- All tasks use strict checklist format: checkbox + ID + optional `[P]` + optional `[USx]` + file path.
- Domain implementation tasks in Phase 2 each have a corresponding unit test task.
- Priorities are preserved through execution order: P1 -> P2/P3 -> P4.
