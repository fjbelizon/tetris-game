namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public sealed class Piece
{
    public TetrominoType Type { get; }
    public int Rotation { get; }
    public int OriginX { get; }
    public int OriginY { get; }

    private static readonly (int dx, int dy)[][][] Shapes = BuildShapes();

    public Piece(TetrominoType type, int rotation, int originX, int originY)
    {
        Type = type;
        Rotation = rotation & 3;
        OriginX = originX;
        OriginY = originY;
    }

    public (int x, int y)[] GetAbsoluteCells()
    {
        var offsets = Shapes[(int)Type][Rotation];
        var cells = new (int x, int y)[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
            cells[i] = (OriginX + offsets[i].dx, OriginY + offsets[i].dy);
        return cells;
    }

    public Piece Move(int dx, int dy) => new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() => new(Type, (Rotation + 1) & 3, OriginX, OriginY);

    private static (int dx, int dy)[][][] BuildShapes()
    {
        // Shapes indexed by [TetrominoType][rotation 0-3][(block offsets)]
        // Spawn origin for each piece places it at the top-center of a 10-wide board (col 3 or 4).
        // All rotations are pre-defined (clockwise).
        return new (int dx, int dy)[][][]
        {
            // I
            new[]
            {
                new[] { (-1, 0), (0, 0), (1, 0), (2, 0) },
                new[] { (0, -1), (0, 0), (0, 1), (0, 2) },
                new[] { (-1, 0), (0, 0), (1, 0), (2, 0) },
                new[] { (0, -1), (0, 0), (0, 1), (0, 2) },
            },
            // O
            new[]
            {
                new[] { (0, 0), (1, 0), (0, 1), (1, 1) },
                new[] { (0, 0), (1, 0), (0, 1), (1, 1) },
                new[] { (0, 0), (1, 0), (0, 1), (1, 1) },
                new[] { (0, 0), (1, 0), (0, 1), (1, 1) },
            },
            // T
            new[]
            {
                new[] { (-1, 0), (0, 0), (1, 0), (0, 1) },
                new[] { (0, -1), (0, 0), (0, 1), (-1, 0) },
                new[] { (-1, 0), (0, 0), (1, 0), (0, -1) },
                new[] { (0, -1), (0, 0), (0, 1), (1, 0) },
            },
            // S
            new[]
            {
                new[] { (0, 0), (1, 0), (-1, 1), (0, 1) },
                new[] { (0, -1), (0, 0), (1, 0), (1, 1) },
                new[] { (0, 0), (1, 0), (-1, 1), (0, 1) },
                new[] { (0, -1), (0, 0), (1, 0), (1, 1) },
            },
            // Z
            new[]
            {
                new[] { (-1, 0), (0, 0), (0, 1), (1, 1) },
                new[] { (1, -1), (0, 0), (1, 0), (0, 1) },
                new[] { (-1, 0), (0, 0), (0, 1), (1, 1) },
                new[] { (1, -1), (0, 0), (1, 0), (0, 1) },
            },
            // J
            new[]
            {
                new[] { (-1, 0), (0, 0), (1, 0), (-1, 1) },
                new[] { (0, -1), (0, 0), (0, 1), (-1, -1) },
                new[] { (-1, 0), (0, 0), (1, 0), (1, -1) },
                new[] { (0, -1), (0, 0), (0, 1), (1, 1) },
            },
            // L
            new[]
            {
                new[] { (-1, 0), (0, 0), (1, 0), (1, 1) },
                new[] { (0, -1), (0, 0), (0, 1), (-1, 1) },
                new[] { (-1, 0), (0, 0), (1, 0), (-1, -1) },
                new[] { (0, -1), (0, 0), (0, 1), (1, -1) },
            },
        };
    }
}
