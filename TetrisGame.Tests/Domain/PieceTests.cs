using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceTests
{
    [Fact]
    public void Piece_HasCorrectType()
    {
        var piece = new Piece(PieceType.T, 0, 4, 0);
        Assert.Equal(PieceType.T, piece.Type);
    }

    [Theory]
    [InlineData(PieceType.I)]
    [InlineData(PieceType.O)]
    [InlineData(PieceType.T)]
    [InlineData(PieceType.S)]
    [InlineData(PieceType.Z)]
    [InlineData(PieceType.J)]
    [InlineData(PieceType.L)]
    public void Piece_TypeIsPreservedForAllPieceTypes(PieceType type)
    {
        var piece = new Piece(type, 0, 4, 0);
        Assert.Equal(type, piece.Type);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Piece_RotationStoredCorrectly(int rotation)
    {
        var piece = new Piece(PieceType.T, rotation, 4, 0);
        Assert.Equal(rotation, piece.Rotation);
    }

    [Fact]
    public void Piece_RotationWrapsModulo4()
    {
        var piece = new Piece(PieceType.T, 4, 4, 0);
        Assert.Equal(0, piece.Rotation);

        var piece2 = new Piece(PieceType.T, 5, 4, 0);
        Assert.Equal(1, piece2.Rotation);
    }

    [Fact]
    public void Piece_OriginIsStoredCorrectly()
    {
        var piece = new Piece(PieceType.L, 0, 3, 7);
        Assert.Equal(3, piece.OriginX);
        Assert.Equal(7, piece.OriginY);
    }

    [Fact]
    public void Move_ReturnsPieceWithUpdatedOrigin()
    {
        var piece = new Piece(PieceType.T, 0, 4, 0);
        var moved = piece.Move(1, 0);

        Assert.Equal(5, moved.OriginX);
        Assert.Equal(0, moved.OriginY);
    }

    [Fact]
    public void Move_Down_IncreasesOriginY()
    {
        var piece = new Piece(PieceType.T, 0, 4, 5);
        var moved = piece.Move(0, 1);

        Assert.Equal(4, moved.OriginX);
        Assert.Equal(6, moved.OriginY);
    }

    [Fact]
    public void Move_PreservesTypeAndRotation()
    {
        var piece = new Piece(PieceType.S, 2, 4, 3);
        var moved = piece.Move(-1, 2);

        Assert.Equal(PieceType.S, moved.Type);
        Assert.Equal(2, moved.Rotation);
        Assert.Equal(3, moved.OriginX);
        Assert.Equal(5, moved.OriginY);
    }

    [Fact]
    public void Move_IsImmutable_OriginalUnchanged()
    {
        var piece = new Piece(PieceType.I, 0, 4, 0);
        _ = piece.Move(3, 2);

        Assert.Equal(4, piece.OriginX);
        Assert.Equal(0, piece.OriginY);
    }

    [Fact]
    public void RotateClockwise_IncrementsRotationByOne()
    {
        var piece = new Piece(PieceType.T, 0, 4, 0);
        var rotated = piece.RotateClockwise();
        Assert.Equal(1, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_WrapsFromRotation3To0()
    {
        var piece = new Piece(PieceType.T, 3, 4, 0);
        var rotated = piece.RotateClockwise();
        Assert.Equal(0, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_PreservesOriginAndType()
    {
        var piece = new Piece(PieceType.J, 1, 5, 8);
        var rotated = piece.RotateClockwise();

        Assert.Equal(PieceType.J, rotated.Type);
        Assert.Equal(5, rotated.OriginX);
        Assert.Equal(8, rotated.OriginY);
        Assert.Equal(2, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_IsImmutable_OriginalUnchanged()
    {
        var piece = new Piece(PieceType.T, 0, 4, 0);
        _ = piece.RotateClockwise();
        Assert.Equal(0, piece.Rotation);
    }

    [Fact]
    public void GetAbsoluteCells_ReturnsCorrectCountForAllTypes()
    {
        foreach (PieceType type in Enum.GetValues<PieceType>())
        {
            var piece = new Piece(type, 0, 0, 0);
            var cells = piece.GetAbsoluteCells();
            Assert.Equal(4, cells.Count);
        }
    }

    [Fact]
    public void GetAbsoluteCells_OffsetsTranslateByOrigin()
    {
        var piece = new Piece(PieceType.O, 0, 5, 10);
        var cells = piece.GetAbsoluteCells();

        foreach (var cell in cells)
        {
            Assert.True(cell.X >= 5 && cell.X <= 6,
                $"Expected O-piece cell X in [5,6], got {cell.X}");
            Assert.True(cell.Y >= 10 && cell.Y <= 11,
                $"Expected O-piece cell Y in [10,11], got {cell.Y}");
        }
    }

    [Fact]
    public void GetAbsoluteCells_O_Piece_IsIdempotentAcrossRotations()
    {
        var rot0 = new Piece(PieceType.O, 0, 4, 0).GetAbsoluteCells();
        var rot1 = new Piece(PieceType.O, 1, 4, 0).GetAbsoluteCells();
        var rot2 = new Piece(PieceType.O, 2, 4, 0).GetAbsoluteCells();
        var rot3 = new Piece(PieceType.O, 3, 4, 0).GetAbsoluteCells();

        Assert.Equal(rot0.OrderBy(c => c.X).ThenBy(c => c.Y),
                     rot1.OrderBy(c => c.X).ThenBy(c => c.Y));
        Assert.Equal(rot0.OrderBy(c => c.X).ThenBy(c => c.Y),
                     rot2.OrderBy(c => c.X).ThenBy(c => c.Y));
        Assert.Equal(rot0.OrderBy(c => c.X).ThenBy(c => c.Y),
                     rot3.OrderBy(c => c.X).ThenBy(c => c.Y));
    }

    [Fact]
    public void GetAbsoluteCells_I_Piece_Rotation0_IsHorizontal()
    {
        var piece = new Piece(PieceType.I, 0, 0, 0);
        var cells = piece.GetAbsoluteCells();

        var ys = cells.Select(c => c.Y).Distinct().ToList();
        Assert.Single(ys);
    }

    [Fact]
    public void GetAbsoluteCells_I_Piece_Rotation1_IsVertical()
    {
        var piece = new Piece(PieceType.I, 1, 0, 0);
        var cells = piece.GetAbsoluteCells();

        var xs = cells.Select(c => c.X).Distinct().ToList();
        Assert.Single(xs);
    }

    [Fact]
    public void GetAbsoluteCells_AllCellsAreUnique()
    {
        foreach (PieceType type in Enum.GetValues<PieceType>())
        {
            for (var rotation = 0; rotation < 4; rotation++)
            {
                var piece = new Piece(type, rotation, 0, 0);
                var cells = piece.GetAbsoluteCells();
                var unique = cells.Distinct().Count();
                Assert.Equal(4, unique);
            }
        }
    }
}
