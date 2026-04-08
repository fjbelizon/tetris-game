using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

/// <summary>
/// T035 – Game-over and replay flow tests.
///
/// Covers the spawn-collision game-over transition (T036) and associated
/// state invariants: controls disabled after game over, reset returning to
/// a playable state (T039 path).
/// </summary>
public sealed class GameStateGameOverTests
{
    // -----------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------

    /// <summary>
    /// Fills columns 3, 4 and 5 of row 0 so that every possible spawn piece
    /// (all 7 tetromino types at origin R0) has at least one cell blocked.
    /// These three cells are insufficient to form a complete row (10 needed),
    /// so they survive ClearCompleteLines().
    /// </summary>
    private static void BlockSpawnRow(Board board)
    {
        board.ForceSettle(3, 0);
        board.ForceSettle(4, 0);
        board.ForceSettle(5, 0);
    }

    // -----------------------------------------------------------------------
    // Initial state
    // -----------------------------------------------------------------------

    [Fact]
    public void IsGameOver_IsFalse_OnNewGame()
    {
        var state = new GameState(new PieceFactory());
        Assert.False(state.IsGameOver);
    }

    // -----------------------------------------------------------------------
    // T036 – Spawn-collision game-over transition
    // -----------------------------------------------------------------------

    [Fact]
    public void LockAndSpawnNext_SetsIsGameOver_WhenSpawnPositionIsBlocked()
    {
        // Arrange – pre-settle cells that block every possible spawn.
        var state = new GameState(new PieceFactory(new Random(0)));
        BlockSpawnRow(state.Board);

        // Act – lock the current active piece; the next piece will fail CanPlace.
        state.LockAndSpawnNext();

        // Assert
        Assert.True(state.IsGameOver);
    }

    [Fact]
    public void LockAndSpawnNext_DoesNotSetIsGameOver_WhenSpawnPositionIsFree()
    {
        // Arrange – drop the active piece to the bottom so the spawn area
        // (rows 0–1) remains free for the next piece to arrive.
        var state = new GameState(new PieceFactory(new Random(0)));
        while (state.TryMoveDown()) { }

        // Act
        state.LockAndSpawnNext();

        // Assert
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void LockAndSpawnNext_SetsIsGameOver_RegardlessOfPieceType()
    {
        // Verify game-over detection works across multiple seeded bags.
        // Each seed produces a different first piece type; all should still
        // trigger game over when spawn row 0 is blocked.
        for (int seed = 0; seed < 20; seed++)
        {
            var state = new GameState(new PieceFactory(new Random(seed)));
            BlockSpawnRow(state.Board);
            state.LockAndSpawnNext();
            Assert.True(state.IsGameOver,
                $"Expected game over with seed {seed}");
        }
    }

    // -----------------------------------------------------------------------
    // Controls disabled after game over
    // -----------------------------------------------------------------------

    [Fact]
    public void TryMoveLeft_ReturnsFalse_AfterGameOver()
    {
        var state = BuildGameOverState();
        Assert.False(state.TryMoveLeft());
    }

    [Fact]
    public void TryMoveRight_ReturnsFalse_AfterGameOver()
    {
        var state = BuildGameOverState();
        Assert.False(state.TryMoveRight());
    }

    [Fact]
    public void TryMoveDown_ReturnsFalse_AfterGameOver()
    {
        var state = BuildGameOverState();
        Assert.False(state.TryMoveDown());
    }

    [Fact]
    public void TryRotateClockwise_ReturnsFalse_AfterGameOver()
    {
        var state = BuildGameOverState();
        Assert.False(state.TryRotateClockwise());
    }

    [Fact]
    public void TickGravity_IsNoOp_AfterGameOver()
    {
        var state = BuildGameOverState();
        var piece = state.ActivePiece;
        state.TickGravity();     // should not throw or change state
        Assert.True(state.IsGameOver);
        Assert.Equal(piece.OriginX, state.ActivePiece.OriginX);
        Assert.Equal(piece.OriginY, state.ActivePiece.OriginY);
    }

    // -----------------------------------------------------------------------
    // Reset (T039 – replay path)
    // -----------------------------------------------------------------------

    [Fact]
    public void Reset_ClearsGameOverFlag()
    {
        var state = BuildGameOverState();
        state.Reset();
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void Reset_ClearsScore()
    {
        var state = BuildGameOverState();
        state.Reset();
        Assert.Equal(0, state.Score.Score);
        Assert.Equal(0, state.Score.TotalLines);
        Assert.Equal(1, state.Score.Level);
    }

    [Fact]
    public void Reset_ClearsBoard()
    {
        var state = BuildGameOverState();
        state.Reset();

        // After reset the board should have no occupied cells.
        for (int x = 0; x < Board.Width; x++)
        for (int y = 0; y < Board.Height; y++)
            Assert.False(state.Board.IsOccupied(x, y),
                $"Expected cell ({x},{y}) to be empty after Reset");
    }

    [Fact]
    public void Reset_AllowsInputAfterGameOver()
    {
        var state = BuildGameOverState();
        state.Reset();

        // At least MoveLeft or MoveRight should be actionable on an empty board.
        bool moved = state.TryMoveLeft() || state.TryMoveRight();
        Assert.True(moved, "Expected at least one move to succeed after reset");
    }

    // -----------------------------------------------------------------------
    // Private factory
    // -----------------------------------------------------------------------

    private static GameState BuildGameOverState()
    {
        var state = new GameState(new PieceFactory(new Random(42)));
        BlockSpawnRow(state.Board);
        state.LockAndSpawnNext();
        Assert.True(state.IsGameOver);   // guard: confirm precondition
        return state;
    }
}
