using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

/// <summary>
/// T017 — Line clear tests: single and multi-line clearing, row shift verification.
/// </summary>
public class BoardLineClearTests
{
    /// <summary>
    /// Fills row <paramref name="row"/> completely using three I-pieces.
    /// Cols 0-3 and 4-7 are filled exactly; cols 8-9 are covered by a third I-piece
    /// whose cols 10-11 are out-of-bounds (Lock skips them safely).
    /// </summary>
    private static void FillRow(Board board, int row)
    {
        board.Lock(new Piece(TetrominoType.I, 0, 0, row));  // cols 0-3
        board.Lock(new Piece(TetrominoType.I, 0, 4, row));  // cols 4-7
        board.Lock(new Piece(TetrominoType.I, 0, 8, row));  // cols 8-9 (10-11 skipped by bounds check)
    }

    // -----------------------------------------------------------
    // Test 1: No complete rows — ClearCompleteLines returns 0
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_EmptyBoard_ReturnsZero()
    {
        var board = new Board();
        int cleared = board.ClearCompleteLines();
        Assert.Equal(0, cleared);
    }

    // -----------------------------------------------------------
    // Test 2: One complete row — returns 1
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_OneCompleteRow_ReturnsOne()
    {
        var board = new Board();
        FillRow(board, 19);

        int cleared = board.ClearCompleteLines();

        Assert.Equal(1, cleared);
    }

    // -----------------------------------------------------------
    // Test 3: One complete row is removed; upper rows shift down
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_OneRow_RemovedAndAboveRowShiftsDown()
    {
        var board = new Board();
        FillRow(board, 19);

        // Marker at row 18, cols 0-3 (partial row — won't be cleared)
        board.Lock(new Piece(TetrominoType.I, 0, 0, 18));

        board.ClearCompleteLines();

        // Former row 18 should now be at row 19
        Assert.True(board.GetSettled(0, 19), "Col 0 of former row 18 should now be at row 19");
        Assert.True(board.GetSettled(1, 19), "Col 1 of former row 18 should now be at row 19");
        // Row 18 should be clear (the marker moved down)
        Assert.False(board.GetSettled(0, 18), "Row 18 should be empty after row 19 was cleared");
    }

    // -----------------------------------------------------------
    // Test 4: Two simultaneous complete rows cleared — returns 2
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_TwoCompleteRows_ReturnsTwo()
    {
        var board = new Board();
        FillRow(board, 18);
        FillRow(board, 19);

        int cleared = board.ClearCompleteLines();

        Assert.Equal(2, cleared);
    }

    // -----------------------------------------------------------
    // Test 5: Two rows cleared; rows above shift down by 2
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_TwoRows_AboveRowsShiftDownByTwo()
    {
        var board = new Board();
        FillRow(board, 18);
        FillRow(board, 19);

        // Marker at row 17, cols 0-3
        board.Lock(new Piece(TetrominoType.I, 0, 0, 17));

        board.ClearCompleteLines();

        // Former row 17 should now be at row 19 (shifted down by 2)
        Assert.True(board.GetSettled(0, 19), "Col 0 of former row 17 should be at row 19");
        Assert.True(board.GetSettled(3, 19), "Col 3 of former row 17 should be at row 19");
        Assert.False(board.GetSettled(0, 17), "Row 17 should be empty after shift");
        Assert.False(board.GetSettled(0, 18), "Row 18 should be empty after shift");
    }

    // -----------------------------------------------------------
    // Test 6: Four simultaneous complete rows (Tetris) — returns 4
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_FourCompleteRows_ReturnsFour()
    {
        var board = new Board();
        FillRow(board, 16);
        FillRow(board, 17);
        FillRow(board, 18);
        FillRow(board, 19);

        int cleared = board.ClearCompleteLines();

        Assert.Equal(4, cleared);
    }

    // -----------------------------------------------------------
    // Test 7: Partial row (9 of 10 cells) — NOT cleared
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_PartialRow_NotCleared()
    {
        var board = new Board();
        // Fill cols 0-8 of row 19 only (col 9 remains empty)
        board.Lock(new Piece(TetrominoType.I, 0, 0, 19));  // cols 0-3
        board.Lock(new Piece(TetrominoType.I, 0, 4, 19));  // cols 4-7
        board.Lock(new Piece(TetrominoType.I, 0, 5, 19));  // cols 5-8 (redundant but col 9 still empty)

        int cleared = board.ClearCompleteLines();

        Assert.Equal(0, cleared);
        Assert.True(board.GetSettled(0, 19), "Row 19 blocks should still be there");
    }

    // -----------------------------------------------------------
    // Test 8: Non-contiguous complete rows are each cleared
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_NonContiguousCompleteRows_BothCleared()
    {
        var board = new Board();
        FillRow(board, 17);
        FillRow(board, 19);
        // Row 18 is partial (not cleared)
        board.Lock(new Piece(TetrominoType.I, 0, 0, 18));  // cols 0-3 only

        int cleared = board.ClearCompleteLines();

        Assert.Equal(2, cleared);
    }

    // -----------------------------------------------------------
    // Test 9: All rows filled — board becomes completely empty
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_AllRowsFilled_BoardBecomesEmpty()
    {
        var board = new Board();
        for (int row = 0; row < Board.Height; row++)
            FillRow(board, row);

        int cleared = board.ClearCompleteLines();

        Assert.Equal(Board.Height, cleared);

        for (int row = 0; row < Board.Height; row++)
            for (int col = 0; col < Board.Width; col++)
                Assert.False(board.GetSettled(col, row), $"Cell ({col},{row}) should be empty");
    }

    // -----------------------------------------------------------
    // Test 10: Top row is empty after clear and shift
    // -----------------------------------------------------------
    [Fact]
    public void ClearCompleteLines_TopRowIsEmptyAfterShift()
    {
        var board = new Board();
        FillRow(board, 19);

        board.ClearCompleteLines();

        for (int col = 0; col < Board.Width; col++)
            Assert.False(board.GetSettled(col, 0), $"Top row col {col} should be empty after shift");
    }
}
