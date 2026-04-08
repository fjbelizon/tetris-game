namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public sealed class Piece
{
    private static readonly (int dx, int dy)[][][] Shapes = BuildShapes();

    public TetrominoType Type { get; }
    public int Rotation { get; }
    public int OriginX { get; }
    public int OriginY { get; }

    public Piece(TetrominoType type, int rotation, int originX, int originY)
    {
        Type = type;
        Rotation = rotation % 4;
        OriginX = originX;
        OriginY = originY;
    }

    public Piece Move(int dx, int dy) => new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() => new(Type, (Rotation + 1) % 4, OriginX, OriginY);

    public IEnumerable<(int x, int y)> GetAbsoluteCells()
    {
        foreach (var (dx, dy) in Shapes[(int)Type][Rotation])
            yield return (OriginX + dx, OriginY + dy);
    }

    private static (int dx, int dy)[][][] BuildShapes()
    {
        // Shapes[type][rotation] = list of offsets from origin
        var shapes = new (int, int)[7][][];

        // I — horizontal/vertical
        shapes[(int)TetrominoType.I] =
        [
            [(0, 0), (1, 0), (2, 0), (3, 0)],  // rotation 0: horizontal
            [(2, -1), (2, 0), (2, 1), (2, 2)],  // rotation 1: vertical
            [(0, 1), (1, 1), (2, 1), (3, 1)],  // rotation 2: horizontal flipped
            [(1, -1), (1, 0), (1, 1), (1, 2)],  // rotation 3: vertical flipped
        ];

        // O — square, all rotations identical
        shapes[(int)TetrominoType.O] =
        [
            [(0, 0), (1, 0), (0, 1), (1, 1)],
            [(0, 0), (1, 0), (0, 1), (1, 1)],
            [(0, 0), (1, 0), (0, 1), (1, 1)],
            [(0, 0), (1, 0), (0, 1), (1, 1)],
        ];

        // T
        shapes[(int)TetrominoType.T] =
        [
            [(0, 0), (1, 0), (2, 0), (1, 1)],  // rotation 0
            [(1, 0), (0, 1), (1, 1), (1, 2)],  // rotation 1
            [(1, 0), (0, 1), (1, 1), (2, 1)],  // rotation 2
            [(0, 0), (0, 1), (1, 1), (0, 2)],  // rotation 3
        ];

        // S
        shapes[(int)TetrominoType.S] =
        [
            [(1, 0), (2, 0), (0, 1), (1, 1)],
            [(0, 0), (0, 1), (1, 1), (1, 2)],
            [(1, 0), (2, 0), (0, 1), (1, 1)],
            [(0, 0), (0, 1), (1, 1), (1, 2)],
        ];

        // Z
        shapes[(int)TetrominoType.Z] =
        [
            [(0, 0), (1, 0), (1, 1), (2, 1)],
            [(1, 0), (0, 1), (1, 1), (0, 2)],
            [(0, 0), (1, 0), (1, 1), (2, 1)],
            [(1, 0), (0, 1), (1, 1), (0, 2)],
        ];

        // J
        shapes[(int)TetrominoType.J] =
        [
            [(0, 0), (0, 1), (1, 1), (2, 1)],
            [(0, 0), (1, 0), (0, 1), (0, 2)],
            [(0, 0), (1, 0), (2, 0), (2, 1)],
            [(1, 0), (1, 1), (0, 2), (1, 2)],
        ];

        // L
        shapes[(int)TetrominoType.L] =
        [
            [(2, 0), (0, 1), (1, 1), (2, 1)],
            [(0, 0), (0, 1), (0, 2), (1, 2)],
            [(0, 0), (1, 0), (2, 0), (0, 1)],
            [(0, 0), (1, 0), (1, 1), (1, 2)],
        ];

        return shapes;
    }
}
