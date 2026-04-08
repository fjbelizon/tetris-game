namespace TetrisGame.Domain;

public sealed class PieceFactory
{
    private static readonly TetrominoType[] AllTypes =
        [TetrominoType.I, TetrominoType.O, TetrominoType.T,
         TetrominoType.S, TetrominoType.Z, TetrominoType.J, TetrominoType.L];

    private readonly Random _rng;
    private readonly Queue<TetrominoType> _bag = new();

    public PieceFactory(Random? rng = null)
    {
        _rng = rng ?? Random.Shared;
        RefillBag();
    }

    public Piece Next()
    {
        if (_bag.Count == 0)
            RefillBag();

        var type = _bag.Dequeue();
        // Spawn near column 3-4 (center of 10-wide board), row 0
        return new Piece(type, 0, 3, 0);
    }

    private void RefillBag()
    {
        var shuffled = AllTypes.OrderBy(_ => _rng.Next()).ToArray();
        foreach (var t in shuffled)
            _bag.Enqueue(t);
    }
}
