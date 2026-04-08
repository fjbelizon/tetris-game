namespace TetrisGame.Domain;

public sealed class PieceFactory
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
    }

    public Piece Next()
    {
        if (_bag.Count == 0)
            Refill();

        var type = _bag.Dequeue();
        return SpawnPiece(type);
    }

    private void Refill()
    {
        var types = (TetrominoType[])Enum.GetValues(typeof(TetrominoType));
        // Fisher-Yates shuffle
        for (int i = types.Length - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (types[i], types[j]) = (types[j], types[i]);
        }

        foreach (var t in types)
            _bag.Enqueue(t);
    }
        foreach (var t in types)
            _bag.Enqueue(t);
    }

    private static Piece SpawnPiece(TetrominoType type)
    {
        // Spawn at top-center of a 10-wide board
        return type switch
        {
            TetrominoType.I => new Piece(type, 0, 3, 0),
            TetrominoType.O => new Piece(type, 0, 4, 0),
            _ => new Piece(type, 0, 3, 0)
        };
    }
}
