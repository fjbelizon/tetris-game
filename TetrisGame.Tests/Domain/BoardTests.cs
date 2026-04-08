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

    [Fact]
    public void IsInside_ReturnsTrueForValidCoords()
    {
        var board = new Board();
        Assert.True(board.IsInside(0, 0));
        Assert.True(board.IsInside(9, 19));
        Assert.True(board.IsInside(5, 10));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(10, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 20)]
    public void IsInside_ReturnsFalseForOutOfBounds(int x, int y)
    {
        var board = new Board();
        Assert.False(board.IsInside(x, y));
    }

    [Fact]
    public void NewBoard_HasNoOccupiedCells()
    {
        var board = new Board();
        for (int y = 0; y < Board.Height; y++)
            for (int x = 0; x < Board.Width; x++)
                Assert.False(board.GetSettled(x, y));
    }
}
