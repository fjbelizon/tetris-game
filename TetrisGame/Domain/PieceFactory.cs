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

    public Piece Next()
    {
        if (_bag.Count == 0)
            RefillBag();

        var type = _bag.Dequeue();
        return Spawn(type);
    }

    public static Piece Spawn(TetrominoType type)
    {
        const int spawnX = 4;
        const int spawnY = 0;
        return new Piece(type, 0, spawnX, spawnY);
    }

    private void RefillBag()
    {
        var types = (TetrominoType[])Enum.GetValues(typeof(TetrominoType));
        Shuffle(types);
        foreach (var t in types)
            _bag.Enqueue(t);
    }

    private void Shuffle(TetrominoType[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
