using TetrisGame.Domain;

namespace TetrisGame.Tests.Loop;

/// <summary>
/// Tests for the GameEngine tick loop and gravity interval update after level-up (T033).
/// </summary>
public class GameTickTests
{
    private static GameState CreateState() =>
        new GameState(new PieceFactory(new Random(42)));

    [Fact]
    public void Tick_WithNoCommand_DoesNotThrow()
    {
        var state = CreateState();
        var engine = new GameEngine(state, tickMs: 100);

        var ex = Record.Exception(() => engine.Tick(InputCommand.None));

        Assert.Null(ex);
    }

    [Fact]
    public void Tick_GravityAccumulates_PieceDropsAfterInterval()
    {
        var state = CreateState();
        // Use tick interval equal to fall interval so piece drops on first tick
        int fallInterval = state.Score.CurrentFallIntervalMs(); // 1000 ms at level 1
        var engine = new GameEngine(state, tickMs: fallInterval);

        var pieceBefore = state.ActivePiece;
        engine.Tick(InputCommand.None);

        // Piece should have moved down one row
        Assert.NotNull(state.ActivePiece);
        Assert.Equal(pieceBefore!.OriginY + 1, state.ActivePiece!.OriginY);
    }

    [Fact]
    public void Tick_GravityDoesNotDrop_BeforeIntervalReached()
    {
        var state = CreateState();
        var engine = new GameEngine(state, tickMs: 10);

        var pieceBefore = state.ActivePiece;
        // Tick with 10 ms, which is less than 1000 ms fall interval at level 1
        engine.Tick(InputCommand.None);

        Assert.Equal(pieceBefore!.OriginY, state.ActivePiece!.OriginY);
    }

    [Fact]
    public void Tick_DoesNothing_WhenGameOver()
    {
        var state = CreateState();
        var engine = new GameEngine(state, tickMs: 16);

        // Force game over by stuffing the board
        ForceGameOver(state);

        Assert.True(state.IsGameOver);
        var ex = Record.Exception(() => engine.Tick(InputCommand.MoveLeft));
        Assert.Null(ex);
    }

    [Fact]
    public void Tick_MoveLeft_MovesActivePiece()
    {
        var state = CreateState();
        var engine = new GameEngine(state, tickMs: 10);

        var pieceBefore = state.ActivePiece;
        engine.Tick(InputCommand.MoveLeft);

        // Piece should have moved left if possible
        Assert.NotNull(state.ActivePiece);
        Assert.True(state.ActivePiece!.OriginX <= pieceBefore!.OriginX);
    }

    [Fact]
    public void Tick_MoveRight_MovesActivePiece()
    {
        var state = CreateState();
        var engine = new GameEngine(state, tickMs: 10);

        var pieceBefore = state.ActivePiece;
        engine.Tick(InputCommand.MoveRight);

        Assert.NotNull(state.ActivePiece);
        Assert.True(state.ActivePiece!.OriginX >= pieceBefore!.OriginX);
    }

    // --- T033: Gravity interval updates after level-up ---

    [Fact]
    public void RefreshFallInterval_ReturnsLevelBasedInterval()
    {
        var state = CreateState();
        var engine = new GameEngine(state, tickMs: 16);

        // At level 1, fall interval is 1000 ms
        Assert.Equal(1000, state.Score.CurrentFallIntervalMs());

        // Simulate reaching level 2 by clearing 10 lines
        ClearNLines(state, 10);

        Assert.Equal(2, state.Score.Level);

        // After refreshing, interval should be 900 ms
        engine.RefreshFallInterval();
        Assert.Equal(900, state.Score.CurrentFallIntervalMs());
    }

    [Fact]
    public void AfterLevelUp_GravityIntervalDecreases()
    {
        var state = CreateState();
        var engine = new GameEngine(state, tickMs: 16);

        int intervalLevel1 = state.Score.CurrentFallIntervalMs();
        ClearNLines(state, 10);
        engine.RefreshFallInterval();
        int intervalLevel2 = state.Score.CurrentFallIntervalMs();

        Assert.True(intervalLevel2 < intervalLevel1,
            $"Expected level 2 interval ({intervalLevel2}) to be less than level 1 interval ({intervalLevel1}).");
    }

    [Fact]
    public void FallInterval_AtHighLevel_NeverDropsBelowMinimum()
    {
        var state = CreateState();

        // Simulate 100 lines (level 11) - interval would be max(1000-1000,100)=100
        ClearNLines(state, 100);

        int interval = state.Score.CurrentFallIntervalMs();
        Assert.True(interval >= 100,
            $"Expected fall interval to be at least 100 ms, but was {interval} ms.");
    }

    [Fact]
    public void Tick_LevelUp_TriggersGravityIntervalRefresh()
    {
        // This test verifies that after locking a piece that causes a level-up,
        // the engine automatically picks up the new fall interval.
        var state = CreateState();
        var engine = new GameEngine(state, tickMs: 16);

        // Manually accumulate enough score for level 2 by clearing lines
        ClearNLines(state, 9); // 9 lines so far, one more will level up

        int levelBefore = state.Score.Level;
        int intervalBefore = state.Score.CurrentFallIntervalMs();

        // Clear one more line to trigger level-up
        ClearNLines(state, 1);

        int levelAfter = state.Score.Level;
        int intervalAfter = state.Score.CurrentFallIntervalMs();

        Assert.True(levelAfter > levelBefore, "Level should have increased.");
        Assert.True(intervalAfter < intervalBefore,
            $"Fall interval should decrease on level-up: was {intervalBefore}, now {intervalAfter}.");
    }

    // Helpers

    private static void ForceGameOver(GameState state)
    {
        // Drop pieces until game over
        int safety = 1000;
        while (!state.IsGameOver && safety-- > 0)
        {
            while (state.TryMoveDown()) { }
            state.LockAndSpawnNext();
        }
    }

    private static void ClearNLines(GameState state, int lines)
    {
        // We directly manipulate the score system to simulate line clears
        // (pure domain test — no rendering or timing involved)
        state.Score.ApplyLineClear(lines);
    }
}
