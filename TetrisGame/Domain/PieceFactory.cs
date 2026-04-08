namespace TetrisGame.Domain;

/// <summary>
/// Generates tetrominoes using a shuffled 7-bag algorithm.
/// Each bag contains exactly one instance of every TetrominoType.
/// When the bag is exhausted a new shuffled bag is generated automatically.
/// </summary>
public class PieceFactory
{
    private readonly Queue<TetrominoType> _bag = new();
    private readonly Random _random;

    public PieceFactory(Random? random = null)
    {
        _random = random ?? Random.Shared;
        RefillBag();
    }

    /// <summary>
    /// Returns the next TetrominoType from the bag, refilling when empty.
    /// </summary>
    public TetrominoType TakeNext()
    {
        if (_bag.Count == 0)
            RefillBag();
        return _bag.Dequeue();
    }

    private void RefillBag()
    {
        var types = new List<TetrominoType>
        {
            TetrominoType.I,
            TetrominoType.O,
            TetrominoType.T,
            TetrominoType.S,
            TetrominoType.Z,
            TetrominoType.J,
            TetrominoType.L
        };

        // Fisher-Yates shuffle
        for (int i = types.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (types[i], types[j]) = (types[j], types[i]);
        }

        foreach (var t in types)
            _bag.Enqueue(t);
    }
}
