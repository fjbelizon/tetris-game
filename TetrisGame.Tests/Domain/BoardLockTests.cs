using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardLockTests
{
    [Fact]
    public void Lock_SetsCellsAsOccupied()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 4, 4);
        board.Lock(piece);

        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            Assert.True(board.IsOccupied(x, y));
        }
    }

    [Fact]
    public void AfterLock_CannotPlacePieceAtSamePosition()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 4, 4);
        board.Lock(piece);

        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalse_WhenDownwardMoveWouldHitSettled()
    {
        var board = new Board();
        // Lock piece at bottom
        var bottom = new Piece(TetrominoType.O, 0, 4, 18);
        board.Lock(bottom);

        // Piece one row above should not be able to move down
        var above = new Piece(TetrominoType.O, 0, 4, 17);
        var movedDown = above.Move(0, 1);
        Assert.False(board.CanPlace(movedDown));
    }
}
