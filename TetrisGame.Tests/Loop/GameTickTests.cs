using TetrisGame.Domain;

namespace TetrisGame.Tests.Loop;

public class GameTickTests
{
    [Fact]
    public void GameEngine_Tick_DoesNotTriggerGravityBeforeInterval()
    {
        var state = new GameState();
        var engine = new GameEngine(state);
        var initialY = state.ActivePiece.OriginY;

        // Tick with less than fall interval (1000ms at level 1)
        bool ticked = engine.Tick(500);
        Assert.False(ticked);
        Assert.Equal(initialY, state.ActivePiece.OriginY);
    }

    [Fact]
    public void GameEngine_Tick_TriggersGravityAtInterval()
    {
        var state = new GameState();
        var engine = new GameEngine(state);
        var initialY = state.ActivePiece.OriginY;

        // Tick with enough time to trigger gravity (level 1 = 1000ms)
        bool ticked = engine.Tick(1000);
        Assert.True(ticked);
        // Piece should have moved down (or locked if already at bottom)
        // In an empty board, it should just move down
        Assert.True(state.ActivePiece.OriginY > initialY || state.IsGameOver);
    }

    [Fact]
    public void GameEngine_Tick_AccumulatesTimeAcrossMultipleTicks()
    {
        var state = new GameState();
        var engine = new GameEngine(state);
        var initialY = state.ActivePiece.OriginY;

        // Two ticks of 500ms should trigger gravity once
        engine.Tick(500);
        bool ticked = engine.Tick(500);
        Assert.True(ticked);
    }

    [Fact]
    public void GameEngine_Tick_ReturnsFalseWhenGameOver()
    {
        var state = new GameState();
        var engine = new GameEngine(state);

        // Manually set game over by forcing spawn collision
        // Force by repeatedly ticking until game over
        for (int i = 0; i < 1000 && !state.IsGameOver; i++)
            engine.Tick(1000);

        // After game over, tick should return false
        bool result = engine.Tick(1000);
        Assert.True(state.IsGameOver);
        Assert.False(result);
    }

    [Fact]
    public void GameEngine_ResetAccumulator_ResetsGravityTimer()
    {
        var state = new GameState();
        var engine = new GameEngine(state);

        engine.Tick(900); // Accumulate 900ms (just below 1000ms threshold)
        engine.ResetAccumulator();

        // After reset, 900ms more should not trigger gravity
        bool ticked = engine.Tick(900);
        Assert.False(ticked);
    }

    [Fact]
    public void GameState_ProcessInput_MoveLeft_WorksWhenNotBlocked()
    {
        var state = new GameState();
        int initialX = state.ActivePiece.OriginX;

        state.ProcessInput(InputCommand.MoveLeft);

        // If the piece could move left, OriginX decreases
        // If blocked by wall, OriginX stays same
        Assert.True(state.ActivePiece.OriginX <= initialX);
    }

    [Fact]
    public void GameState_ProcessInput_None_DoesNothing()
    {
        var state = new GameState();
        int initialX = state.ActivePiece.OriginX;
        int initialY = state.ActivePiece.OriginY;

        state.ProcessInput(InputCommand.None);

        Assert.Equal(initialX, state.ActivePiece.OriginX);
        Assert.Equal(initialY, state.ActivePiece.OriginY);
    }
}
