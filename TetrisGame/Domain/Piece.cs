namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public sealed record Piece(TetrominoType Type, int Rotation, int OriginX, int OriginY)
{
    // Offsets[type][rotation] -> list of (dx, dy)
    private static readonly (int dx, int dy)[][][] Offsets = BuildOffsets();

    public (int x, int y)[] GetAbsoluteCells()
    {
        var offsets = Offsets[(int)Type][Rotation % 4];
        var cells = new (int x, int y)[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
            cells[i] = (OriginX + offsets[i].dx, OriginY + offsets[i].dy);
        return cells;
    }

    public Piece Move(int dx, int dy) => this with { OriginX = OriginX + dx, OriginY = OriginY + dy };

    public Piece RotateClockwise() => this with { Rotation = (Rotation + 1) % 4 };

    private static (int dx, int dy)[][][] BuildOffsets()
    {
        // Each piece has 4 rotations; each rotation has 4 cells.
        // Coordinates are relative to origin (spawn anchor).
        var offsets = new (int dx, int dy)[7][][];

        // I piece
        offsets[(int)TetrominoType.I] =
        [
            [(0,0),(1,0),(2,0),(3,0)],  // 0
            [(2,0),(2,1),(2,2),(2,3)],  // 1  (right)
            [(0,2),(1,2),(2,2),(3,2)],  // 2
            [(1,0),(1,1),(1,2),(1,3)],  // 3
        ];

        // O piece
        offsets[(int)TetrominoType.O] =
        [
            [(0,0),(1,0),(0,1),(1,1)],
            [(0,0),(1,0),(0,1),(1,1)],
            [(0,0),(1,0),(0,1),(1,1)],
            [(0,0),(1,0),(0,1),(1,1)],
        ];

        // T piece
        offsets[(int)TetrominoType.T] =
        [
            [(1,0),(0,1),(1,1),(2,1)],  // 0: T pointing up
            [(0,0),(0,1),(1,1),(0,2)],  // 1: T pointing right
            [(0,0),(1,0),(2,0),(1,1)],  // 2: T pointing down
            [(1,0),(0,1),(1,1),(1,2)],  // 3: T pointing left
        ];

        // S piece
        offsets[(int)TetrominoType.S] =
        [
            [(1,0),(2,0),(0,1),(1,1)],
            [(0,0),(0,1),(1,1),(1,2)],
            [(1,0),(2,0),(0,1),(1,1)],
            [(0,0),(0,1),(1,1),(1,2)],
        ];

        // Z piece
        offsets[(int)TetrominoType.Z] =
        [
            [(0,0),(1,0),(1,1),(2,1)],
            [(1,0),(0,1),(1,1),(0,2)],
            [(0,0),(1,0),(1,1),(2,1)],
            [(1,0),(0,1),(1,1),(0,2)],
        ];

        // J piece
        offsets[(int)TetrominoType.J] =
        [
            [(0,0),(0,1),(1,1),(2,1)],  // 0
            [(0,0),(1,0),(0,1),(0,2)],  // 1
            [(0,0),(1,0),(2,0),(2,1)],  // 2
            [(1,0),(1,1),(0,2),(1,2)],  // 3
        ];

        // L piece
        offsets[(int)TetrominoType.L] =
        [
            [(2,0),(0,1),(1,1),(2,1)],  // 0
            [(0,0),(0,1),(0,2),(1,2)],  // 1
            [(0,0),(1,0),(2,0),(0,1)],  // 2
            [(0,0),(1,0),(1,1),(1,2)],  // 3
        ];

        return offsets;
    }
}
