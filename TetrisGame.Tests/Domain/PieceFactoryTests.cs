using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceFactoryTests
{
    [Fact]
    public void CreateNext_ReturnsAllSevenTypesInOneBag()
    {
        var factory = new PieceFactory();
        var types = new HashSet<TetrominoType>();

        for (int i = 0; i < 7; i++)
        {
            var piece = factory.CreateNext();
            types.Add(piece.Type);
        }

        Assert.Equal(7, types.Count);
    }

    [Fact]
    public void CreateNext_SecondBagAlsoHasAllSevenTypes()
    {
        var factory = new PieceFactory();
        for (int i = 0; i < 7; i++) factory.CreateNext(); // drain first bag

        var types = new HashSet<TetrominoType>();
        for (int i = 0; i < 7; i++)
        {
            types.Add(factory.CreateNext().Type);
        }

        Assert.Equal(7, types.Count);
    }

    [Fact]
    public void PeekNext_DoesNotConsumeFromBag()
    {
        var factory = new PieceFactory();
        var peeked = factory.PeekNext();
        var first = factory.CreateNext();
        Assert.Equal(peeked, first.Type);
    }

    [Fact]
    public void CreateNext_SpawnsAtTopCenter()
    {
        var factory = new PieceFactory();
        var piece = factory.CreateNext();
        // Spawn origin should be near center column 4 (width/2 - 1) and row 1
        Assert.Equal(Board.Width / 2 - 1, piece.OriginX);
        Assert.Equal(1, piece.OriginY);
    }
}
