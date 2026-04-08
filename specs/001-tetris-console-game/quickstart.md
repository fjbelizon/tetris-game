# Quickstart: Tetris Console Game

## Prerequisites
- .NET 10 SDK installed.
- Terminal capable of running `System.Console` applications.

## Create solution and projects

```shell
dotnet new sln -n TetrisGame

dotnet new console -n TetrisGame -f net10.0
dotnet new xunit -n TetrisGame.Tests -f net10.0

dotnet sln TetrisGame.sln add TetrisGame/TetrisGame.csproj
dotnet sln TetrisGame.sln add TetrisGame.Tests/TetrisGame.Tests.csproj

dotnet add TetrisGame.Tests/TetrisGame.Tests.csproj reference TetrisGame/TetrisGame.csproj
```

## Configure language version

Add `<LangVersion>14</LangVersion>` to both project files.

## Final project structure

```text
TetrisGame/
├── Program.cs
├── Domain/
│   ├── Board.cs
│   ├── GameEngine.cs
│   ├── GameState.cs
│   ├── InputCommand.cs
│   ├── Piece.cs
│   ├── PieceFactory.cs
│   └── ScoreSystem.cs
├── Rendering/
│   └── ConsoleRenderer.cs
└── Input/
    └── KeyboardInputProvider.cs

TetrisGame.Tests/
├── Domain/
│   ├── BoardCollisionTests.cs
│   ├── BoardLineClearTests.cs
│   ├── BoardLockTests.cs
│   ├── BoardTests.cs
│   ├── GameStateGameOverTests.cs
│   ├── LevelProgressionTests.cs
│   ├── PieceFactoryTests.cs
│   ├── PieceRotationTests.cs
│   ├── PieceTests.cs
│   └── ScoreSystemTests.cs
└── Loop/
    └── GameTickTests.cs
```

## Build

```shell
dotnet build TetrisGame.sln
```

## Run the game

```shell
dotnet run --project TetrisGame/TetrisGame.csproj
```

## Keyboard controls

| Key | Action |
|-----|--------|
| ← Left arrow | Move piece left |
| → Right arrow | Move piece right |
| ↓ Down arrow | Accelerated drop |
| Space | Rotate piece 90° clockwise |

## Game-over prompt

When a new piece cannot be placed, the game-over screen appears displaying the final score and the prompt (the application uses Spanish for this prompt as per the spec):

```
¿Deseas jugar de nuevo? (S/N)
```

- Press **S** (`Sí` / Yes) to reset the board, score, and level and start a new game.
- Press **N** (`No`) to exit the application.

## Run the test suite

```shell
dotnet test TetrisGame.sln
```

## Runtime behavior checklist
- Main game loop is synchronous.
- Tick interval is configurable (for example, constructor/config value).
- Input polling is non-blocking using `Console.KeyAvailable`.
- Gravity interval follows level rule: `max(1000 - ((level - 1) * 100), 100)` ms.
- Rendering always includes board, next piece, score, and level.
- Level increments every 10 lines cleared; fall speed increases accordingly.
