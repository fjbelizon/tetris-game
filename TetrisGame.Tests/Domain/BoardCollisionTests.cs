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
        var piece = new Piece(TetrominoType.O, 0, 9, 0); // O spans x=9,10
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalseWhenOverlapsSettledBlock()
    {
        var board = new Board();
        // Lock an O piece at (4,18) to fill the bottom
        board.Lock(new Piece(TetrominoType.O, 0, 4, 18));
        // Attempt to place another O at same position
        Assert.False(board.CanPlace(new Piece(TetrominoType.O, 0, 4, 18)));
    }
}
