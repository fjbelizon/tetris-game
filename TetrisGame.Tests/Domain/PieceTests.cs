using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceTests
{
    [Fact]
    public void Piece_HasCorrectType()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 0);
        Assert.Equal(TetrominoType.T, piece.Type);
    }

    [Fact]
    public void Piece_HasCorrectInitialRotation()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 0);
        Assert.Equal(0, piece.Rotation);
    }

    [Fact]
    public void Piece_HasCorrectOrigin()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 5);
        Assert.Equal(3, piece.OriginX);
        Assert.Equal(5, piece.OriginY);
    }

    [Fact]
    public void Move_UpdatesOriginCorrectly()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 0);
        var moved = piece.Move(2, 3);
        Assert.Equal(5, moved.OriginX);
        Assert.Equal(3, moved.OriginY);
    }

    [Fact]
    public void Move_DoesNotMutateOriginalPiece()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 0);
        piece.Move(1, 1);
        Assert.Equal(3, piece.OriginX);
        Assert.Equal(0, piece.OriginY);
    }

    [Fact]
    public void GetAbsoluteCells_ReflectsOriginOffset()
    {
        var piece = new Piece(TetrominoType.O, 0, 0, 0);
        var cells = piece.GetAbsoluteCells();
        Assert.Contains((0, 0), cells);
        Assert.Contains((1, 0), cells);
        Assert.Contains((0, 1), cells);
        Assert.Contains((1, 1), cells);
    }

    [Fact]
    public void GetAbsoluteCells_MovedPiece_ReflectsNewOrigin()
    {
        var piece = new Piece(TetrominoType.O, 0, 2, 3);
        var cells = piece.GetAbsoluteCells();
        Assert.Contains((2, 3), cells);
        Assert.Contains((3, 3), cells);
        Assert.Contains((2, 4), cells);
        Assert.Contains((3, 4), cells);
    }
}
