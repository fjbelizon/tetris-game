using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceTests
{
    [Fact]
    public void Piece_HasCorrectTypeAndRotation()
    {
        var piece = new Piece(TetrominoType.T, 0, 4, 0);
        Assert.Equal(TetrominoType.T, piece.Type);
        Assert.Equal(0, piece.Rotation);
        Assert.Equal(4, piece.OriginX);
        Assert.Equal(0, piece.OriginY);
    }

    [Fact]
    public void Move_ShiftsOriginCorrectly()
    {
        var piece = new Piece(TetrominoType.O, 0, 4, 0);
        var moved = piece.Move(2, 3);
        Assert.Equal(6, moved.OriginX);
        Assert.Equal(3, moved.OriginY);
    }

    [Fact]
    public void RotateClockwise_IncrementsRotationModulo4()
    {
        var piece = new Piece(TetrominoType.T, 3, 4, 0);
        var rotated = piece.RotateClockwise();
        Assert.Equal(0, rotated.Rotation);
    }

    [Fact]
    public void GetAbsoluteCells_OPiece_ReturnsCorrectCells()
    {
        var piece = new Piece(TetrominoType.O, 0, 4, 0);
        var cells = piece.GetAbsoluteCells();
        Assert.Contains((4, 0), cells);
        Assert.Contains((5, 0), cells);
        Assert.Contains((4, 1), cells);
        Assert.Contains((5, 1), cells);
    }
}
