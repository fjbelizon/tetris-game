using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceRotationTests
{
    [Fact]
    public void RotateClockwise_IncrementsRotationByOne()
    {
        var piece = new Piece(TetrominoType.T, 0, 5, 5);
        var rotated = piece.RotateClockwise();
        Assert.Equal(1, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_WrapsAt4()
    {
        var piece = new Piece(TetrominoType.T, 3, 5, 5);
        var rotated = piece.RotateClockwise();
        Assert.Equal(0, rotated.Rotation);
    }

    [Fact]
    public void OPiece_RotationIsIdempotent()
    {
        var piece = new Piece(TetrominoType.O, 0, 5, 5);
        var cells0 = piece.GetAbsoluteCells().OrderBy(c => c.x).ThenBy(c => c.y).ToList();
        var cells1 = piece.RotateClockwise().GetAbsoluteCells().OrderBy(c => c.x).ThenBy(c => c.y).ToList();
        Assert.Equal(cells0, cells1);
    }

    [Theory]
    [InlineData(TetrominoType.I)]
    [InlineData(TetrominoType.T)]
    [InlineData(TetrominoType.S)]
    [InlineData(TetrominoType.Z)]
    [InlineData(TetrominoType.J)]
    [InlineData(TetrominoType.L)]
    public void AllPieceTypes_FourRotations_EachHasFourCells(TetrominoType type)
    {
        for (int r = 0; r < 4; r++)
        {
            var piece = new Piece(type, r, 5, 5);
            var cells = piece.GetAbsoluteCells().ToList();
            Assert.Equal(4, cells.Count);
        }
    }
}
