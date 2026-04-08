namespace TetrisGame.Domain;

public sealed class PieceFactory
{
    private readonly Queue<TetrominoType> _bag = new();
    private readonly Random _random;

    public PieceFactory(Random? random = null)
    {
        _random = random ?? new Random();
        Refill();
    }

    public Piece Next()
    {
        if (_bag.Count == 0)
            Refill();
        var type = _bag.Dequeue();
        return SpawnPiece(type);
    }

    public void Reset()
    {
        _bag.Clear();
        Refill();
    }

    private void Refill()
    {
        var types = Enum.GetValues<TetrominoType>();
        var shuffled = types.OrderBy(_ => _random.Next()).ToList();
        foreach (var t in shuffled)
            _bag.Enqueue(t);
    }

    private static Piece SpawnPiece(TetrominoType type)
    {
        int spawnX = Board.Width / 2 - 1;
        int spawnY = 0;
        return new Piece(type, 0, spawnX, spawnY);
    }
}
