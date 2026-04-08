namespace TetrisGame.Domain;

public class PieceFactory
{
    private readonly Queue<TetrominoType> _bag = new();
    private readonly Random _random;

    public PieceFactory(Random? random = null)
    {
        _random = random ?? Random.Shared;
    }

    public Piece CreateNext()
    {
        RefillIfEmpty();
        var type = _bag.Dequeue();
        return CreateSpawnPiece(type);
    }

    public TetrominoType PeekNext()
    {
        RefillIfEmpty();
        return _bag.Peek();
    }

    private void RefillIfEmpty()
    {
        if (_bag.Count > 0) return;

        var types = (TetrominoType[])Enum.GetValues(typeof(TetrominoType));
        var shuffled = types.OrderBy(_ => _random.Next()).ToArray();
        foreach (var t in shuffled)
            _bag.Enqueue(t);
    }

    private static Piece CreateSpawnPiece(TetrominoType type)
    {
        // Spawn near the top-center of the board
        int spawnX = Board.Width / 2 - 1;
        int spawnY = 1;
        return new Piece(type, 0, spawnX, spawnY);
    }
}
