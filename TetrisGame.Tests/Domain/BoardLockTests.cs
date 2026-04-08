using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class BoardLockTests
{
    [Fact]
    public void Lock_MarksAllCellsOccupied()
    {
        var board = new Board();
        var piece = new Piece(PieceType.O, 18, 0);
        board.Lock(piece);

        foreach (var (r, c) in piece.GetCells())
            Assert.True(board.IsOccupied(r, c));
    }
}
