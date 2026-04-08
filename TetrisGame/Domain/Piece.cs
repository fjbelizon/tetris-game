namespace TetrisGame.Domain;

/// <summary>
/// Immutable representation of a falling tetromino.
/// Positions are (col, row) offsets from the piece origin.
/// </summary>
public sealed class Piece
{
    // Shapes[typeIndex][rotation] = array of (dx, dy) offsets from origin
    private static readonly (int dx, int dy)[][][] Shapes = BuildShapes();

    public TetrominoType Type { get; }
    public int Rotation { get; }
    public int OriginX { get; }
    public int OriginY { get; }

    public Piece(TetrominoType type, int rotation, int originX, int originY)
    {
        Type = type;
        Rotation = ((rotation % 4) + 4) % 4;
        OriginX = originX;
        OriginY = originY;
    }

    /// <summary>Creates a new piece at its standard spawn position (top-center).</summary>
    public static Piece Spawn(TetrominoType type) =>
        type == TetrominoType.O
            ? new Piece(type, 0, 4, 0)
            : new Piece(type, 0, 3, 0);

    /// <summary>Returns absolute board coordinates occupied by this piece.</summary>
    public IEnumerable<(int x, int y)> GetAbsoluteCells()
    {
        foreach (var (dx, dy) in Shapes[(int)Type][Rotation])
            yield return (OriginX + dx, OriginY + dy);
    }

    public Piece Move(int dx, int dy) =>
        new Piece(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() =>
        new Piece(Type, (Rotation + 1) % 4, OriginX, OriginY);

    // -------------------------------------------------------------------------
    // Shape tables  (col, row) offsets from origin, rotation 0 = spawn
    // -------------------------------------------------------------------------
    private static (int dx, int dy)[][][] BuildShapes()
    {
        var s = new (int dx, int dy)[7][][];

        // I piece – 4-wide horizontal bar
        s[(int)TetrominoType.I] =
        [
            [(0, 0), (1, 0), (2, 0), (3, 0)],   // R0 – horizontal top
            [(2, 0), (2, 1), (2, 2), (2, 3)],   // R1 – vertical right
            [(0, 2), (1, 2), (2, 2), (3, 2)],   // R2 – horizontal low
            [(1, 0), (1, 1), (1, 2), (1, 3)],   // R3 – vertical left
        ];

        // O piece – 2 × 2 square (spawn at col 4 to centre it)
        s[(int)TetrominoType.O] =
        [
            [(0, 0), (1, 0), (0, 1), (1, 1)],
            [(0, 0), (1, 0), (0, 1), (1, 1)],
            [(0, 0), (1, 0), (0, 1), (1, 1)],
            [(0, 0), (1, 0), (0, 1), (1, 1)],
        ];

        // T piece
        s[(int)TetrominoType.T] =
        [
            [(1, 0), (0, 1), (1, 1), (2, 1)],   // R0 – bump up   (spawn)
            [(0, 0), (0, 1), (1, 1), (0, 2)],   // R1 – bump right
            [(0, 1), (1, 1), (2, 1), (1, 2)],   // R2 – bump down
            [(1, 0), (0, 1), (1, 1), (1, 2)],   // R3 – bump left
        ];

        // S piece
        s[(int)TetrominoType.S] =
        [
            [(1, 0), (2, 0), (0, 1), (1, 1)],   // R0
            [(0, 0), (0, 1), (1, 1), (1, 2)],   // R1
            [(1, 1), (2, 1), (0, 2), (1, 2)],   // R2
            [(0, 0), (0, 1), (1, 1), (1, 2)],   // R3 (= R1)
        ];

        // Z piece
        s[(int)TetrominoType.Z] =
        [
            [(0, 0), (1, 0), (1, 1), (2, 1)],   // R0
            [(1, 0), (0, 1), (1, 1), (0, 2)],   // R1
            [(0, 1), (1, 1), (1, 2), (2, 2)],   // R2
            [(1, 0), (0, 1), (1, 1), (0, 2)],   // R3 (= R1)
        ];

        // J piece
        s[(int)TetrominoType.J] =
        [
            [(0, 0), (0, 1), (1, 1), (2, 1)],   // R0 – spawn
            [(0, 0), (1, 0), (0, 1), (0, 2)],   // R1
            [(0, 1), (1, 1), (2, 1), (2, 2)],   // R2
            [(1, 0), (1, 1), (0, 2), (1, 2)],   // R3
        ];

        // L piece
        s[(int)TetrominoType.L] =
        [
            [(2, 0), (0, 1), (1, 1), (2, 1)],   // R0 – spawn
            [(0, 0), (0, 1), (0, 2), (1, 2)],   // R1
            [(0, 1), (1, 1), (2, 1), (0, 2)],   // R2
            [(0, 0), (1, 0), (1, 1), (1, 2)],   // R3
        ];

        return s;
    }
}
