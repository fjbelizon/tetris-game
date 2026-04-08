namespace TetrisGame.Domain;

public sealed class PieceFactory
{
    private readonly Queue<TetrominoType> _bag = new();
    private readonly Random _rng;

    public PieceFactory(Random? rng = null) => _rng = rng ?? Random.Shared;

    public Piece Next()
    {
        RefillIfEmpty();
        var type = _bag.Dequeue();
        return CreateSpawn(type);
    }

    private void RefillIfEmpty()
    {
        if (_bag.Count == 0)
        {
            var types = (TetrominoType[])Enum.GetValues(typeof(TetrominoType));
            var shuffled = types.OrderBy(_ => _rng.Next()).ToArray();
            foreach (var t in shuffled)
                _bag.Enqueue(t);
        }
    }

    private static Piece CreateSpawn(TetrominoType type) => new(type, 0, Board.Width / 2 - 1, 0);

    public void Reset()
    {
        _bag.Clear();
    }
}
