namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public sealed record Piece(TetrominoType Type, int Rotation, int OriginX, int OriginY)
{
    // Shape offsets for each tetromino type and rotation (0-3).
    // Each offset is (dx, dy) relative to the origin.
    private static readonly Dictionary<TetrominoType, (int dx, int dy)[][]> Shapes = new()
    {
        [TetrominoType.I] = [
            [(0,0),(1,0),(2,0),(3,0)],  // 0
            [(2,0),(2,1),(2,2),(2,3)],  // 1
            [(0,2),(1,2),(2,2),(3,2)],  // 2
            [(1,0),(1,1),(1,2),(1,3)],  // 3
        ],
        [TetrominoType.O] = [
            [(0,0),(1,0),(0,1),(1,1)],
            [(0,0),(1,0),(0,1),(1,1)],
            [(0,0),(1,0),(0,1),(1,1)],
            [(0,0),(1,0),(0,1),(1,1)],
        ],
        [TetrominoType.T] = [
            [(0,0),(1,0),(2,0),(1,1)],  // 0
            [(1,0),(1,1),(1,2),(0,1)],  // 1
            [(0,1),(1,1),(2,1),(1,0)],  // 2
            [(1,0),(1,1),(1,2),(2,1)],  // 3
        ],
        [TetrominoType.S] = [
            [(1,0),(2,0),(0,1),(1,1)],
            [(1,0),(1,1),(2,1),(2,2)],
            [(1,1),(2,1),(0,2),(1,2)],
            [(0,0),(0,1),(1,1),(1,2)],
        ],
        [TetrominoType.Z] = [
            [(0,0),(1,0),(1,1),(2,1)],
            [(2,0),(1,1),(2,1),(1,2)],
            [(0,1),(1,1),(1,2),(2,2)],
            [(1,0),(0,1),(1,1),(0,2)],
        ],
        [TetrominoType.J] = [
            [(0,0),(0,1),(1,1),(2,1)],  // 0
            [(1,0),(2,0),(1,1),(1,2)],  // 1
            [(0,1),(1,1),(2,1),(2,2)],  // 2
            [(1,0),(1,1),(0,2),(1,2)],  // 3
        ],
        [TetrominoType.L] = [
            [(2,0),(0,1),(1,1),(2,1)],  // 0
            [(1,0),(1,1),(1,2),(2,2)],  // 1
            [(0,1),(1,1),(2,1),(0,2)],  // 2
            [(0,0),(1,0),(1,1),(1,2)],  // 3
        ],
    };

    public IReadOnlyList<(int X, int Y)> GetAbsoluteCells()
    {
        var offsets = Shapes[Type][Rotation];
        return [.. offsets.Select(o => (OriginX + o.dx, OriginY + o.dy))];
    }

    public Piece Move(int dx, int dy) => this with { OriginX = OriginX + dx, OriginY = OriginY + dy };

    public Piece RotateClockwise() => this with { Rotation = (Rotation + 1) % 4 };
}
