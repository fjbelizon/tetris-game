using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

/// <summary>
/// Tests for game-over detection and replay reset flow.
/// Covers US4 acceptance scenarios: spawn-collision detection, game-over state,
/// and full reset behavior (board, score, level, bag).
/// </summary>
public sealed class GameStateGameOverTests
{
    // -----------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------

    /// <summary>
    /// Settles a vertical I-piece at column 4, rows 0-3 on the board.
    /// All 7 tetromino types spawn at origin (4, 0) with rotation 0, and every
    /// rotation-0 shape includes cell (4, 0), so this settled column guarantees
    /// the next spawned piece will collide without forming any complete rows
    /// (which would be cleared and remove the obstruction).
    /// </summary>
    /// <remarks>
    /// I-piece rotation 1 offsets: (1,-1),(1,0),(1,1),(1,2).
    /// With origin (3, 1): cells locked at (4,0),(4,1),(4,2),(4,3).
    /// </remarks>
    private static void OccupySpawnZone(Board board)
    {
        var verticalI = new Piece(TetrominoType.I, 1, 3, 1);
        board.Lock(verticalI);
    }

    // -----------------------------------------------------------------------
    // US4-1: Spawn-collision triggers game over
    // -----------------------------------------------------------------------

    [Fact]
    public void LockAndSpawnNext_WhenSpawnCellsOccupied_SetsIsGameOverTrue()
    {
        var state = new GameState();

        // Fill spawn rows so the next spawn will collide.
        OccupySpawnZone(state.Board);

        // Move active piece somewhere safe away from spawn zone before locking.
        // Drop it to the bottom so locking it doesn't fill the spawn area.
        while (state.TryMoveDown()) { }

        state.LockAndSpawnNext();

        Assert.True(state.IsGameOver);
    }

    [Fact]
    public void LockAndSpawnNext_WhenSpawnCellsAreFree_DoesNotSetGameOver()
    {
        var state = new GameState();

        // Drop the active piece to the bottom without filling spawn rows.
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();

        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void IsGameOver_IsFalseOnInitialState()
    {
        var state = new GameState();
        Assert.False(state.IsGameOver);
    }

    // -----------------------------------------------------------------------
    // US4-1: Controls are disabled once game is over
    // -----------------------------------------------------------------------

    [Fact]
    public void TryMoveLeft_WhenGameOver_ReturnsFalse()
    {
        var state = new GameState();
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext(); // triggers game over

        bool moved = state.TryMoveLeft();

        Assert.False(moved);
    }

    [Fact]
    public void TryMoveRight_WhenGameOver_ReturnsFalse()
    {
        var state = new GameState();
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();

        bool moved = state.TryMoveRight();

        Assert.False(moved);
    }

    [Fact]
    public void TryMoveDown_WhenGameOver_ReturnsFalse()
    {
        var state = new GameState();
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();

        bool moved = state.TryMoveDown();

        Assert.False(moved);
    }

    [Fact]
    public void TryRotateClockwise_WhenGameOver_ReturnsFalse()
    {
        var state = new GameState();
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();

        bool rotated = state.TryRotateClockwise();

        Assert.False(rotated);
    }

    [Fact]
    public void TickGravity_WhenGameOver_DoesNothing()
    {
        var state = new GameState();
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext(); // game over
        var pieceBeforeTick = state.ActivePiece;

        state.TickGravity();

        // Active piece position must not change.
        Assert.Equal(pieceBeforeTick.OriginX, state.ActivePiece.OriginX);
        Assert.Equal(pieceBeforeTick.OriginY, state.ActivePiece.OriginY);
        Assert.True(state.IsGameOver);
    }

    // -----------------------------------------------------------------------
    // US4-3: Reset clears game-over and restores initial state
    // -----------------------------------------------------------------------

    [Fact]
    public void Reset_AfterGameOver_ClearsIsGameOverFlag()
    {
        var state = new GameState();
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();
        Assert.True(state.IsGameOver); // precondition

        state.Reset();

        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void Reset_ResetsScoreToZero()
    {
        var state = new GameState();
        // Drop piece and lock to accumulate state.
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();

        state.Reset();

        Assert.Equal(0, state.Score.Score);
    }

    [Fact]
    public void Reset_ResetsLevelToOne()
    {
        var state = new GameState();
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();

        state.Reset();

        Assert.Equal(1, state.Score.Level);
    }

    [Fact]
    public void Reset_ClearsBoardSettledBlocks()
    {
        var state = new GameState();
        // Fill several board cells then reset.
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();

        state.Reset();

        // After reset no settled cells should remain in the top rows.
        for (int y = 0; y < Board.Height; y++)
            for (int x = 0; x < Board.Width; x++)
                Assert.False(state.Board.GetCell(x, y),
                    $"Cell ({x},{y}) should be empty after reset.");
    }

    [Fact]
    public void Reset_AllowsNewPiecesToSpawnSuccessfully()
    {
        var state = new GameState();
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext(); // triggers game over

        state.Reset();

        // After reset the active piece must be placeable on the cleared board.
        Assert.True(state.Board.CanPlace(state.ActivePiece));
    }

    [Fact]
    public void Reset_ActivePieceIsPlaceableOnEmptyBoard()
    {
        var state = new GameState();
        // Simulate multiple locks to advance state, then reset.
        for (int i = 0; i < 3 && !state.IsGameOver; i++)
        {
            while (state.TryMoveDown()) { }
            state.LockAndSpawnNext();
        }

        state.Reset();

        Assert.True(state.Board.CanPlace(state.ActivePiece));
    }

    // -----------------------------------------------------------------------
    // US4-1: LockAndSpawnNext is idempotent once game-over is set
    // -----------------------------------------------------------------------

    [Fact]
    public void LockAndSpawnNext_WhenAlreadyGameOver_DoesNotChangePiece()
    {
        var state = new GameState();
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext(); // first game over
        Assert.True(state.IsGameOver);
        var pieceAfterGameOver = state.ActivePiece;

        // Calling again should be a no-op.
        state.LockAndSpawnNext();

        Assert.Equal(pieceAfterGameOver.Type, state.ActivePiece.Type);
        Assert.Equal(pieceAfterGameOver.OriginX, state.ActivePiece.OriginX);
        Assert.Equal(pieceAfterGameOver.OriginY, state.ActivePiece.OriginY);
    }

    // -----------------------------------------------------------------------
    // US4-3: Full replay cycle — game over then reset then playable again
    // -----------------------------------------------------------------------

    [Fact]
    public void FullReplayCycle_GameOverThenResetThenDropPiece_Succeeds()
    {
        var state = new GameState();

        // Cause game over.
        OccupySpawnZone(state.Board);
        while (state.TryMoveDown()) { }
        state.LockAndSpawnNext();
        Assert.True(state.IsGameOver);

        // Replay.
        state.Reset();
        Assert.False(state.IsGameOver);

        // New game: dropping the piece should work.
        bool dropped = state.TryMoveDown();
        Assert.True(dropped);
    }
}
