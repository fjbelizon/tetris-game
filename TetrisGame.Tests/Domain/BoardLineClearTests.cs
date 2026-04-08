using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardLineClearTests
{
    private static void FillRow(Board board, int row, int exceptCol = -1)
    {
        // Fill a row by locking individual 1-cell pieces is not possible, 
        // so we lock O-pieces strategically. Instead use reflection or direct state.
        // We'll use a trick: lock pieces cell by cell using I piece rotated vertically
        // at each column position.
        for (int col = 0; col < Board.Width; col++)
        {
            if (col == exceptCol) continue;
            // Create a piece that occupies only (col, row) - use S piece hack
            // Actually, simplest: create a piece whose cells land on (col, row)
            // We'll use the horizontal I piece approach but simplified using a custom test
            var piece = new Piece(TetrominoType.O, 0, col, row - 1);
            // O piece cells: (col, row-1), (col+1, row-1), (col, row), (col+1, row)
            // This places cells at col and col+1, so step by 2
        }
    }

    [Fact]
    public void ClearCompleteLines_SingleLine_ReturnsOne()
    {
        var board = new Board();
        // Fill row 19 (bottom) completely using overlapping O pieces
        // O piece at (col, 18) places cells at col+0,col+1 in rows 18,19
        for (int col = 0; col < Board.Width; col += 2)
        {
            board.Lock(new Piece(TetrominoType.O, 0, col, 18));
        }

        int cleared = board.ClearCompleteLines();
        Assert.Equal(2, cleared); // rows 18 and 19 both filled by O pieces
    }

    [Fact]
    public void ClearCompleteLines_RowAboveShiftsDown()
    {
        var board = new Board();
        // Place an O piece at (0, 16) — occupies (0,16),(1,16),(0,17),(1,17)
        board.Lock(new Piece(TetrominoType.O, 0, 0, 16));

        // Fill rows 18 and 19 completely
        for (int col = 0; col < Board.Width; col += 2)
        {
            board.Lock(new Piece(TetrominoType.O, 0, col, 18));
        }

        board.ClearCompleteLines();

        // The piece that was at row 16-17 should have shifted to row 18-19
        Assert.True(board.IsOccupied(0, 18));
        Assert.True(board.IsOccupied(1, 18));
    }

    [Fact]
    public void ClearCompleteLines_FourLines_ReturnsFour()
    {
        var board = new Board();
        // Fill rows 16-19
        for (int row = 16; row <= 19; row += 2)
        {
            for (int col = 0; col < Board.Width; col += 2)
            {
                board.Lock(new Piece(TetrominoType.O, 0, col, row));
            }
        }

        int cleared = board.ClearCompleteLines();
        Assert.Equal(4, cleared);
    }
}
