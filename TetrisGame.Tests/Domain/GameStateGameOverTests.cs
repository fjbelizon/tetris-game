namespace TetrisGame.Tests.Domain;

using TetrisGame.Domain;

/// <summary>
/// T035 + T039: Tests for game-over detection (spawn collision) and full reset behavior.
/// </summary>
public class GameStateGameOverTests
{
    // ── T036: spawn-collision game-over transition ──────────────────────────

    [Fact]
    public void LockAndSpawnNext_WhenSpawnIsBlocked_SetsIsGameOver()
    {
        // Arrange: fill the top two rows so any newly spawned piece overlaps them
        var state = new GameState();
        FillTopRows(state.Board, rows: 2);

        // Act: locking the current active piece triggers spawning the next piece,
        // which cannot be placed → game over
        state.LockAndSpawnNext();

        // Assert
        Assert.True(state.IsGameOver);
    }

    [Fact]
    public void LockAndSpawnNext_WhenSpawnIsOpen_DoesNotSetGameOver()
    {
        // Arrange: board is empty — spawn always succeeds
        var state = new GameState();

        // Act: move active piece to the bottom and lock
        MoveToBottom(state);
        state.LockAndSpawnNext();

        // Assert
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void TryMoveLeft_WhenGameOver_DoesNotChangeActivePiece()
    {
        var state = new GameState();
        FillTopRows(state.Board, rows: 2);
        state.LockAndSpawnNext(); // trigger game over
        var pieceBefore = state.ActivePiece;

        state.TryMoveLeft();

        Assert.Equal(pieceBefore.OriginX, state.ActivePiece.OriginX);
    }

    [Fact]
    public void TickGravity_WhenGameOver_DoesNothing()
    {
        var state = new GameState();
        FillTopRows(state.Board, rows: 2);
        state.LockAndSpawnNext(); // trigger game over

        // Should not throw or change state
        state.TickGravity();

        Assert.True(state.IsGameOver);
    }

    // ── T039: full reset behavior ───────────────────────────────────────────

    [Fact]
    public void Reset_ClearsIsGameOverFlag()
    {
        // Arrange: get into game-over state
        var state = new GameState();
        FillTopRows(state.Board, rows: 2);
        state.LockAndSpawnNext();
        Assert.True(state.IsGameOver);

        // Act
        state.Reset();

        // Assert
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void Reset_ClearsAllBoardCells()
    {
        // Arrange: lock some pieces to fill cells
        var state = new GameState();
        MoveToBottom(state);
        state.LockAndSpawnNext();

        // Act
        state.Reset();

        // Assert: no settled cell on the entire board
        for (int y = 0; y < Board.Height; y++)
            for (int x = 0; x < Board.Width; x++)
                Assert.False(state.Board.GetCell(x, y), $"Cell ({x},{y}) should be empty after Reset");
    }

    [Fact]
    public void Reset_ResetsScoreToZero()
    {
        // Arrange: accumulate some score by clearing lines
        var state = new GameState();
        FillRowExceptOne(state.Board, row: Board.Height - 1);
        MoveToBottom(state);
        state.LockAndSpawnNext(); // triggers line clear

        // Act
        state.Reset();

        // Assert
        Assert.Equal(0, state.ScoreSystem.Score);
    }

    [Fact]
    public void Reset_ResetsLevelToOne()
    {
        // Arrange: simulate many lines to advance level
        var state = new GameState();
        // Directly apply line clears to advance level
        state.ScoreSystem.ApplyLineClear(1); // +100 pts, 1 line
        Assert.Equal(1, state.ScoreSystem.Level); // still level 1

        // Act
        state.Reset();

        // Assert
        Assert.Equal(1, state.ScoreSystem.Level);
    }

    [Fact]
    public void Reset_ResetsLevelFromHighLevel()
    {
        // Arrange: clear 20 lines to reach level 3
        var state = new GameState();
        state.ScoreSystem.ApplyLineClear(4); // 4 lines
        state.ScoreSystem.ApplyLineClear(4); // 8 lines
        state.ScoreSystem.ApplyLineClear(4); // 12 lines — level 2
        state.ScoreSystem.ApplyLineClear(4); // 16 lines
        state.ScoreSystem.ApplyLineClear(4); // 20 lines — level 3
        Assert.True(state.ScoreSystem.Level >= 3);

        // Act
        state.Reset();

        // Assert
        Assert.Equal(1, state.ScoreSystem.Level);
        Assert.Equal(0, state.ScoreSystem.TotalLines);
    }

    [Fact]
    public void Reset_SpawnsFreshActivePiece()
    {
        // Arrange: fill board and trigger game over
        var state = new GameState();
        FillTopRows(state.Board, rows: 2);
        state.LockAndSpawnNext();
        Assert.True(state.IsGameOver);

        // Act
        state.Reset();

        // Assert: active piece can be placed on the now-empty board
        Assert.True(state.Board.CanPlace(state.ActivePiece));
    }

    [Fact]
    public void Reset_SpawnsFreshNextPiece()
    {
        var state = new GameState();
        FillTopRows(state.Board, rows: 2);
        state.LockAndSpawnNext();

        state.Reset();

        // Next piece should be a valid piece that can be placed on an empty board
        Assert.True(state.Board.CanPlace(state.NextPiece));
    }

    [Fact]
    public void Reset_AllowsGameplayToContinueAfterGameOver()
    {
        // Full lifecycle: play → game over → reset → play again
        var state = new GameState();
        FillTopRows(state.Board, rows: 2);
        state.LockAndSpawnNext();
        Assert.True(state.IsGameOver);

        state.Reset();

        // Should be able to tick gravity without errors
        state.TickGravity();
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void Reset_TotalLinesIsZero()
    {
        var state = new GameState();
        state.ScoreSystem.ApplyLineClear(4);
        state.ScoreSystem.ApplyLineClear(4); // 8 lines

        state.Reset();

        Assert.Equal(0, state.ScoreSystem.TotalLines);
    }

    [Fact]
    public void Reset_RefreshesThePieceBag()
    {
        // After reset the new bag produces valid pieces for a full 7-bag cycle
        var state = new GameState();
        state.Reset();

        // Lock many pieces and ensure no crash (bag keeps refilling)
        for (int i = 0; i < 14; i++)
        {
            MoveToBottom(state);
            if (!state.IsGameOver)
                state.LockAndSpawnNext();
            if (state.IsGameOver)
            {
                state.Reset();
            }
        }
        // Just verifying no exception is thrown and game is in a valid state
        Assert.NotNull(state.ActivePiece);
        Assert.NotNull(state.NextPiece);
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Fills columns 1–4 in the top <paramref name="rows"/> rows by locking an I-piece per row.
    /// This is sufficient to block the spawn position of any tetromino type
    /// (all pieces spawn with an origin at column 4, row 1, which falls within the filled columns).
    /// </summary>
    private static void FillTopRows(Board board, int rows)
    {
        for (int y = 0; y < rows; y++)
            board.Lock(new Piece(TetrominoType.I, 0, 2, y)); // cols 1,2,3,4 at row y
    }

    /// <summary>Fills a row leaving one gap at column 0 so it doesn't clear on lock.</summary>
    private static void FillRowExceptOne(Board board, int row)
    {
        board.Lock(new Piece(TetrominoType.I, 0, 2, row)); // cols 1,2,3,4
        board.Lock(new Piece(TetrominoType.I, 0, 6, row)); // cols 5,6,7,8
        // col 0 and col 9 remain empty — row won't clear
    }

    /// <summary>Moves the active piece to the bottom of the board.</summary>
    private static void MoveToBottom(GameState state)
    {
        for (int i = 0; i < Board.Height + 4; i++)
        {
            if (!state.TryMoveDown())
                break;
        }
    }
}
