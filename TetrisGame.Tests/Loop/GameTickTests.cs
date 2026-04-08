using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Loop;

public class GameTickTests
{
    [Fact]
    public void Tick_MovesPieceDown()
    {
        var state = new GameState(new Board(), new PieceFactory(), new ScoreSystem());
        var initialRow = state.ActivePiece.Row;
        state.Tick();
        Assert.Equal(initialRow + 1, state.ActivePiece.Row);
    }

    [Fact]
    public void GameEngine_ProcessCommand_MoveLeft()
    {
        var state = new GameState(new Board(), new PieceFactory(), new ScoreSystem());
        var engine = new GameEngine(state);
        var initialCol = state.ActivePiece.Col;
        engine.ProcessCommand(InputCommand.MoveLeft);
        Assert.Equal(initialCol - 1, state.ActivePiece.Col);
    }

    [Fact]
    public void GameEngine_ProcessCommand_MoveRight()
    {
        var state = new GameState(new Board(), new PieceFactory(), new ScoreSystem());
        var engine = new GameEngine(state);
        var initialCol = state.ActivePiece.Col;
        engine.ProcessCommand(InputCommand.MoveRight);
        Assert.Equal(initialCol + 1, state.ActivePiece.Col);
    }
}
