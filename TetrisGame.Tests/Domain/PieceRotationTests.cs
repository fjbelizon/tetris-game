using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class PieceRotationTests
{
    [Theory]
    [InlineData(PieceType.T)]
    [InlineData(PieceType.I)]
    [InlineData(PieceType.S)]
    [InlineData(PieceType.Z)]
    [InlineData(PieceType.J)]
    [InlineData(PieceType.L)]
    public void Rotation_CyclesThrough4States(PieceType type)
    {
        var p = new Piece(type);
        var r0Cells = p.GetCells().OrderBy(c => c).ToList();

        var p4 = p.Rotated().Rotated().Rotated().Rotated();
        var r4Cells = p4.GetCells().OrderBy(c => c).ToList();

        Assert.Equal(r0Cells, r4Cells);
    }

    [Fact]
    public void OPiece_SameShapeAllRotations()
    {
        var p = new Piece(PieceType.O);
        var r0 = p.GetCells().OrderBy(c => c).ToList();
        var r1 = p.Rotated().GetCells().OrderBy(c => c).ToList();
        Assert.Equal(r0, r1);
    }
}
