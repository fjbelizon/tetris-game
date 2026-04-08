using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardTests
{
    [Fact]
    public void Board_HasWidth10()
    {
        Assert.Equal(10, Board.Width);
    }

    [Fact]
    public void Board_HasHeight20()
    {
        Assert.Equal(20, Board.Height);
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(9, 19, true)]
    [InlineData(5, 10, true)]
    [InlineData(-1, 0, false)]
    [InlineData(0, -1, false)]
    [InlineData(10, 0, false)]
    [InlineData(0, 20, false)]
    [InlineData(10, 20, false)]
    public void IsInside_ReturnsCorrectResult(int x, int y, bool expected)
    {
        var board = new Board();
        Assert.Equal(expected, board.IsInside(x, y));
    }

    [Fact]
    public void NewBoard_HasNoOccupiedCells()
    {
        var board = new Board();
        for (int x = 0; x < Board.Width; x++)
            for (int y = 0; y < Board.Height; y++)
                Assert.False(board.IsOccupied(x, y));
    }

    [Fact]
    public void IsOccupied_OutOfBounds_ReturnsFalse()
    {
        var board = new Board();
        Assert.False(board.IsOccupied(-1, 0));
        Assert.False(board.IsOccupied(0, -1));
        Assert.False(board.IsOccupied(10, 0));
        Assert.False(board.IsOccupied(0, 20));
    }
}
