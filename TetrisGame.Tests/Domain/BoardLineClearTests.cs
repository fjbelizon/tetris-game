using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

/// <summary>
/// Tests for Board.ClearCompleteLines() — verifies single and multi-line clearing
/// behaviour: correct cleared-count return values and proper row-shift semantics.
/// </summary>
public class BoardLineClearTests
{
    // ── helpers ────────────────────────────────────────────────────────────────

    /// <summary>Fills every cell in a row so it qualifies as a complete line.</summary>
    private static void FillRow(Board board, int row)
    {
        for (int col = 0; col < board.Width; col++)
            board.SetSettledCell(row, col, true);
    }

    // ── no clear ───────────────────────────────────────────────────────────────

    [Fact]
    public void ClearCompleteLines_EmptyBoard_ReturnsZero()
    {
        var board = new Board();

        int cleared = board.ClearCompleteLines();

        Assert.Equal(0, cleared);
    }

    [Fact]
    public void ClearCompleteLines_PartialRow_ReturnsZero()
    {
        var board = new Board();
        // Fill all but one cell in the bottom row
        for (int col = 0; col < board.Width - 1; col++)
            board.SetSettledCell(board.Height - 1, col, true);

        int cleared = board.ClearCompleteLines();

        Assert.Equal(0, cleared);
    }

    // ── single line clear ──────────────────────────────────────────────────────

    [Fact]
    public void ClearCompleteLines_OneFullRow_ReturnsOne()
    {
        var board = new Board();
        FillRow(board, board.Height - 1); // bottom row

        int cleared = board.ClearCompleteLines();

        Assert.Equal(1, cleared);
    }

    [Fact]
    public void ClearCompleteLines_OneFullRow_ClearedRowIsEmpty()
    {
        var board = new Board();
        int bottomRow = board.Height - 1;
        FillRow(board, bottomRow);

        board.ClearCompleteLines();

        // After clearing the only complete row the bottom row should now be empty
        for (int col = 0; col < board.Width; col++)
            Assert.False(board.IsSettledAt(bottomRow, col));
    }

    [Fact]
    public void ClearCompleteLines_RowAboveCompleteRow_ShiftsDown()
    {
        var board = new Board();
        int bottomRow = board.Height - 1;
        int markerRow = bottomRow - 1;

        // Place a single marker block in the row above the bottom
        board.SetSettledCell(markerRow, 0, true);
        // Fill the bottom row completely
        FillRow(board, bottomRow);

        board.ClearCompleteLines();

        // The marker block should now occupy the bottom row
        Assert.True(board.IsSettledAt(bottomRow, 0));
        // The original marker row should now be empty
        Assert.False(board.IsSettledAt(markerRow, 0));
    }

    [Fact]
    public void ClearCompleteLines_MiddleFullRow_ShiftsAboveRowsDown()
    {
        var board = new Board();
        int middleRow = board.Height / 2;

        // Mark cell in the row above the middle row
        board.SetSettledCell(middleRow - 1, 3, true);
        // Fill the middle row completely
        FillRow(board, middleRow);

        board.ClearCompleteLines();

        // The block from row middleRow-1 should have shifted to middleRow
        Assert.True(board.IsSettledAt(middleRow, 3));
        Assert.False(board.IsSettledAt(middleRow - 1, 3));
    }

    // ── multi-line clear ───────────────────────────────────────────────────────

    [Fact]
    public void ClearCompleteLines_TwoAdjacentFullRows_ReturnsTwo()
    {
        var board = new Board();
        FillRow(board, board.Height - 1);
        FillRow(board, board.Height - 2);

        int cleared = board.ClearCompleteLines();

        Assert.Equal(2, cleared);
    }

    [Fact]
    public void ClearCompleteLines_ThreeAdjacentFullRows_ReturnsThree()
    {
        var board = new Board();
        FillRow(board, board.Height - 1);
        FillRow(board, board.Height - 2);
        FillRow(board, board.Height - 3);

        int cleared = board.ClearCompleteLines();

        Assert.Equal(3, cleared);
    }

    [Fact]
    public void ClearCompleteLines_FourAdjacentFullRows_ReturnsFour()
    {
        var board = new Board();
        FillRow(board, board.Height - 1);
        FillRow(board, board.Height - 2);
        FillRow(board, board.Height - 3);
        FillRow(board, board.Height - 4);

        int cleared = board.ClearCompleteLines();

        Assert.Equal(4, cleared);
    }

    [Fact]
    public void ClearCompleteLines_TwoNonAdjacentFullRows_ReturnsTwo()
    {
        var board = new Board();
        FillRow(board, board.Height - 1);
        FillRow(board, board.Height - 3); // gap at Height-2

        int cleared = board.ClearCompleteLines();

        Assert.Equal(2, cleared);
    }

    [Fact]
    public void ClearCompleteLines_TwoFullRows_BoardStateCorrectAfterClear()
    {
        var board = new Board();
        int bottom = board.Height - 1;
        int aboveBottom = bottom - 1;
        int markerRow = bottom - 2;

        // Place a marker block two rows above the bottom
        board.SetSettledCell(markerRow, 5, true);
        // Fill the two bottom rows
        FillRow(board, bottom);
        FillRow(board, aboveBottom);

        board.ClearCompleteLines();

        // The marker should have shifted down by 2 rows
        Assert.True(board.IsSettledAt(markerRow + 2, 5));
        // The original marker row should be empty
        Assert.False(board.IsSettledAt(markerRow, 5));
        // The row where the marker moved from should be empty
        Assert.False(board.IsSettledAt(markerRow + 1, 5));
    }

    [Fact]
    public void ClearCompleteLines_FourFullRows_AllClearedRowsBecomeEmpty()
    {
        var board = new Board();
        // Fill the bottom 4 rows
        for (int r = board.Height - 4; r < board.Height; r++)
            FillRow(board, r);

        board.ClearCompleteLines();

        // The bottom 4 rows should now all be empty (no block remains)
        for (int r = board.Height - 4; r < board.Height; r++)
            for (int col = 0; col < board.Width; col++)
                Assert.False(board.IsSettledAt(r, col));
    }

    [Fact]
    public void ClearCompleteLines_NonCompleteRowsBetweenFullRows_ArePreserved()
    {
        var board = new Board();
        int bottom = board.Height - 1;

        // Fill the bottom row
        FillRow(board, bottom);
        // Place a single block in the row between the two full rows
        board.SetSettledCell(bottom - 1, 2, true);
        // Fill the row two above the bottom
        FillRow(board, bottom - 2);

        board.ClearCompleteLines();

        // The two complete rows cleared; the partial row should shift down by 2
        Assert.True(board.IsSettledAt(bottom, 2));
        Assert.False(board.IsSettledAt(bottom - 1, 2));
        Assert.False(board.IsSettledAt(bottom - 2, 2));
    }

    // ── top rows remain empty after clears ────────────────────────────────────

    [Fact]
    public void ClearCompleteLines_AfterSingleClear_TopRowIsAlwaysEmpty()
    {
        var board = new Board();
        FillRow(board, board.Height - 1);

        board.ClearCompleteLines();

        // Top row (row 0) should be empty after the shift
        for (int col = 0; col < board.Width; col++)
            Assert.False(board.IsSettledAt(0, col));
    }
}
