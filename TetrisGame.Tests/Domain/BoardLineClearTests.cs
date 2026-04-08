using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class BoardLineClearTests
{
    [Fact]
    public void ClearFullLines_ClearsOneCompletedRow()
    {
        var board = new Board();
        // Fill rows 18 and 19 with 5 O pieces at cols 0,2,4,6,8 (each O is 2x2)
        for (int c = 0; c < 10; c += 2)
        {
            var piece = new Piece(PieceType.O, 18, c);
            board.Lock(piece);
        }

        int cleared = board.ClearFullLines();
        Assert.Equal(2, cleared); // Both rows 18 and 19 are filled by O pieces
    }

    [Fact]
    public void ClearFullLines_ShiftsRowsDown()
    {
        var board = new Board();
        for (int c = 0; c < 10; c += 2)
        {
            var piece = new Piece(PieceType.O, 18, c);
            board.Lock(piece);
        }
        board.ClearFullLines();

        for (int c = 0; c < Board.Width; c++)
        {
            Assert.False(board.IsOccupied(18, c));
            Assert.False(board.IsOccupied(19, c));
        }
    }

    [Fact]
    public void ClearFullLines_Returns0_WhenNoFullLines()
    {
        var board = new Board();
        int cleared = board.ClearFullLines();
        Assert.Equal(0, cleared);
    }
}

