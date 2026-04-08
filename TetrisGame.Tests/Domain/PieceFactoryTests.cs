using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceFactoryTests
{
    [Fact]
    public void Next_ReturnsAllSevenTypes_InFirstBag()
    {
        var factory = new PieceFactory(new Random(42));
        var types = new HashSet<TetrominoType>();
        for (int i = 0; i < 7; i++)
            types.Add(factory.Next().Type);

        Assert.Equal(7, types.Count);
    }

    [Fact]
    public void Next_Over14Calls_ContainsEachTypeTwice()
    {
        var factory = new PieceFactory(new Random(42));
        var types = new List<TetrominoType>();
        for (int i = 0; i < 14; i++)
            types.Add(factory.Next().Type);

        foreach (TetrominoType t in Enum.GetValues<TetrominoType>())
            Assert.Equal(2, types.Count(x => x == t));
    }

    [Fact]
    public void Next_SpawnsAtRotation0()
    {
        var factory = new PieceFactory();
        var piece = factory.Next();
        Assert.Equal(0, piece.Rotation);
    }

    [Fact]
    public void Next_SpawnsNearTopOfBoard()
    {
        var factory = new PieceFactory();
        var piece = factory.Next();
        Assert.True(piece.OriginY >= 0 && piece.OriginY <= 2);
    }
}
