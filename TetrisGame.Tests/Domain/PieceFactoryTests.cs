using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class PieceFactoryTests
{
    [Fact]
    public void Factory_ProducesAll7PieceTypes_InOneBag()
    {
        var factory = new PieceFactory();
        var types = new HashSet<PieceType>();
        for (int i = 0; i < 7; i++)
            types.Add(factory.Next().Type);

        Assert.Equal(7, types.Count);
        foreach (PieceType type in Enum.GetValues(typeof(PieceType)))
            Assert.Contains(type, types);
    }

    [Fact]
    public void Factory_Refills_After7Pieces()
    {
        var factory = new PieceFactory();
        for (int i = 0; i < 7; i++) factory.Next();

        // Next 7 should again contain all types
        var types = new HashSet<PieceType>();
        for (int i = 0; i < 7; i++)
            types.Add(factory.Next().Type);

        Assert.Equal(7, types.Count);
    }
}
