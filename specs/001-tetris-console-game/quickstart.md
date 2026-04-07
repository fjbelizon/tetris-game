# Quickstart: Tetris Console Game

## Prerequisites
- .NET 10 SDK installed.
- Terminal capable of running `System.Console` applications.

## Create solution and projects

```powershell
Set-Location c:\GitHub\fjbelizon\tetris-game

dotnet new sln -n TetrisGame

dotnet new console -n TetrisGame -f net10.0
dotnet new xunit -n TetrisGame.Tests -f net10.0

dotnet sln TetrisGame.sln add .\TetrisGame\TetrisGame.csproj
dotnet sln TetrisGame.sln add .\TetrisGame.Tests\TetrisGame.Tests.csproj

dotnet add .\TetrisGame.Tests\TetrisGame.Tests.csproj reference .\TetrisGame\TetrisGame.csproj
```

## Configure language version

Add `<LangVersion>14</LangVersion>` to both project files.

## Suggested folders

```text
TetrisGame/
├── Domain/
├── Rendering/
├── Input/
└── Program.cs

TetrisGame.Tests/
└── Domain/
```

## Build and test

```powershell
dotnet build .\TetrisGame.sln
dotnet test .\TetrisGame.sln
```

## Runtime behavior checklist
- Main game loop is synchronous.
- Tick interval is configurable (for example, constructor/config value).
- Input polling is non-blocking using `Console.KeyAvailable`.
- Gravity interval follows level rule: `max(1000 - ((level - 1) * 100), 100)` ms.
- Rendering always includes board, next piece, score, and level.
