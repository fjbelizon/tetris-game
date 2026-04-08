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

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 0)] // wraps to 0
    public void Piece_Rotation_WrapsModulo4(int rotation, int expected)
    {
        var piece = new Piece(TetrominoType.T, rotation, 0, 0);
        Assert.Equal(expected, piece.Rotation);
    }

    [Fact]
    public void Piece_OriginIsSet()
    {
        var piece = new Piece(TetrominoType.I, 0, 3, 5);
        Assert.Equal(3, piece.OriginX);
        Assert.Equal(5, piece.OriginY);
    }

    [Fact]
    public void Piece_Move_ReturnsNewPieceWithUpdatedOrigin()
    {
        var piece = new Piece(TetrominoType.O, 0, 3, 3);
        var moved = piece.Move(1, 2);
        Assert.Equal(4, moved.OriginX);
        Assert.Equal(5, moved.OriginY);
        // Original unchanged
        Assert.Equal(3, piece.OriginX);
        Assert.Equal(3, piece.OriginY);
    }

    [Fact]
    public void Piece_Move_PreservesTypeAndRotation()
    {
        var piece = new Piece(TetrominoType.J, 1, 2, 3);
        var moved = piece.Move(-1, 1);
        Assert.Equal(TetrominoType.J, moved.Type);
        Assert.Equal(1, moved.Rotation);
    }

    [Fact]
    public void Piece_GetAbsoluteCells_Returns4Cells()
    {
        foreach (TetrominoType type in Enum.GetValues<TetrominoType>())
        {
            var piece = new Piece(type, 0, 0, 0);
            Assert.Equal(4, piece.GetAbsoluteCells().Length);
        }
    }

    [Fact]
    public void Piece_GetAbsoluteCells_ReflectsOriginOffset()
    {
        var piece = new Piece(TetrominoType.O, 0, 2, 3);
        var cells = piece.GetAbsoluteCells();
        // O piece at rotation 0 has offsets (0,0),(1,0),(0,1),(1,1)
        Assert.Contains((2, 3), cells);
        Assert.Contains((3, 3), cells);
        Assert.Contains((2, 4), cells);
        Assert.Contains((3, 4), cells);
    }
}
