using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceFactoryTests
{
    [Fact]
    public void Next_ReturnsAllSevenTypesInFirstBag()
    {
        var factory = new PieceFactory();
        var types = new HashSet<TetrominoType>();

        for (int i = 0; i < 7; i++)
            types.Add(factory.Next().Type);

        Assert.Equal(7, types.Count);
        foreach (TetrominoType t in Enum.GetValues(typeof(TetrominoType)))
            Assert.Contains(t, types);
    }

    [Fact]
    public void Next_ReturnsAllSevenTypesAcrossMultipleBags()
    {
        var factory = new PieceFactory();
        var types = new HashSet<TetrominoType>();

        for (int i = 0; i < 14; i++)
            types.Add(factory.Next().Type);

        Assert.Equal(7, types.Count);
    }

    [Fact]
    public void Next_AutoRefillsWhenBagIsExhausted()
    {
        var factory = new PieceFactory();

        // Drain first bag
        for (int i = 0; i < 7; i++)
            factory.Next();

        // Second bag should work without exception
        var typesInSecondBag = new HashSet<TetrominoType>();
        for (int i = 0; i < 7; i++)
            typesInSecondBag.Add(factory.Next().Type);

        Assert.Equal(7, typesInSecondBag.Count);
    }

    [Fact]
    public void Next_EachBagContainsEachTypeExactlyOnce()
    {
        var factory = new PieceFactory();

        // Collect 7 bags
        for (int bag = 0; bag < 7; bag++)
        {
            var typeCounts = new Dictionary<TetrominoType, int>();
            for (int i = 0; i < 7; i++)
            {
                var t = factory.Next().Type;
                typeCounts[t] = typeCounts.GetValueOrDefault(t) + 1;
            }

            foreach (TetrominoType type in Enum.GetValues(typeof(TetrominoType)))
                Assert.Equal(1, typeCounts[type]);
        }
    }

    [Fact]
    public void Next_SpawnedPiece_HasRotationZero()
    {
        var factory = new PieceFactory();
        var piece = factory.Next();
        Assert.Equal(0, piece.Rotation);
    }

    [Fact]
    public void Next_SpawnedPiece_HasPositiveOriginY()
    {
        var factory = new PieceFactory();
        for (int i = 0; i < 7; i++)
        {
            var piece = factory.Next();
            Assert.True(piece.OriginY >= 0, $"Expected OriginY >= 0 for {piece.Type}");
        }
    }

    [Fact]
    public void Spawn_ReturnsCorrectType()
    {
        foreach (TetrominoType type in Enum.GetValues(typeof(TetrominoType)))
        {
            var piece = PieceFactory.Spawn(type);
            Assert.Equal(type, piece.Type);
        }
    }

    [Fact]
    public void BagDistribution_IsNotAlwaysSameOrder()
    {
        // With a non-seeded random, it's astronomically unlikely two bags are identical.
        // Use two factories with different seeds to guarantee distinct orderings in test.
        var factory1 = new PieceFactory(new Random(1));
        var factory2 = new PieceFactory(new Random(2));

        var bag1 = Enumerable.Range(0, 7).Select(_ => factory1.Next().Type).ToArray();
        var bag2 = Enumerable.Range(0, 7).Select(_ => factory2.Next().Type).ToArray();

        Assert.False(bag1.SequenceEqual(bag2), "Two different seeds should produce different orderings.");
    }
}
