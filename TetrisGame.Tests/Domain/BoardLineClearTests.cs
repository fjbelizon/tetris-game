using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardLineClearTests
{
    [Fact]
    public void ClearCompleteLines_NoCompleteRows_Returns0()
    {
        var board = new Board();
        // Lock just a partial row (cols 0-7 only)
        board.Lock(new Piece(TetrominoType.I, 0, 0, 19)); // cols 0-3
        board.Lock(new Piece(TetrominoType.I, 0, 4, 19)); // cols 4-7
        // Cols 8-9 are empty -> row 19 incomplete

        int cleared = board.ClearCompleteLines();
        Assert.Equal(0, cleared);
    }

    [Fact]
    public void ClearCompleteLines_FullRow19_ClearsAndReturns1()
    {
        var board = new Board();
        // Fill row 19 completely: cols 0-3 via I, cols 4-7 via I, cols 8-9 via O (also fills row 18)
        board.Lock(new Piece(TetrominoType.I, 0, 0, 19));  // cols 0-3, row 19
        board.Lock(new Piece(TetrominoType.I, 0, 4, 19));  // cols 4-7, row 19
        board.Lock(new Piece(TetrominoType.O, 0, 8, 18));  // (8,18),(9,18),(8,19),(9,19)

        int cleared = board.ClearCompleteLines();
        Assert.Equal(1, cleared);
        // Row 19 should now be empty (was cleared), and row 18 content shifted down
        Assert.False(board.IsOccupied(0, 19));
    }

    [Fact]
    public void ClearCompleteLines_TwoFullRows_Returns2()
    {
        var board = new Board();
        // Fill rows 18 and 19
        board.Lock(new Piece(TetrominoType.I, 0, 0, 18));
        board.Lock(new Piece(TetrominoType.I, 0, 4, 18));
        board.Lock(new Piece(TetrominoType.O, 0, 8, 17)); // covers (8,17),(9,17),(8,18),(9,18)

        board.Lock(new Piece(TetrominoType.I, 0, 0, 19));
        board.Lock(new Piece(TetrominoType.I, 0, 4, 19));
        board.Lock(new Piece(TetrominoType.O, 0, 8, 19)); // covers (8,19),(9,19) - row 20 out of bounds

        int cleared = board.ClearCompleteLines();
        Assert.Equal(2, cleared);
    }

    [Fact]
    public void ClearCompleteLines_RowsAboveShiftDown()
    {
        var board = new Board();
        // Place a single block in row 18 col 0 ONLY (not a full row)
        // Use O piece at (0,18) but we only want cell (0,18) to be set
        // Since O covers (0,18),(1,18),(0,19),(1,19), let's lock an I-vertical
        // I piece rotation=1 at (0,18): covers (0,18),(0,19),(0,20),(0,21) - only (0,18),(0,19) are in bounds

        // Instead, lock an I horizontal piece only in row 18 col 0:
        // We need a single-cell marker; closest: use I-vertical at (0,17) covering rows 17,18,19,20
        // Only rows 17,18,19 are valid (20 is out of bounds), so we get (0,17),(0,18),(0,19) occupied

        // Better approach: fill row 18 col 0 via O piece (which also fills col 1 and rows 18,19)
        // but then fill the rest of row 19 to make it complete, and check after clear

        // Setup: lock O at (0,17) - covers (0,17),(1,17),(0,18),(1,18)
        board.Lock(new Piece(TetrominoType.O, 0, 0, 17));

        // Fill row 19 completely
        board.Lock(new Piece(TetrominoType.I, 0, 0, 19)); // cols 0-3
        board.Lock(new Piece(TetrominoType.I, 0, 4, 19)); // cols 4-7
        board.Lock(new Piece(TetrominoType.O, 0, 8, 18)); // (8,18),(9,18),(8,19),(9,19)

        // Before clear: col 0 row 18 is occupied (from O piece at (0,17))
        Assert.True(board.IsOccupied(0, 18));

        int cleared = board.ClearCompleteLines();
        Assert.Equal(1, cleared);

        // After clearing row 19, row 18 shifts down to become row 19
        // So row 19 col 0 should now be occupied (shifted from row 18)
        Assert.True(board.IsOccupied(0, 19));
        // Row 18 should now have what was in row 17 (col 0 from O piece at (0,17))
        Assert.True(board.IsOccupied(0, 18));
    }

    [Fact]
    public void ClearCompleteLines_IncompleteRow_NotCleared()
    {
        var board = new Board();
        // Fill row 19 with only 8 cells (missing cols 8,9)
        board.Lock(new Piece(TetrominoType.I, 0, 0, 19));
        board.Lock(new Piece(TetrominoType.I, 0, 4, 19));

        int cleared = board.ClearCompleteLines();
        Assert.Equal(0, cleared);
    }
}
