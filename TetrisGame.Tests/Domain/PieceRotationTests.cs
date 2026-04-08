using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceRotationTests
{
    [Fact]
    public void OPiece_RotationIsIdempotent()
    {
        var piece = new Piece(TetrominoType.O, 0, 2, 2);
        var r0 = piece.GetAbsoluteCells().OrderBy(c => c).ToArray();

        var rotated = piece.RotateClockwise();
        var r1 = rotated.GetAbsoluteCells().OrderBy(c => c).ToArray();

        Assert.Equal(r0, r1);
    }

    [Fact]
    public void RotateClockwise_IncrementsRotationByOne()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 3);
        var rotated = piece.RotateClockwise();
        Assert.Equal(1, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_WrapsAfter4Rotations()
    {
        var piece = new Piece(TetrominoType.T, 3, 3, 3);
        var rotated = piece.RotateClockwise();
        Assert.Equal(0, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_PreservesOrigin()
    {
        var piece = new Piece(TetrominoType.L, 0, 4, 5);
        var rotated = piece.RotateClockwise();
        Assert.Equal(4, rotated.OriginX);
        Assert.Equal(5, rotated.OriginY);
    }

    [Theory]
    [InlineData(TetrominoType.I)]
    [InlineData(TetrominoType.T)]
    [InlineData(TetrominoType.S)]
    [InlineData(TetrominoType.Z)]
    [InlineData(TetrominoType.J)]
    [InlineData(TetrominoType.L)]
    public void AllPieceTypes_HaveDistinctCellsInAllRotations(TetrominoType type)
    {
        for (int r = 0; r < 4; r++)
        {
            var piece = new Piece(type, r, 5, 5);
            var cells = piece.GetAbsoluteCells();
            var unique = cells.Distinct().Count();
            Assert.Equal(4, unique);
        }
    }

    [Fact]
    public void IPiece_Rotation0_IsHorizontal()
    {
        var piece = new Piece(TetrominoType.I, 0, 0, 0);
        var cells = piece.GetAbsoluteCells();
        // All cells should have the same y (horizontal)
        var yValues = cells.Select(c => c.y).Distinct().ToArray();
        Assert.Single(yValues);
    }

    [Fact]
    public void IPiece_Rotation1_IsVertical()
    {
        var piece = new Piece(TetrominoType.I, 1, 0, 0);
        var cells = piece.GetAbsoluteCells();
        // All cells should have the same x (vertical)
        var xValues = cells.Select(c => c.x).Distinct().ToArray();
        Assert.Single(xValues);
    }

    [Fact]
    public void Piece_RotateClockwise_DoesNotModifyOriginal()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 3);
        _ = piece.RotateClockwise();
        Assert.Equal(0, piece.Rotation);
    }
}
