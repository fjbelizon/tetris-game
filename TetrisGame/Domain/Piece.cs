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

    public IReadOnlyList<(int x, int y)> GetAbsoluteCells()
    {
        var offsets = Shapes[(int)Type][Rotation];
        var result = new (int x, int y)[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
            result[i] = (OriginX + offsets[i].dx, OriginY + offsets[i].dy);
        return result;
    }

    public Piece Move(int dx, int dy) => new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() => new(Type, (Rotation + 1) & 3, OriginX, OriginY);

    private static (int dx, int dy)[][][] BuildShapes()
    {
        static (int dx, int dy)[] R(params (int, int)[] cells) => cells;

        var shapes = new (int dx, int dy)[7][][];

        // I
        shapes[(int)TetrominoType.I] = new[]
        {
            R((0,-1),(0,0),(0,1),(0,2)),
            R((-1,0),(0,0),(1,0),(2,0)),
            R((0,-1),(0,0),(0,1),(0,2)),
            R((-1,0),(0,0),(1,0),(2,0)),
        };

        // O
        shapes[(int)TetrominoType.O] = new[]
        {
            R((0,0),(1,0),(0,1),(1,1)),
            R((0,0),(1,0),(0,1),(1,1)),
            R((0,0),(1,0),(0,1),(1,1)),
            R((0,0),(1,0),(0,1),(1,1)),
        };

        // T
        shapes[(int)TetrominoType.T] = new[]
        {
            R((0,0),(-1,0),(1,0),(0,-1)),
            R((0,0),(0,-1),(0,1),(1,0)),
            R((0,0),(1,0),(-1,0),(0,1)),
            R((0,0),(0,1),(0,-1),(-1,0)),
        };

        // S
        shapes[(int)TetrominoType.S] = new[]
        {
            R((0,0),(-1,0),(0,-1),(1,-1)),
            R((0,0),(0,-1),(1,0),(1,1)),
            R((0,0),(-1,0),(0,-1),(1,-1)),
            R((0,0),(0,-1),(1,0),(1,1)),
        };

        // Z
        shapes[(int)TetrominoType.Z] = new[]
        {
            R((0,0),(1,0),(0,-1),(-1,-1)),
            R((0,0),(0,1),(1,0),(1,-1)),
            R((0,0),(1,0),(0,-1),(-1,-1)),
            R((0,0),(0,1),(1,0),(1,-1)),
        };

        // J
        shapes[(int)TetrominoType.J] = new[]
        {
            R((0,0),(-1,0),(1,0),(-1,-1)),
            R((0,0),(0,-1),(0,1),(1,-1)),
            R((0,0),(1,0),(-1,0),(1,1)),
            R((0,0),(0,1),(0,-1),(-1,1)),
        };

        // L
        shapes[(int)TetrominoType.L] = new[]
        {
            R((0,0),(-1,0),(1,0),(1,-1)),
            R((0,0),(0,-1),(0,1),(1,1)),
            R((0,0),(1,0),(-1,0),(-1,1)),
            R((0,0),(0,1),(0,-1),(-1,-1)),
        };

        return shapes;
    }
}
