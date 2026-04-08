using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardCollisionTests
{
    [Fact]
    public void CanPlace_ReturnsTrueForEmptyBoard()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 4, 0);
        Assert.True(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalseWhenOutOfBoundsLeft()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, -1, 0);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalseWhenOutOfBoundsRight()
    {
        var board = new Board();
        // O piece at x=9 extends to x=10 which is out of bounds
        var piece = new Piece(TetrominoType.O, 0, 9, 0);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalseWhenOutOfBoundsBottom()
    {
        var board = new Board();
        // O piece at y=19 extends to y=20 which is out of bounds
        var piece = new Piece(TetrominoType.O, 0, 0, 19);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalseWhenOverlappingSettledCell()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 0, 0);
        board.Lock(piece);  // settle an O block at (0,0)

        var newPiece = new Piece(TetrominoType.I, 0, 0, 0);
        Assert.False(board.CanPlace(newPiece));
    }

    [Fact]
    public void CanPlace_ReturnsTrueWhenAdjacentToSettledCell()
    {
        var board = new Board();
        var settled = new Piece(TetrominoType.O, 0, 0, 18);
        board.Lock(settled);

        var newPiece = new Piece(TetrominoType.O, 0, 0, 16);
        Assert.True(board.CanPlace(newPiece));
    }
}
