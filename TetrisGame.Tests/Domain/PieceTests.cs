using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class PieceTests
{
    [Fact]
    public void Piece_HasCorrectInitialType()
    {
        var piece = new Piece(PieceType.T);
        Assert.Equal(PieceType.T, piece.Type);
    }

    [Fact]
    public void Piece_HasCorrectInitialRotation()
    {
        var piece = new Piece(PieceType.T);
        Assert.Equal(0, piece.Rotation);
    }

    [Fact]
    public void Piece_HasDefaultOriginPosition()
    {
        var piece = new Piece(PieceType.T);
        Assert.Equal(0, piece.Row);
        Assert.Equal(3, piece.Col);
    }

    [Fact]
    public void Piece_GetCells_ReturnsFourCells()
    {
        foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
        {
            var piece = new Piece(type);
            Assert.Equal(4, piece.GetCells().Count());
        }
    }

    [Fact]
    public void Piece_WithOffset_UpdatesPosition()
    {
        var piece = new Piece(PieceType.I, 5, 3);
        var moved = piece.WithOffset(1, 0);
        Assert.Equal(6, moved.Row);
        Assert.Equal(3, moved.Col);
    }

    [Fact]
    public void Piece_Rotated_IncrementsRotation()
    {
        var piece = new Piece(PieceType.T);
        var rotated = piece.Rotated();
        Assert.Equal(1, rotated.Rotation);
    }

    [Fact]
    public void Piece_Rotated_WrapsAt4()
    {
        var piece = new Piece(PieceType.T);
        var r4 = piece.Rotated().Rotated().Rotated().Rotated();
        Assert.Equal(0, r4.Rotation);
    }
}
