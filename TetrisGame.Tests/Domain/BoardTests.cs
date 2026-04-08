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

    [Fact]
    public void IsInside_ReturnsTrueForCornerCells()
    {
        var board = new Board();
        Assert.True(board.IsInside(0, 0));
        Assert.True(board.IsInside(9, 19));
        Assert.True(board.IsInside(0, 19));
        Assert.True(board.IsInside(9, 0));
    }

    [Fact]
    public void IsInside_ReturnsFalseOutsideBounds()
    {
        var board = new Board();
        Assert.False(board.IsInside(-1, 0));
        Assert.False(board.IsInside(0, -1));
        Assert.False(board.IsInside(10, 0));
        Assert.False(board.IsInside(0, 20));
    }

    [Fact]
    public void NewBoard_AllCellsAreEmpty()
    {
        var board = new Board();
        for (int x = 0; x < Board.Width; x++)
            for (int y = 0; y < Board.Height; y++)
                Assert.False(board.IsOccupied(x, y));
    }
}
