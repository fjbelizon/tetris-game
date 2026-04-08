namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public sealed class Piece
{
    private static readonly (int dx, int dy)[][][] AllShapes;

    static Piece()
    {
        AllShapes = new (int, int)[][][]
        {
            // I
            new (int,int)[][]
            {
                new (int,int)[] { (-1,0),(0,0),(1,0),(2,0) },
                new (int,int)[] { (1,-1),(1,0),(1,1),(1,2) },
                new (int,int)[] { (-1,1),(0,1),(1,1),(2,1) },
                new (int,int)[] { (0,-1),(0,0),(0,1),(0,2) },
            },
            // O
            new (int,int)[][]
            {
                new (int,int)[] { (0,0),(1,0),(0,1),(1,1) },
                new (int,int)[] { (0,0),(1,0),(0,1),(1,1) },
                new (int,int)[] { (0,0),(1,0),(0,1),(1,1) },
                new (int,int)[] { (0,0),(1,0),(0,1),(1,1) },
            },
            // T
            new (int,int)[][]
            {
                new (int,int)[] { (-1,0),(0,0),(1,0),(0,1) },
                new (int,int)[] { (0,-1),(0,0),(1,0),(0,1) },
                new (int,int)[] { (-1,0),(0,0),(1,0),(0,-1) },
                new (int,int)[] { (0,-1),(0,0),(-1,0),(0,1) },
            },
            // S
            new (int,int)[][]
            {
                new (int,int)[] { (0,0),(1,0),(-1,1),(0,1) },
                new (int,int)[] { (0,-1),(0,0),(1,0),(1,1) },
                new (int,int)[] { (0,0),(1,0),(-1,1),(0,1) },
                new (int,int)[] { (0,-1),(0,0),(1,0),(1,1) },
            },
            // Z
            new (int,int)[][]
            {
                new (int,int)[] { (-1,0),(0,0),(0,1),(1,1) },
                new (int,int)[] { (1,-1),(0,0),(1,0),(0,1) },
                new (int,int)[] { (-1,0),(0,0),(0,1),(1,1) },
                new (int,int)[] { (1,-1),(0,0),(1,0),(0,1) },
            },
            // J
            new (int,int)[][]
            {
                new (int,int)[] { (-1,0),(0,0),(1,0),(-1,1) },
                new (int,int)[] { (0,-1),(0,0),(0,1),(1,-1) },
                new (int,int)[] { (-1,0),(0,0),(1,0),(1,-1) },
                new (int,int)[] { (0,-1),(0,0),(0,1),(-1,1) },
            },
            // L
            new (int,int)[][]
            {
                new (int,int)[] { (-1,0),(0,0),(1,0),(1,1) },
                new (int,int)[] { (0,-1),(0,0),(0,1),(1,1) },
                new (int,int)[] { (-1,0),(0,0),(1,0),(-1,-1) },
                new (int,int)[] { (0,-1),(0,0),(0,1),(-1,-1) },
            },
        };
    }

    public TetrominoType Type { get; }
    public int Rotation { get; }
    public int OriginX { get; }
    public int OriginY { get; }

    public Piece(TetrominoType type, int rotation, int originX, int originY)
    {
        Type = type;
        Rotation = rotation & 3;
        OriginX = originX;
        OriginY = originY;
    }

    public IReadOnlyList<(int X, int Y)> GetAbsoluteCells()
    {
        var offsets = AllShapes[(int)Type][Rotation];
        var cells = new (int, int)[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
            cells[i] = (OriginX + offsets[i].dx, OriginY + offsets[i].dy);
        return cells;
    }

    public Piece Move(int dx, int dy) => new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() => new(Type, (Rotation + 1) & 3, OriginX, OriginY);
}
