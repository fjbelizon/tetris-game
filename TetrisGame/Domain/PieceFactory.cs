namespace TetrisGame.Domain;

public sealed class PieceFactory
{
    private readonly Random _random;
    private readonly Queue<TetrominoType> _bag = new();

    public PieceFactory(Random? random = null)
    {
        _random = random ?? Random.Shared;
        RefillBag();
    }

    public Piece TakeNext()
    {
        if (_bag.Count == 0)
            RefillBag();

        var type = _bag.Dequeue();
        return CreateSpawnPiece(type);
    }

    private void RefillBag()
    {
        var types = (TetrominoType[])Enum.GetValues(typeof(TetrominoType));
        var shuffled = types.OrderBy(_ => _random.Next()).ToArray();
        foreach (var t in shuffled)
            _bag.Enqueue(t);
    }

    private static Piece CreateSpawnPiece(TetrominoType type)
    {
        int x = type switch
        {
            TetrominoType.I => 3,
            TetrominoType.O => 4,
            _ => 3,
        };
        return new Piece(type, 0, x, 0);
    }
}
