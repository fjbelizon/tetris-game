using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceFactoryTests
{
    [Fact]
    public void TakeNext_ReturnsAllSevenTypesPerBag()
    {
        var factory = new PieceFactory();
        var types = new HashSet<TetrominoType>();
        for (int i = 0; i < 7; i++)
            types.Add(factory.TakeNext());
        Assert.Equal(7, types.Count);
    }

    [Fact]
    public void TakeNext_RefillsAfterExhausting()
    {
        var factory = new PieceFactory();
        // Exhaust first bag
        for (int i = 0; i < 7; i++) factory.TakeNext();
        // Second bag should also yield 7 distinct types
        var types = new HashSet<TetrominoType>();
        for (int i = 0; i < 7; i++)
            types.Add(factory.TakeNext());
        Assert.Equal(7, types.Count);
    }

    [Fact]
    public void CreateSpawnPiece_ReturnsCorrectType()
    {
        var factory = new PieceFactory();
        var piece = factory.CreateSpawnPiece(TetrominoType.T);
        Assert.Equal(TetrominoType.T, piece.Type);
        Assert.Equal(0, piece.Rotation);
    }
}
