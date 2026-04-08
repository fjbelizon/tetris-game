namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public sealed record Cell(int X, int Y);

public sealed class Piece
{
    public TetrominoType Type { get; }
    public int Rotation { get; }
    public int OriginX { get; }
    public int OriginY { get; }

    private static readonly int[][,] Shapes = new int[][,]
    {
        // I
        new int[,] { {0,1},{1,1},{2,1},{3,1} },
        new int[,] { {2,0},{2,1},{2,2},{2,3} },
        new int[,] { {0,2},{1,2},{2,2},{3,2} },
        new int[,] { {1,0},{1,1},{1,2},{1,3} },
        // O
        new int[,] { {1,0},{2,0},{1,1},{2,1} },
        new int[,] { {1,0},{2,0},{1,1},{2,1} },
        new int[,] { {1,0},{2,0},{1,1},{2,1} },
        new int[,] { {1,0},{2,0},{1,1},{2,1} },
        // T
        new int[,] { {1,0},{0,1},{1,1},{2,1} },
        new int[,] { {1,0},{1,1},{2,1},{1,2} },
        new int[,] { {0,1},{1,1},{2,1},{1,2} },
        new int[,] { {1,0},{0,1},{1,1},{1,2} },
        // S
        new int[,] { {1,0},{2,0},{0,1},{1,1} },
        new int[,] { {1,0},{1,1},{2,1},{2,2} },
        new int[,] { {1,0},{2,0},{0,1},{1,1} },
        new int[,] { {1,0},{1,1},{2,1},{2,2} },
        // Z
        new int[,] { {0,0},{1,0},{1,1},{2,1} },
        new int[,] { {2,0},{1,1},{2,1},{1,2} },
        new int[,] { {0,0},{1,0},{1,1},{2,1} },
        new int[,] { {2,0},{1,1},{2,1},{1,2} },
        // J
        new int[,] { {0,0},{0,1},{1,1},{2,1} },
        new int[,] { {1,0},{2,0},{1,1},{1,2} },
        new int[,] { {0,1},{1,1},{2,1},{2,2} },
        new int[,] { {1,0},{1,1},{0,2},{1,2} },
        // L
        new int[,] { {2,0},{0,1},{1,1},{2,1} },
        new int[,] { {1,0},{1,1},{1,2},{2,2} },
        new int[,] { {0,1},{1,1},{2,1},{0,2} },
        new int[,] { {0,0},{1,0},{1,1},{1,2} },
    };

    public Piece(TetrominoType type, int rotation = 0, int originX = 0, int originY = 0)
    {
        Type = type;
        Rotation = ((rotation % 4) + 4) % 4;
        OriginX = originX;
        OriginY = originY;
    }

    public IEnumerable<Cell> GetAbsoluteCells()
    {
        int shapeIndex = (int)Type * 4 + Rotation;
        var offsets = Shapes[shapeIndex];
        for (int i = 0; i < offsets.GetLength(0); i++)
            yield return new Cell(OriginX + offsets[i, 0], OriginY + offsets[i, 1]);
    }

    public Piece Move(int dx, int dy) => new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() => new(Type, (Rotation + 1) % 4, OriginX, OriginY);
}
