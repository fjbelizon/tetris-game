using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class BoardCollisionTests
{
    private static GameState MakeState() =>
        new(new Board(), new PieceFactory(), new ScoreSystem());

    [Fact]
    public void CanPlace_ReturnsFalse_WhenOutOfBounds()
    {
        var board = new Board();
        var piece = new Piece(PieceType.I, -1, 0);
        Assert.False(board.CanPlace(piece));
    }

    [Fact]
    public void CanPlace_ReturnsFalse_WhenOverlapsSettledCell()
    {
        var board = new Board();
        var piece = new Piece(PieceType.O, 18, 0);
        board.Lock(piece);

        var same = new Piece(PieceType.O, 18, 0);
        Assert.False(board.CanPlace(same));
    }

    [Fact]
    public void CanPlace_ReturnsTrue_ForEmptyBoard()
    {
        var board = new Board();
        var piece = new Piece(PieceType.T, 0, 3);
        Assert.True(board.CanPlace(piece));
    }
}
