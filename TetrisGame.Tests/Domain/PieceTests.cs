using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceTests
{
    // ── Type ─────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(TetrominoType.I)]
    [InlineData(TetrominoType.O)]
    [InlineData(TetrominoType.T)]
    [InlineData(TetrominoType.S)]
    [InlineData(TetrominoType.Z)]
    [InlineData(TetrominoType.J)]
    [InlineData(TetrominoType.L)]
    public void Piece_StoresCorrectType(TetrominoType type)
    {
        var piece = new Piece(type);

        Assert.Equal(type, piece.Type);
    }

    // ── Rotation state ───────────────────────────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Piece_StoresRotationInRange0To3(int rotation)
    {
        var piece = new Piece(TetrominoType.T, rotation);

        Assert.Equal(rotation, piece.Rotation);
    }

    [Fact]
    public void Piece_DefaultRotationIsZero()
    {
        var piece = new Piece(TetrominoType.I);

        Assert.Equal(0, piece.Rotation);
    }

    [Theory]
    [InlineData(4, 0)]
    [InlineData(5, 1)]
    [InlineData(-1, 3)]
    [InlineData(-4, 0)]
    public void Piece_RotationWrapsModulo4(int rawRotation, int expectedRotation)
    {
        var piece = new Piece(TetrominoType.T, rawRotation);

        Assert.Equal(expectedRotation, piece.Rotation);
    }

    // ── Origin coordinates ───────────────────────────────────────────────────

    [Fact]
    public void Piece_DefaultOriginIsZeroZero()
    {
        var piece = new Piece(TetrominoType.S);

        Assert.Equal(0, piece.OriginX);
        Assert.Equal(0, piece.OriginY);
    }

    [Theory]
    [InlineData(3, 0)]
    [InlineData(0, 5)]
    [InlineData(4, 7)]
    [InlineData(9, 19)]
    public void Piece_StoresOriginXAndOriginY(int x, int y)
    {
        var piece = new Piece(TetrominoType.L, originX: x, originY: y);

        Assert.Equal(x, piece.OriginX);
        Assert.Equal(y, piece.OriginY);
    }

    // ── Move ─────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(1, 0)]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(2, 3)]
    public void Move_ReturnsNewPieceWithShiftedOrigin(int dx, int dy)
    {
        var piece = new Piece(TetrominoType.J, originX: 4, originY: 2);

        var moved = piece.Move(dx, dy);

        Assert.Equal(4 + dx, moved.OriginX);
        Assert.Equal(2 + dy, moved.OriginY);
    }

    [Fact]
    public void Move_DoesNotMutateOriginalPiece()
    {
        var piece = new Piece(TetrominoType.I, originX: 3, originY: 1);

        _ = piece.Move(2, 1);

        Assert.Equal(3, piece.OriginX);
        Assert.Equal(1, piece.OriginY);
    }

    [Fact]
    public void Move_PreservesTypeAndRotation()
    {
        var piece = new Piece(TetrominoType.Z, rotation: 2, originX: 1, originY: 1);

        var moved = piece.Move(1, 0);

        Assert.Equal(TetrominoType.Z, moved.Type);
        Assert.Equal(2, moved.Rotation);
    }

    // ── RotateClockwise ──────────────────────────────────────────────────────

    [Fact]
    public void RotateClockwise_IncrementsRotationByOne()
    {
        var piece = new Piece(TetrominoType.T, rotation: 0);

        var rotated = piece.RotateClockwise();

        Assert.Equal(1, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_WrapsFrom3To0()
    {
        var piece = new Piece(TetrominoType.T, rotation: 3);

        var rotated = piece.RotateClockwise();

        Assert.Equal(0, rotated.Rotation);
    }

    [Fact]
    public void RotateClockwise_AfterFourRotationsReturnsSameRotation()
    {
        var piece = new Piece(TetrominoType.S, rotation: 0);

        var result = piece
            .RotateClockwise()
            .RotateClockwise()
            .RotateClockwise()
            .RotateClockwise();

        Assert.Equal(0, result.Rotation);
    }

    [Fact]
    public void RotateClockwise_DoesNotMutateOriginalPiece()
    {
        var piece = new Piece(TetrominoType.T, rotation: 1);

        _ = piece.RotateClockwise();

        Assert.Equal(1, piece.Rotation);
    }

    [Fact]
    public void RotateClockwise_PreservesTypeAndOrigin()
    {
        var piece = new Piece(TetrominoType.L, rotation: 0, originX: 4, originY: 5);

        var rotated = piece.RotateClockwise();

        Assert.Equal(TetrominoType.L, rotated.Type);
        Assert.Equal(4, rotated.OriginX);
        Assert.Equal(5, rotated.OriginY);
    }

    // ── O-piece rotation invariant ───────────────────────────────────────────

    [Fact]
    public void OPiece_RotateClockwise_IsIdempotentInCellShape()
    {
        var piece = new Piece(TetrominoType.O, rotation: 0, originX: 3, originY: 0);

        var rotated = piece.RotateClockwise();

        Assert.Equal(piece.GetAbsoluteCells(), rotated.GetAbsoluteCells());
    }

    // ── GetAbsoluteCells ─────────────────────────────────────────────────────

    [Fact]
    public void GetAbsoluteCells_ReturnsFourCells()
    {
        var piece = new Piece(TetrominoType.I);

        Assert.Equal(4, piece.GetAbsoluteCells().Count);
    }

    [Fact]
    public void GetAbsoluteCells_OffsetsByOrigin()
    {
        int expectedMinX = 3;
        int expectedMinY = 5;
        var piece = new Piece(TetrominoType.O, rotation: 0, originX: expectedMinX, originY: expectedMinY);

        var cells = piece.GetAbsoluteCells();

        Assert.All(cells, cell =>
        {
            Assert.True(cell.X >= expectedMinX, $"Expected X >= {expectedMinX} but got {cell.X}");
            Assert.True(cell.Y >= expectedMinY, $"Expected Y >= {expectedMinY} but got {cell.Y}");
        });
    }

    [Fact]
    public void GetAbsoluteCells_AtOriginZeroMatchesShapeOffsets()
    {
        var piece = new Piece(TetrominoType.T, rotation: 0, originX: 0, originY: 0);

        var cells = piece.GetAbsoluteCells();
        var offsets = piece.ShapeOffsets;

        Assert.Equal(offsets.Count, cells.Count);
        for (int i = 0; i < offsets.Count; i++)
        {
            Assert.Equal(offsets[i].dx, cells[i].X);
            Assert.Equal(offsets[i].dy, cells[i].Y);
        }
    }
}
