using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardTests
{
    [Fact]
    public void Board_HasTenColumns()
    {
        Assert.Equal(10, Board.Columns);
    }

    [Fact]
    public void Board_HasTwentyRows()
    {
        Assert.Equal(20, Board.Rows);
    }

    [Fact]
    public void NewBoard_AllCellsAreEmpty()
    {
        var board = new Board();

        for (int row = 0; row < Board.Rows; row++)
            for (int col = 0; col < Board.Columns; col++)
                Assert.False(board.IsOccupied(row, col));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 9)]
    [InlineData(19, 0)]
    [InlineData(19, 9)]
    [InlineData(10, 5)]
    public void IsInBounds_ReturnsTrueForValidCoordinates(int row, int col)
    {
        Assert.True(Board.IsInBounds(row, col));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(20, 0)]
    [InlineData(0, 10)]
    [InlineData(-1, -1)]
    [InlineData(20, 10)]
    public void IsInBounds_ReturnsFalseForOutOfBoundsCoordinates(int row, int col)
    {
        Assert.False(Board.IsInBounds(row, col));
    }

    [Fact]
    public void SetOccupied_MarksCell()
    {
        var board = new Board();
        board.SetOccupied(5, 3);
        Assert.True(board.IsOccupied(5, 3));
    }

    [Fact]
    public void SetOccupied_DoesNotAffectOtherCells()
    {
        var board = new Board();
        board.SetOccupied(5, 3);
        Assert.False(board.IsOccupied(5, 4));
        Assert.False(board.IsOccupied(6, 3));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(20, 0)]
    [InlineData(0, 10)]
    public void SetOccupied_ThrowsForOutOfBoundsCoordinates(int row, int col)
    {
        var board = new Board();
        Assert.Throws<ArgumentOutOfRangeException>(() => board.SetOccupied(row, col));
    }

    [Fact]
    public void ClearCell_UnmarksOccupiedCell()
    {
        var board = new Board();
        board.SetOccupied(2, 7);
        board.ClearCell(2, 7);
        Assert.False(board.IsOccupied(2, 7));
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(20, 0)]
    [InlineData(0, 10)]
    public void ClearCell_ThrowsForOutOfBoundsCoordinates(int row, int col)
    {
        var board = new Board();
        Assert.Throws<ArgumentOutOfRangeException>(() => board.ClearCell(row, col));
    }
}
