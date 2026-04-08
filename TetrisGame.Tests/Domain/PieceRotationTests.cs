using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceRotationTests
{
    [Theory]
    [InlineData(TetrominoType.I)]
    [InlineData(TetrominoType.T)]
    [InlineData(TetrominoType.S)]
    [InlineData(TetrominoType.Z)]
    [InlineData(TetrominoType.J)]
    [InlineData(TetrominoType.L)]
    public void RotateClockwise_AlwaysCyclesThroughFourStates(TetrominoType type)
    {
        var piece = new Piece(type, 0, 0, 0);
        var rotated1 = piece.RotateClockwise();
        var rotated2 = rotated1.RotateClockwise();
        var rotated3 = rotated2.RotateClockwise();
        var rotated4 = rotated3.RotateClockwise();
        Assert.Equal(0, rotated4.Rotation);
    }

    [Fact]
    public void OPiece_RotationIsIdempotent()
    {
        var piece = new Piece(TetrominoType.O, 0, 0, 0);
        var r0 = piece.GetAbsoluteCells();
        var r1 = piece.RotateClockwise().GetAbsoluteCells();
        Assert.Equal(r0.OrderBy(c => c.x).ThenBy(c => c.y).ToArray(),
                     r1.OrderBy(c => c.x).ThenBy(c => c.y).ToArray());
    }
}
