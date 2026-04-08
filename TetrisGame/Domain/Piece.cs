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
        Rotation = ((rotation % 4) + 4) % 4;
        OriginX = originX;
        OriginY = originY;
    }

    public IEnumerable<(int x, int y)> GetAbsoluteCells()
    {
        foreach (var (dx, dy) in Shapes[(int)Type][Rotation])
            yield return (OriginX + dx, OriginY + dy);
    }

    public Piece Move(int dx, int dy) => new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() => new(Type, (Rotation + 1) % 4, OriginX, OriginY);

    private static (int dx, int dy)[][][] BuildShapes()
    {
        // Each piece has 4 rotation states, each with a list of cell offsets from origin.
        // Rotations follow clockwise 90-degree transforms.
        return
        [
            // I
            [
                [(0,0),(1,0),(2,0),(3,0)],    // 0
                [(2,-1),(2,0),(2,1),(2,2)],   // 1
                [(0,1),(1,1),(2,1),(3,1)],    // 2
                [(1,-1),(1,0),(1,1),(1,2)],   // 3
            ],
            // O
            [
                [(0,0),(1,0),(0,1),(1,1)],
                [(0,0),(1,0),(0,1),(1,1)],
                [(0,0),(1,0),(0,1),(1,1)],
                [(0,0),(1,0),(0,1),(1,1)],
            ],
            // T
            [
                [(0,0),(1,0),(2,0),(1,1)],
                [(1,-1),(1,0),(2,0),(1,1)],
                [(1,-1),(0,0),(1,0),(2,0)],
                [(1,-1),(0,0),(1,0),(1,1)],
            ],
            // S
            [
                [(1,0),(2,0),(0,1),(1,1)],
                [(1,-1),(1,0),(2,0),(2,1)],
                [(1,0),(2,0),(0,1),(1,1)],
                [(1,-1),(1,0),(2,0),(2,1)],
            ],
            // Z
            [
                [(0,0),(1,0),(1,1),(2,1)],
                [(2,-1),(1,0),(2,0),(1,1)],
                [(0,0),(1,0),(1,1),(2,1)],
                [(2,-1),(1,0),(2,0),(1,1)],
            ],
            // J
            [
                [(0,0),(0,1),(1,1),(2,1)],
                [(1,-1),(2,-1),(1,0),(1,1)],
                [(0,0),(1,0),(2,0),(2,1)],
                [(1,-1),(1,0),(0,1),(1,1)],
            ],
            // L
            [
                [(2,0),(0,1),(1,1),(2,1)],
                [(1,-1),(1,0),(1,1),(2,1)],
                [(0,0),(1,0),(2,0),(0,1)],
                [(0,-1),(1,-1),(1,0),(1,1)],
            ],
        ];
    }
}
