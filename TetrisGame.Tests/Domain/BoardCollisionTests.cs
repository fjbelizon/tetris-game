using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardCollisionTests
{
    [Fact]
    public void CanPlace_EmptyBoardTopLeft_ReturnsTrue()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 0, 0);
        Assert.True(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_PieceOutOfBoundsLeft_ReturnsFalse()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, -1, 0);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_PieceOutOfBoundsRight_ReturnsFalse()
    {
        var board = new Board();
        // O piece at x=9 would occupy columns 9 and 10 (10 is out of bounds)
        var piece = new Piece(TetrominoType.O, 0, 9, 0);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_PieceOutOfBoundsBottom_ReturnsFalse()
    {
        var board = new Board();
        // O piece at y=19 would occupy rows 19 and 20 (20 is out of bounds)
        var piece = new Piece(TetrominoType.O, 0, 0, 19);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_PieceOverlapsSettledBlock_ReturnsFalse()
    {
        var board = new Board();
        // Lock an O piece at (0,0)
        var piece1 = new Piece(TetrominoType.O, 0, 0, 0);
        board.Lock(piece1);

        // Try to place another piece at the same position
        var piece2 = new Piece(TetrominoType.O, 0, 0, 0);
        Assert.False(board.CanPlace(piece2));
    }

    [Fact]
    public void CanPlace_PieceAdjacentToSettledBlock_ReturnsTrue()
    {
        var board = new Board();
        var piece1 = new Piece(TetrominoType.O, 0, 0, 0);
        board.Lock(piece1);

        // Adjacent piece to the right (no overlap)
        var piece2 = new Piece(TetrominoType.O, 0, 2, 0);
        Assert.True(board.CanPlace(piece2));
    }

    [Fact]
    public void CanPlace_IPieceAtRightEdge_ReturnsTrue()
    {
        var board = new Board();
        // I piece horizontal (0,0)-(3,0); at x=6 it goes 6,7,8,9 (within width 10)
        var piece = new Piece(TetrominoType.I, 0, 6, 0);
        Assert.True(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_IPieceExceedsRightEdge_ReturnsFalse()
    {
        var board = new Board();
        // I piece horizontal at x=7 would occupy 7,8,9,10 (10 is out of bounds)
        var piece = new Piece(TetrominoType.I, 0, 7, 0);
        Assert.False(board.CanPlace(piece));
    }
}
