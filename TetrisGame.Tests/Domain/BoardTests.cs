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
    [InlineData(0, -1, false)]
    [InlineData(0, 20, false)]
    public void IsInside_ReturnsCorrectResult(int x, int y, bool expected)
    {
        var board = new Board();
        Assert.Equal(expected, board.IsInside(x, y));
    }

    [Fact]
    public void NewBoard_IsNotOccupied()
    {
        var board = new Board();
        for (int y = 0; y < Board.Height; y++)
            for (int x = 0; x < Board.Width; x++)
                Assert.False(board.IsOccupied(x, y));
    }
}
