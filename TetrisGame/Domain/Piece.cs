namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public class Piece
{
    // Rotation offsets: [type][rotation][cell] -> (dx, dy)
    private static readonly (int dx, int dy)[,,] Shapes = BuildShapes();

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

    public IEnumerable<(int x, int y)> GetAbsoluteCells()
    {
        int typeIndex = (int)Type;
        for (int i = 0; i < 4; i++)
        {
            yield return (OriginX + Shapes[typeIndex, Rotation, i].dx,
                          OriginY + Shapes[typeIndex, Rotation, i].dy);
        }
    }

    public Piece Move(int dx, int dy) => new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() => new(Type, (Rotation + 1) % 4, OriginX, OriginY);

    private static (int dx, int dy)[,,] BuildShapes()
    {
        // Order: I, O, T, S, Z, J, L  (7 types, 4 rotations, 4 cells)
        var s = new (int dx, int dy)[7, 4, 4];

        // I - pivot at second cell from left on horizontal
        // Rotation 0: horizontal  [ ][ ][X][ ]
        //             row 0:      -1, 0, 1, 2  (y=0)
        s[(int)TetrominoType.I, 0, 0] = (-1, 0);
        s[(int)TetrominoType.I, 0, 1] = (0, 0);
        s[(int)TetrominoType.I, 0, 2] = (1, 0);
        s[(int)TetrominoType.I, 0, 3] = (2, 0);
        // Rotation 1: vertical
        s[(int)TetrominoType.I, 1, 0] = (0, -1);
        s[(int)TetrominoType.I, 1, 1] = (0, 0);
        s[(int)TetrominoType.I, 1, 2] = (0, 1);
        s[(int)TetrominoType.I, 1, 3] = (0, 2);
        // Rotation 2: horizontal (same as 0 but mirrored)
        s[(int)TetrominoType.I, 2, 0] = (-1, 0);
        s[(int)TetrominoType.I, 2, 1] = (0, 0);
        s[(int)TetrominoType.I, 2, 2] = (1, 0);
        s[(int)TetrominoType.I, 2, 3] = (2, 0);
        // Rotation 3: vertical (same as 1)
        s[(int)TetrominoType.I, 3, 0] = (0, -1);
        s[(int)TetrominoType.I, 3, 1] = (0, 0);
        s[(int)TetrominoType.I, 3, 2] = (0, 1);
        s[(int)TetrominoType.I, 3, 3] = (0, 2);

        // O - 2x2 square, rotation is idempotent
        for (int r = 0; r < 4; r++)
        {
            s[(int)TetrominoType.O, r, 0] = (0, 0);
            s[(int)TetrominoType.O, r, 1] = (1, 0);
            s[(int)TetrominoType.O, r, 2] = (0, 1);
            s[(int)TetrominoType.O, r, 3] = (1, 1);
        }

        // T - T shape
        // Rotation 0: T up  (flat bottom, point up)
        s[(int)TetrominoType.T, 0, 0] = (-1, 0);
        s[(int)TetrominoType.T, 0, 1] = (0, 0);
        s[(int)TetrominoType.T, 0, 2] = (1, 0);
        s[(int)TetrominoType.T, 0, 3] = (0, -1);
        // Rotation 1: T right
        s[(int)TetrominoType.T, 1, 0] = (0, -1);
        s[(int)TetrominoType.T, 1, 1] = (0, 0);
        s[(int)TetrominoType.T, 1, 2] = (0, 1);
        s[(int)TetrominoType.T, 1, 3] = (1, 0);
        // Rotation 2: T down
        s[(int)TetrominoType.T, 2, 0] = (-1, 0);
        s[(int)TetrominoType.T, 2, 1] = (0, 0);
        s[(int)TetrominoType.T, 2, 2] = (1, 0);
        s[(int)TetrominoType.T, 2, 3] = (0, 1);
        // Rotation 3: T left
        s[(int)TetrominoType.T, 3, 0] = (0, -1);
        s[(int)TetrominoType.T, 3, 1] = (0, 0);
        s[(int)TetrominoType.T, 3, 2] = (0, 1);
        s[(int)TetrominoType.T, 3, 3] = (-1, 0);

        // S piece
        // Rotation 0:  _[X][X]
        //             [X][X]
        s[(int)TetrominoType.S, 0, 0] = (0, 0);
        s[(int)TetrominoType.S, 0, 1] = (1, 0);
        s[(int)TetrominoType.S, 0, 2] = (-1, 1);
        s[(int)TetrominoType.S, 0, 3] = (0, 1);
        // Rotation 1: vertical
        s[(int)TetrominoType.S, 1, 0] = (0, -1);
        s[(int)TetrominoType.S, 1, 1] = (0, 0);
        s[(int)TetrominoType.S, 1, 2] = (1, 0);
        s[(int)TetrominoType.S, 1, 3] = (1, 1);
        // Rotation 2: same as 0
        s[(int)TetrominoType.S, 2, 0] = (0, 0);
        s[(int)TetrominoType.S, 2, 1] = (1, 0);
        s[(int)TetrominoType.S, 2, 2] = (-1, 1);
        s[(int)TetrominoType.S, 2, 3] = (0, 1);
        // Rotation 3: same as 1
        s[(int)TetrominoType.S, 3, 0] = (0, -1);
        s[(int)TetrominoType.S, 3, 1] = (0, 0);
        s[(int)TetrominoType.S, 3, 2] = (1, 0);
        s[(int)TetrominoType.S, 3, 3] = (1, 1);

        // Z piece
        // Rotation 0: [X][X]
        //              _[X][X]
        s[(int)TetrominoType.Z, 0, 0] = (-1, 0);
        s[(int)TetrominoType.Z, 0, 1] = (0, 0);
        s[(int)TetrominoType.Z, 0, 2] = (0, 1);
        s[(int)TetrominoType.Z, 0, 3] = (1, 1);
        // Rotation 1: vertical
        s[(int)TetrominoType.Z, 1, 0] = (1, -1);
        s[(int)TetrominoType.Z, 1, 1] = (0, 0);
        s[(int)TetrominoType.Z, 1, 2] = (1, 0);
        s[(int)TetrominoType.Z, 1, 3] = (0, 1);
        // Rotation 2: same as 0
        s[(int)TetrominoType.Z, 2, 0] = (-1, 0);
        s[(int)TetrominoType.Z, 2, 1] = (0, 0);
        s[(int)TetrominoType.Z, 2, 2] = (0, 1);
        s[(int)TetrominoType.Z, 2, 3] = (1, 1);
        // Rotation 3: same as 1
        s[(int)TetrominoType.Z, 3, 0] = (1, -1);
        s[(int)TetrominoType.Z, 3, 1] = (0, 0);
        s[(int)TetrominoType.Z, 3, 2] = (1, 0);
        s[(int)TetrominoType.Z, 3, 3] = (0, 1);

        // J piece
        // Rotation 0: [X]
        //             [X][X][X]
        s[(int)TetrominoType.J, 0, 0] = (-1, 0);
        s[(int)TetrominoType.J, 0, 1] = (0, 0);
        s[(int)TetrominoType.J, 0, 2] = (1, 0);
        s[(int)TetrominoType.J, 0, 3] = (-1, -1);
        // Rotation 1: [X][X]
        //             [X]
        //             [X]
        s[(int)TetrominoType.J, 1, 0] = (0, -1);
        s[(int)TetrominoType.J, 1, 1] = (0, 0);
        s[(int)TetrominoType.J, 1, 2] = (0, 1);
        s[(int)TetrominoType.J, 1, 3] = (1, -1);
        // Rotation 2: [X][X][X]
        //                    [X]
        s[(int)TetrominoType.J, 2, 0] = (-1, 0);
        s[(int)TetrominoType.J, 2, 1] = (0, 0);
        s[(int)TetrominoType.J, 2, 2] = (1, 0);
        s[(int)TetrominoType.J, 2, 3] = (1, 1);
        // Rotation 3:    [X]
        //                [X]
        //             [X][X]
        s[(int)TetrominoType.J, 3, 0] = (0, -1);
        s[(int)TetrominoType.J, 3, 1] = (0, 0);
        s[(int)TetrominoType.J, 3, 2] = (0, 1);
        s[(int)TetrominoType.J, 3, 3] = (-1, 1);

        // L piece
        // Rotation 0:        [X]
        //             [X][X][X]
        s[(int)TetrominoType.L, 0, 0] = (-1, 0);
        s[(int)TetrominoType.L, 0, 1] = (0, 0);
        s[(int)TetrominoType.L, 0, 2] = (1, 0);
        s[(int)TetrominoType.L, 0, 3] = (1, -1);
        // Rotation 1: [X]
        //             [X]
        //             [X][X]
        s[(int)TetrominoType.L, 1, 0] = (0, -1);
        s[(int)TetrominoType.L, 1, 1] = (0, 0);
        s[(int)TetrominoType.L, 1, 2] = (0, 1);
        s[(int)TetrominoType.L, 1, 3] = (-1, 1);
        // Rotation 2: [X][X][X]
        //             [X]
        s[(int)TetrominoType.L, 2, 0] = (-1, 0);
        s[(int)TetrominoType.L, 2, 1] = (0, 0);
        s[(int)TetrominoType.L, 2, 2] = (1, 0);
        s[(int)TetrominoType.L, 2, 3] = (-1, 1);
        // Rotation 3: [X][X]
        //                [X]
        //                [X]
        s[(int)TetrominoType.L, 3, 0] = (0, -1);
        s[(int)TetrominoType.L, 3, 1] = (0, 0);
        s[(int)TetrominoType.L, 3, 2] = (0, 1);
        s[(int)TetrominoType.L, 3, 3] = (1, -1);

        return s;
    }
}
