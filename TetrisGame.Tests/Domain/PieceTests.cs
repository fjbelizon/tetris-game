using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceTests
{
    [Fact]
    public void Piece_HasCorrectType()
    {
        var piece = new Piece(TetrominoType.T, 0, 4, 1);
        Assert.Equal(TetrominoType.T, piece.Type);
    }

    [Fact]
    public void Piece_HasCorrectRotation()
    {
        var piece = new Piece(TetrominoType.T, 2, 4, 1);
        Assert.Equal(2, piece.Rotation);
    }

    [Fact]
    public void Piece_RotationWrapsModulo4()
    {
        var piece = new Piece(TetrominoType.T, 4, 4, 1);
        Assert.Equal(0, piece.Rotation);
    }

    [Fact]
    public void Piece_HasCorrectOrigin()
    {
        var piece = new Piece(TetrominoType.T, 0, 4, 1);
        Assert.Equal(4, piece.OriginX);
        Assert.Equal(1, piece.OriginY);
    }

    [Fact]
    public void Piece_Move_ReturnsNewPieceWithOffset()
    {
        var piece = new Piece(TetrominoType.O, 0, 4, 1);
        var moved = piece.Move(1, 2);
        Assert.Equal(5, moved.OriginX);
        Assert.Equal(3, moved.OriginY);
    }

    [Fact]
    public void Piece_GetAbsoluteCells_ReturnsFourCells()
    {
        var piece = new Piece(TetrominoType.T, 0, 5, 5);
        var cells = piece.GetAbsoluteCells().ToList();
        Assert.Equal(4, cells.Count);
    }
}
