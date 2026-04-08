using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardTests
{
    [Fact]
    public void Board_Width_Is10()
    {
        Assert.Equal(10, Board.Width);
    }

    [Fact]
    public void Board_Height_Is20()
    {
        Assert.Equal(20, Board.Height);
    }

    [Theory]
    [InlineData(0, 0, true)]
    [InlineData(9, 19, true)]
    [InlineData(-1, 0, false)]
    [InlineData(10, 0, false)]
    [InlineData(0, -1, false)]
    [InlineData(0, 20, false)]
    [InlineData(9, 20, false)]
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

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(10, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 20)]
    public void IsOccupied_OutOfBounds_ReturnsFalse(int x, int y)
    {
        var board = new Board();
        Assert.False(board.IsOccupied(x, y));
    }
}
