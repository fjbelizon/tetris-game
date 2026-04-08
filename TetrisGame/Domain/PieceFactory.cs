namespace TetrisGame.Domain;

public sealed class PieceFactory
{
    private static readonly TetrominoType[] AllTypes =
        [TetrominoType.I, TetrominoType.O, TetrominoType.T, TetrominoType.S, TetrominoType.Z, TetrominoType.J, TetrominoType.L];

    private readonly Queue<TetrominoType> _bag = new();
    private readonly Random _random;

    public PieceFactory(Random? random = null)
    {
        _random = random ?? Random.Shared;
        Refill();
    }

    public Piece TakeNext()
    {
        if (_bag.Count == 0) Refill();
        return CreatePiece(_bag.Dequeue());
    }

    public TetrominoType PeekNextType()
    {
        if (_bag.Count == 0) Refill();
        return _bag.Peek();
    }

    private void Refill()
    {
        var types = AllTypes.ToArray();
        // Fisher-Yates shuffle
        for (int i = types.Length - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (types[i], types[j]) = (types[j], types[i]);
        }
        foreach (var t in types)
            _bag.Enqueue(t);
    }

    private static Piece CreatePiece(TetrominoType type)
    {
        // All pieces spawn with origin at column 4, row 1.
        // Offsets with y=-1 reach row 0; I piece stays at row 1.
        return new Piece(type, 0, 4, 1);
    }
}
