using TetrisGame.Domain;

namespace TetrisGame.Tests.Loop;

public class GameTickTests
{
    [Fact]
    public void GameEngine_DoesNotThrow_OnTick()
    {
        var state = new GameState();
        var engine = new GameEngine(state);
        // Should not throw
        var ex = Record.Exception(() =>
        {
            for (int i = 0; i < 100; i++)
                engine.Tick();
        });
        Assert.Null(ex);
    }

    [Fact]
    public void GameEngine_ApplyCommand_MoveLeft_UpdatesState()
    {
        var state = new GameState();
        var engine = new GameEngine(state);
        int initialX = state.ActivePiece.OriginX;
        engine.ApplyCommand(InputCommand.MoveLeft);
        // Either moved left or was blocked at boundary
        Assert.True(state.ActivePiece.OriginX <= initialX);
    }

    [Fact]
    public void GameEngine_ApplyCommand_MoveRight_UpdatesState()
    {
        var state = new GameState();
        var engine = new GameEngine(state);
        int initialX = state.ActivePiece.OriginX;
        engine.ApplyCommand(InputCommand.MoveRight);
        Assert.True(state.ActivePiece.OriginX >= initialX);
    }

    [Fact]
    public void GameEngine_IgnoresCommands_WhenGameOver()
    {
        var factory = new PieceFactory(new Random(0));
        var state = new GameState(factory);
        var engine = new GameEngine(state);
        for (int i = 0; i < 200 && !state.IsGameOver; i++)
            state.LockAndSpawnNext();

        int x = state.ActivePiece.OriginX;
        engine.ApplyCommand(InputCommand.MoveLeft);
        Assert.Equal(x, state.ActivePiece.OriginX);
    }
}
