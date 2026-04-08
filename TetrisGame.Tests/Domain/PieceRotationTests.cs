using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceRotationTests
{
    [Fact]
    public void RotateClockwise_IncreasesRotationBy1()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 0);
        var rotated = piece.RotateClockwise();
        Assert.Equal(1, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_WrapsAt4()
    {
        var piece = new Piece(TetrominoType.T, 3, 3, 0);
        var rotated = piece.RotateClockwise();
        Assert.Equal(0, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_DoesNotMutateOriginalPiece()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 0);
        piece.RotateClockwise();
        Assert.Equal(0, piece.Rotation);
    }

    [Fact]
    public void OPiece_RotationIsIdempotent()
    {
        var piece = new Piece(TetrominoType.O, 0, 0, 0);
        var r1 = piece.RotateClockwise().GetAbsoluteCells();
        var r0 = piece.GetAbsoluteCells();
        Assert.Equal(r0.OrderBy(c => c.X).ThenBy(c => c.Y),
                     r1.OrderBy(c => c.X).ThenBy(c => c.Y));
    }

    [Fact]
    public void IPiece_Rotation0_IsHorizontal()
    {
        var piece = new Piece(TetrominoType.I, 0, 0, 0);
        var cells = piece.GetAbsoluteCells();
        // All cells on the same row
        Assert.True(cells.All(c => c.Y == cells[0].Y));
        Assert.Equal(4, cells.Count);
    }

    [Fact]
    public void IPiece_Rotation1_IsVertical()
    {
        var piece = new Piece(TetrominoType.I, 1, 0, 0);
        var cells = piece.GetAbsoluteCells();
        // All cells on the same column
        Assert.True(cells.All(c => c.X == cells[0].X));
        Assert.Equal(4, cells.Count);
    }

    [Theory]
    [InlineData(TetrominoType.T)]
    [InlineData(TetrominoType.S)]
    [InlineData(TetrominoType.Z)]
    [InlineData(TetrominoType.J)]
    [InlineData(TetrominoType.L)]
    [InlineData(TetrominoType.I)]
    [InlineData(TetrominoType.O)]
    public void AllTypes_4Rotations_EachHas4Cells(TetrominoType type)
    {
        for (int rot = 0; rot < 4; rot++)
        {
            var piece = new Piece(type, rot, 0, 0);
            Assert.Equal(4, piece.GetAbsoluteCells().Count);
        }
    }
}
