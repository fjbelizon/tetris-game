namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public sealed class Piece
{
    // Offsets for each tetromino type and rotation (0-3)
    // Each offset is (dx, dy) from origin
    private static readonly (int dx, int dy)[,][] Shapes = BuildShapes();

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

    public (int x, int y)[] GetAbsoluteCells()
    {
        var offsets = Shapes[(int)Type, Rotation];
        var cells = new (int x, int y)[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
            cells[i] = (OriginX + offsets[i].dx, OriginY + offsets[i].dy);
        return cells;
    }

    public Piece Move(int dx, int dy) =>
        new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() =>
        new(Type, (Rotation + 1) % 4, OriginX, OriginY);

    private static (int dx, int dy)[,][] BuildShapes()
    {
        int types = 7;
        int rotations = 4;
        var shapes = new (int dx, int dy)[types, rotations][];

        // I piece
        shapes[(int)TetrominoType.I, 0] = [(0, 0), (1, 0), (2, 0), (3, 0)];
        shapes[(int)TetrominoType.I, 1] = [(0, 0), (0, 1), (0, 2), (0, 3)];
        shapes[(int)TetrominoType.I, 2] = [(0, 0), (1, 0), (2, 0), (3, 0)];
        shapes[(int)TetrominoType.I, 3] = [(0, 0), (0, 1), (0, 2), (0, 3)];

        // O piece (rotation is idempotent)
        shapes[(int)TetrominoType.O, 0] = [(0, 0), (1, 0), (0, 1), (1, 1)];
        shapes[(int)TetrominoType.O, 1] = [(0, 0), (1, 0), (0, 1), (1, 1)];
        shapes[(int)TetrominoType.O, 2] = [(0, 0), (1, 0), (0, 1), (1, 1)];
        shapes[(int)TetrominoType.O, 3] = [(0, 0), (1, 0), (0, 1), (1, 1)];

        // T piece
        shapes[(int)TetrominoType.T, 0] = [(0, 0), (1, 0), (2, 0), (1, 1)];
        shapes[(int)TetrominoType.T, 1] = [(0, 0), (0, 1), (0, 2), (-1, 1)];
        shapes[(int)TetrominoType.T, 2] = [(0, 0), (1, 0), (2, 0), (1, -1)];
        shapes[(int)TetrominoType.T, 3] = [(0, 0), (0, 1), (0, 2), (1, 1)];

        // S piece
        shapes[(int)TetrominoType.S, 0] = [(0, 1), (1, 1), (1, 0), (2, 0)];
        shapes[(int)TetrominoType.S, 1] = [(0, 0), (0, 1), (1, 1), (1, 2)];
        shapes[(int)TetrominoType.S, 2] = [(0, 1), (1, 1), (1, 0), (2, 0)];
        shapes[(int)TetrominoType.S, 3] = [(0, 0), (0, 1), (1, 1), (1, 2)];

        // Z piece
        shapes[(int)TetrominoType.Z, 0] = [(0, 0), (1, 0), (1, 1), (2, 1)];
        shapes[(int)TetrominoType.Z, 1] = [(0, 1), (0, 2), (1, 0), (1, 1)];
        shapes[(int)TetrominoType.Z, 2] = [(0, 0), (1, 0), (1, 1), (2, 1)];
        shapes[(int)TetrominoType.Z, 3] = [(0, 1), (0, 2), (1, 0), (1, 1)];

        // J piece
        shapes[(int)TetrominoType.J, 0] = [(0, 0), (0, 1), (1, 1), (2, 1)];
        shapes[(int)TetrominoType.J, 1] = [(0, 0), (1, 0), (0, 1), (0, 2)];
        shapes[(int)TetrominoType.J, 2] = [(0, 0), (1, 0), (2, 0), (2, 1)];
        shapes[(int)TetrominoType.J, 3] = [(0, 2), (1, 0), (1, 1), (1, 2)];

        // L piece
        shapes[(int)TetrominoType.L, 0] = [(2, 0), (0, 1), (1, 1), (2, 1)];
        shapes[(int)TetrominoType.L, 1] = [(0, 0), (0, 1), (0, 2), (1, 2)];
        shapes[(int)TetrominoType.L, 2] = [(0, 0), (1, 0), (2, 0), (0, 1)];
        shapes[(int)TetrominoType.L, 3] = [(0, 0), (1, 0), (1, 1), (1, 2)];

        return shapes;
    }
}
