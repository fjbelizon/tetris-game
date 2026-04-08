namespace TetrisGame.Domain;

/// <summary>
/// Shuffled 7-bag piece generator.
/// Each bag contains exactly one instance of every tetromino type.
/// The bag is reshuffled when exhausted.
/// </summary>
public sealed class PieceFactory
{
    private readonly Random _random;
    private readonly Queue<TetrominoType> _bag = new();

    public PieceFactory(Random? random = null)
    {
        _random = random ?? Random.Shared;
        Refill();
    }

    /// <summary>Returns the next piece from the bag, refilling if necessary.</summary>
    public Piece Next()
    {
        if (_bag.Count == 0)
            Refill();
        return Piece.Spawn(_bag.Dequeue());
    }

    private void Refill()
    {
        var types = Enum.GetValues<TetrominoType>().OrderBy(_ => _random.Next());
        foreach (var t in types)
            _bag.Enqueue(t);
    }
}
