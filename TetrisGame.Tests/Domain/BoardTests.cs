using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardTests
{
    [Fact]
    public void Board_HasCorrectDimensions()
    {
        Assert.Equal(10, Board.Width);
        Assert.Equal(20, Board.Height);
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(9, 19, true)]
    [InlineData(-1, 0, false)]
    [InlineData(10, 0, false)]
    [InlineData(0, 20, false)]
    public void IsInside_ChecksBounds(int x, int y, bool expected)
    {
        var board = new Board();
        Assert.Equal(expected, board.IsInside(x, y));
    }

    [Fact]
    public void Lock_MarksSettledCells()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 0, 0);
        board.Lock(piece);
        Assert.True(board.IsOccupied(0, 0));
        Assert.True(board.IsOccupied(1, 0));
        Assert.True(board.IsOccupied(0, 1));
        Assert.True(board.IsOccupied(1, 1));
    }
}
