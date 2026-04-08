using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardLineClearTests
{
    [Fact]
    public void ClearCompleteLines_ClearsOneFullRow()
    {
        var board = new Board();
        // Lock O pieces at y=19 (bottom row only; y=20 is out of bounds and ignored by Lock)
        for (int col = 0; col <= 8; col += 2)
            board.Lock(new Piece(TetrominoType.O, 0, col, 19));
        int cleared = board.ClearCompleteLines();
        Assert.Equal(1, cleared);
        // Row 19 should now be empty after the clear
        for (int col = 0; col < Board.Width; col++)
            Assert.False(board.GetSettled(col, 19));
    }

    [Fact]
    public void ClearCompleteLines_ClearsTwoFullRows()
    {
        var board = new Board();
        // Fill rows 18 and 19 completely
        for (int col = 0; col <= 8; col += 2)
            board.Lock(new Piece(TetrominoType.O, 0, col, 18)); // fills rows 18 and 19
        int cleared = board.ClearCompleteLines();
        Assert.Equal(2, cleared);
    }

    [Fact]
    public void ClearCompleteLines_ShiftsRowsDownAfterClear()
    {
        var board = new Board();
        // Put a partially-filled row at y=18 (cols 0-3 via I piece)
        board.Lock(new Piece(TetrominoType.I, 0, 0, 18)); // fills row 18 cols 0-3
        // Fill row 19 completely using O pieces anchored at y=19
        for (int col = 0; col <= 8; col += 2)
            board.Lock(new Piece(TetrominoType.O, 0, col, 19));
        board.ClearCompleteLines();
        // The partial row at 18 should have shifted down to 19
        Assert.True(board.GetSettled(0, 19));
    }
}
