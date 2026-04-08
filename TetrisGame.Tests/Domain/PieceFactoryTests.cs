using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceFactoryTests
{
    private static readonly TetrominoType[] AllTypes =
    [
        TetrominoType.I,
        TetrominoType.O,
        TetrominoType.T,
        TetrominoType.S,
        TetrominoType.Z,
        TetrominoType.J,
        TetrominoType.L
    ];

    [Fact]
    public void TakeNext_FirstBag_ContainsAllSevenTypes()
    {
        var factory = new PieceFactory();
        var taken = new List<TetrominoType>();

        for (int i = 0; i < 7; i++)
            taken.Add(factory.TakeNext());

        Assert.Equal(AllTypes.Length, taken.Distinct().Count());
        Assert.All(AllTypes, t => Assert.Contains(t, taken));
    }

    [Fact]
    public void TakeNext_FirstBag_ContainsExactlyOneCopyOfEachType()
    {
        var factory = new PieceFactory();
        var taken = new List<TetrominoType>();

        for (int i = 0; i < 7; i++)
            taken.Add(factory.TakeNext());

        foreach (var type in AllTypes)
            Assert.Single(taken, t => t == type);
    }

    [Fact]
    public void TakeNext_AfterBagExhausted_AutomaticallyRefillsWithAllSevenTypes()
    {
        var factory = new PieceFactory();

        // Drain the first bag
        for (int i = 0; i < 7; i++)
            factory.TakeNext();

        // The second bag should also contain all 7 types exactly once
        var secondBag = new List<TetrominoType>();
        for (int i = 0; i < 7; i++)
            secondBag.Add(factory.TakeNext());

        Assert.Equal(AllTypes.Length, secondBag.Distinct().Count());
        Assert.All(AllTypes, t => Assert.Contains(t, secondBag));
    }

    [Fact]
    public void TakeNext_OverMultipleBags_EachBagContainsAllSevenTypes()
    {
        var factory = new PieceFactory();
        const int bagsToCheck = 5;

        for (int bag = 0; bag < bagsToCheck; bag++)
        {
            var bagContents = new List<TetrominoType>();
            for (int i = 0; i < 7; i++)
                bagContents.Add(factory.TakeNext());

            Assert.Equal(AllTypes.Length, bagContents.Distinct().Count());
            Assert.All(AllTypes, t => Assert.Contains(t, bagContents));
        }
    }

    [Fact]
    public void TakeNext_OverMultipleBags_EachBagContainsExactlyOneOfEachType()
    {
        var factory = new PieceFactory();
        const int bagsToCheck = 3;

        for (int bag = 0; bag < bagsToCheck; bag++)
        {
            var bagContents = new List<TetrominoType>();
            for (int i = 0; i < 7; i++)
                bagContents.Add(factory.TakeNext());

            foreach (var type in AllTypes)
                Assert.Single(bagContents, t => t == type);
        }
    }

    [Fact]
    public void TakeNext_BagDistributionIsRandom_TwoBagsCanDifferInOrder()
    {
        // Run multiple factories and verify that ordering is not always identical
        // (statistically: probability of all being the same is 1/7! per test, negligible over many runs)
        const int attempts = 20;
        var seen = new HashSet<string>();

        for (int attempt = 0; attempt < attempts; attempt++)
        {
            var factory = new PieceFactory();
            var order = new List<TetrominoType>();
            for (int i = 0; i < 7; i++)
                order.Add(factory.TakeNext());
            seen.Add(string.Join(",", order));
        }

        // With 20 factories and 7! = 5040 possible orderings, we expect more than 1 unique order
        Assert.True(seen.Count > 1, "7-bag should shuffle pieces; all bags had the same order across 20 attempts.");
    }

    [Fact]
    public void TakeNext_BagOfExactlySevenPieces_NoBagHasFewerThanSevenBeforeRefill()
    {
        // Verify bag size invariant: after taking 6 pieces the 7th is still in the current bag
        var factory = new PieceFactory();
        var firstSix = new List<TetrominoType>();
        for (int i = 0; i < 6; i++)
            firstSix.Add(factory.TakeNext());

        // There must be exactly one remaining type not yet taken
        var remainingType = AllTypes.Except(firstSix).Single();

        // The 7th piece must be the one remaining type
        var seventh = factory.TakeNext();
        Assert.Equal(remainingType, seventh);
    }

    [Fact]
    public void TakeNext_AllTypesReturnedAcrossLargeDrawSample_EvenDistribution()
    {
        // Draw 70 pieces (10 complete bags) and verify each type appears exactly 10 times
        var factory = new PieceFactory();
        var counts = new Dictionary<TetrominoType, int>();
        foreach (var t in AllTypes)
            counts[t] = 0;

        for (int i = 0; i < 70; i++)
            counts[factory.TakeNext()]++;

        foreach (var type in AllTypes)
            Assert.Equal(10, counts[type]);
    }
}
