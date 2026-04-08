using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardCollisionTests
{
    [Fact]
    public void CanPlace_ReturnsFalse_WhenPieceOutOfBounds_Left()
    {
        var board = new Board();
        // O piece at x=-1: cells at (-1,0),(0,0),(-1,1),(0,1) → -1 out of bounds
        var piece = new Piece(TetrominoType.O, 0, -1, 0);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalse_WhenPieceOutOfBounds_Right()
    {
        var board = new Board();
        // O piece at x=9: cells at (9,0),(10,0) → 10 out of bounds
        var piece = new Piece(TetrominoType.O, 0, 9, 0);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalse_WhenPieceOutOfBounds_Bottom()
    {
        var board = new Board();
        // O piece at y=19: cells include y=20 → out of bounds
        var piece = new Piece(TetrominoType.O, 0, 4, 19);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsTrue_WhenPositionIsValid()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 4, 4);
        Assert.True(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalse_WhenOverlappingSettledBlock()
    {
        var board = new Board();
        // Lock an O piece at (4,4)
        var locked = new Piece(TetrominoType.O, 0, 4, 4);
        board.Lock(locked);

        // Try to place same piece again
        Assert.False(board.CanPlace(locked));
    }
}
