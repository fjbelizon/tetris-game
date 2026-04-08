namespace TetrisGame.Domain;

public enum TetrominoType { I, O, T, S, Z, J, L }

public sealed class Piece
{
    // Base offsets (dx, dy) at rotation 0 for each piece type.
    // Origin is the pivot/spawn point. Y increases downward.
    private static readonly Dictionary<TetrominoType, (int dx, int dy)[]> BaseOffsets = new()
    {
        [TetrominoType.I] = [(-1, 0), (0, 0), (1, 0), (2, 0)],
        [TetrominoType.O] = [(0, 0), (1, 0), (0, -1), (1, -1)],
        [TetrominoType.T] = [(-1, 0), (0, 0), (1, 0), (0, -1)],
        [TetrominoType.S] = [(-1, 0), (0, 0), (0, -1), (1, -1)],
        [TetrominoType.Z] = [(-1, -1), (0, -1), (0, 0), (1, 0)],
        [TetrominoType.J] = [(-1, -1), (-1, 0), (0, 0), (1, 0)],
        [TetrominoType.L] = [(1, -1), (-1, 0), (0, 0), (1, 0)],
    };

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

    // Clockwise rotation in screen coords (Y-down): (dx, dy) -> (-dy, dx)
    private static (int dx, int dy)[] ComputeOffsets(TetrominoType type, int rotation)
    {
        var offsets = (BaseOffsets[type].ToArray());
        if (type == TetrominoType.O)
            return offsets;

        int steps = rotation & 3;
        for (int r = 0; r < steps; r++)
        {
            for (int i = 0; i < offsets.Length; i++)
            {
                var (dx, dy) = offsets[i];
                offsets[i] = (-dy, dx);
            }
        }
        return offsets;
    }

    public (int x, int y)[] GetAbsoluteCells()
    {
        var offsets = ComputeOffsets(Type, Rotation);
        return offsets.Select(o => (OriginX + o.dx, OriginY + o.dy)).ToArray();
    }

    public Piece Move(int dx, int dy) => new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() => new(Type, (Rotation + 1) & 3, OriginX, OriginY);
}
