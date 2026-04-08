namespace TetrisGame.Domain;

public sealed class PieceFactory
{
    private static readonly TetrominoType[] AllTypes =
        [TetrominoType.I, TetrominoType.O, TetrominoType.T, TetrominoType.S, TetrominoType.Z, TetrominoType.J, TetrominoType.L];

    private readonly Random _random;
    private readonly Queue<TetrominoType> _bag = new();

    public PieceFactory(Random? random = null)
    {
        _random = random ?? Random.Shared;
        Refill();
    }

    public TetrominoType TakeNext()
    {
        if (_bag.Count == 0) Refill();
        return _bag.Dequeue();
    }

    public Piece CreateSpawnPiece(TetrominoType type)
    {
        // Spawn near top-center of board
        int spawnX = type == TetrominoType.I ? 3 : 4;
        return new Piece(type, 0, spawnX, 0);
    }

    private void Refill()
    {
        var types = (TetrominoType[])AllTypes.Clone();
        // Fisher-Yates shuffle
        for (int i = types.Length - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (types[i], types[j]) = (types[j], types[i]);
        }
        foreach (var t in types)
            _bag.Enqueue(t);
    }

    public void Reset()
    {
        _bag.Clear();
        Refill();
    }
}
