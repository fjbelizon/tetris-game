using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardLockTests
{
    [Fact]
    public void Lock_SettlesCellsOnBoard()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 4, 18);
        board.Lock(piece);
        Assert.True(board.GetSettled(4, 18));
        Assert.True(board.GetSettled(5, 18));
        Assert.True(board.GetSettled(4, 19));
        Assert.True(board.GetSettled(5, 19));
    }

    [Fact]
    public void Lock_DoesNotAffectOtherCells()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 4, 18);
        board.Lock(piece);
        Assert.False(board.GetSettled(3, 18));
        Assert.False(board.GetSettled(6, 18));
    }
}
