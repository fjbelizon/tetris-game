using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceTests
{
    [Theory]
    [InlineData(TetrominoType.I)]
    [InlineData(TetrominoType.O)]
    [InlineData(TetrominoType.T)]
    [InlineData(TetrominoType.S)]
    [InlineData(TetrominoType.Z)]
    [InlineData(TetrominoType.J)]
    [InlineData(TetrominoType.L)]
    public void Piece_PreservesType(TetrominoType type)
    {
        var piece = new Piece(type, 0, 0, 0);
        Assert.Equal(type, piece.Type);
    }

    [Fact]
    public void Piece_InitialRotation_IsZero()
    {
        var piece = new Piece(TetrominoType.T, 0, 5, 5);
        Assert.Equal(0, piece.Rotation);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 0)]  // wraps
    [InlineData(5, 1)]
    public void Piece_RotationWrapsModulo4(int inputRotation, int expectedRotation)
    {
        var piece = new Piece(TetrominoType.T, inputRotation, 0, 0);
        Assert.Equal(expectedRotation, piece.Rotation);
    }

    [Fact]
    public void Piece_PreservesOrigin()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 7);
        Assert.Equal(3, piece.OriginX);
        Assert.Equal(7, piece.OriginY);
    }

    [Fact]
    public void Move_ShiftsOrigin()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 5);
        var moved = piece.Move(2, -1);
        Assert.Equal(5, moved.OriginX);
        Assert.Equal(4, moved.OriginY);
    }

    [Fact]
    public void Move_DoesNotMutateOriginal()
    {
        var piece = new Piece(TetrominoType.T, 0, 3, 5);
        _ = piece.Move(2, -1);
        Assert.Equal(3, piece.OriginX);
        Assert.Equal(5, piece.OriginY);
    }

    [Fact]
    public void GetAbsoluteCells_ReturnsCorrectCount()
    {
        var piece = new Piece(TetrominoType.T, 0, 5, 5);
        var cells = piece.GetAbsoluteCells();
        Assert.Equal(4, cells.Length);
    }

    [Fact]
    public void GetAbsoluteCells_AppliesOriginOffset()
    {
        var piece = new Piece(TetrominoType.O, 0, 3, 4);
        var cells = piece.GetAbsoluteCells();
        foreach (var (x, y) in cells)
        {
            Assert.True(x >= 3 && x <= 4, $"x={x} out of expected range [3,4]");
            Assert.True(y >= 4 && y <= 5, $"y={y} out of expected range [4,5]");
        }
    }

    [Fact]
    public void RotateClockwise_IncrementsRotationBy1()
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
    public void O_Piece_RotationIsIdempotent()
    {
        var piece = new Piece(TetrominoType.O, 0, 4, 4);
        for (int i = 0; i < 4; i++)
        {
            var rotated = new Piece(TetrominoType.O, i, 4, 4);
            var cells = rotated.GetAbsoluteCells();
            var original = piece.GetAbsoluteCells();
            Assert.Equal(
                original.OrderBy(c => c).ToArray(),
                cells.OrderBy(c => c).ToArray());
        }
    }
}
