namespace TetrisGame.Domain;

public class PieceFactory
{
    private readonly Random _random;
    private readonly Queue<PieceType> _bag = new();

    public PieceFactory(Random? random = null)
    {
        _random = random ?? Random.Shared;
        Refill();
    }

    public Piece Next()
    {
        if (_bag.Count == 0)
            Refill();

        var type = _bag.Dequeue();
        return new Piece(type);
    }

    public void Reset()
    {
        _bag.Clear();
        Refill();
    }

    private void Refill()
    {
        var types = (PieceType[])Enum.GetValues(typeof(PieceType));
        var shuffled = types.OrderBy(_ => _random.Next()).ToList();
        foreach (var t in shuffled)
            _bag.Enqueue(t);
    }
}
