using TetrisGame.Domain;
using Xunit;

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
    public void IsInBounds_ReturnsTrueForValidCell()
    {
        var board = new Board();
        Assert.True(board.IsInBounds(0, 0));
        Assert.True(board.IsInBounds(19, 9));
    }

    [Fact]
    public void IsInBounds_ReturnsFalseForOutOfBoundsCell()
    {
        var board = new Board();
        Assert.False(board.IsInBounds(-1, 0));
        Assert.False(board.IsInBounds(20, 0));
        Assert.False(board.IsInBounds(0, -1));
        Assert.False(board.IsInBounds(0, 10));
    }

    [Fact]
    public void IsOccupied_ReturnsFalseForEmptyCell()
    {
        var board = new Board();
        Assert.False(board.IsOccupied(0, 0));
    }
}
