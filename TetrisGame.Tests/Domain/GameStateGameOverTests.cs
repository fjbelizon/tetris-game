using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class GameStateGameOverTests
{
    /// <summary>
    /// Forces a game-over condition by stacking pieces until the spawn area is blocked.
    /// </summary>
    [Fact]
    public void IsGameOver_FalseAtStart()
    {
        var state = new GameState();
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void LockAndSpawnNext_SetsGameOver_WhenSpawnBlocked()
    {
        // Use a fixed-seed factory so pieces are deterministic
        var factory = new PieceFactory(new Random(0));
        var state = new GameState(factory);

        // Fill the board almost to the top to force a spawn collision.
        // Lock pieces in the bottom rows until spawning a new piece fails.
        // We'll simulate this by calling LockAndSpawnNext until game over.
        int maxIterations = 200;
        for (int i = 0; i < maxIterations && !state.IsGameOver; i++)
            state.LockAndSpawnNext();

        Assert.True(state.IsGameOver);
    }

    [Fact]
    public void Reset_ClearsGameOverFlag()
    {
        var factory = new PieceFactory(new Random(0));
        var state = new GameState(factory);

        // Force game over
        for (int i = 0; i < 200 && !state.IsGameOver; i++)
            state.LockAndSpawnNext();

        Assert.True(state.IsGameOver);

        state.Reset();
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void Reset_ZerosScoreAndLevel()
    {
        var factory = new PieceFactory(new Random(0));
        var state = new GameState(factory);
        state.Reset();
        Assert.Equal(0, state.Score.Score);
        Assert.Equal(1, state.Score.Level);
    }

    [Fact]
    public void Reset_ClearsBoard()
    {
        var state = new GameState();
        // Lock some pieces manually
        state.LockAndSpawnNext();
        state.Reset();
        // Board should be empty
        for (int y = 0; y < Board.Height; y++)
            for (int x = 0; x < Board.Width; x++)
                Assert.False(state.Board.GetSettled(x, y));
    }

    [Fact]
    public void Controls_AreIgnoredAfterGameOver()
    {
        var factory = new PieceFactory(new Random(0));
        var state = new GameState(factory);
        for (int i = 0; i < 200 && !state.IsGameOver; i++)
            state.LockAndSpawnNext();

        // All movements must be rejected when game is over
        Assert.False(state.TryMoveLeft());
        Assert.False(state.TryMoveRight());
        Assert.False(state.TryMoveDown());
        Assert.False(state.TryRotateClockwise());
    }
}
