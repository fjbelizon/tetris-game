using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class PieceFactoryTests
{
    [Fact]
    public void PieceFactory_ProducesAllSevenTypesInFirstBag()
    {
        var rng = new Random(42);
        var factory = new PieceFactory(rng);

        var types = new HashSet<TetrominoType>();
        for (int i = 0; i < 7; i++)
            types.Add(factory.Next().Type);

        foreach (TetrominoType t in Enum.GetValues<TetrominoType>())
            Assert.Contains(t, types);
    }

    [Fact]
    public void PieceFactory_ProducesAllSevenTypesInSecondBag()
    {
        var rng = new Random(99);
        var factory = new PieceFactory(rng);

        // Exhaust first bag
        for (int i = 0; i < 7; i++) factory.Next();

        // Second bag should also contain all 7 types
        var types = new HashSet<TetrominoType>();
        for (int i = 0; i < 7; i++)
            types.Add(factory.Next().Type);

        foreach (TetrominoType t in Enum.GetValues<TetrominoType>())
            Assert.Contains(t, types);
    }

    [Fact]
    public void PieceFactory_NextPiece_HasValidRotation()
    {
        var factory = new PieceFactory();
        for (int i = 0; i < 14; i++)
        {
            var piece = factory.Next();
            Assert.InRange(piece.Rotation, 0, 3);
        }
    }

    [Fact]
    public void PieceFactory_EachBagHasExactly7Pieces()
    {
        var rng = new Random(1);
        var factory = new PieceFactory(rng);

        // Run 3 bags worth of pieces
        var allPieces = new List<Piece>();
        for (int i = 0; i < 21; i++)
            allPieces.Add(factory.Next());

        // Each group of 7 should have exactly one of each type
        for (int bag = 0; bag < 3; bag++)
        {
            var bagTypes = allPieces.Skip(bag * 7).Take(7).Select(p => p.Type).ToHashSet();
            Assert.Equal(7, bagTypes.Count);
        }
    }
}
