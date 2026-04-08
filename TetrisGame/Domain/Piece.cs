namespace TetrisGame.Domain;

/// <summary>
/// Immutable representation of a tetromino on the board.
/// </summary>
public sealed record Piece
{
    private static readonly Dictionary<TetrominoType, (int dx, int dy)[][]> ShapeTable =
        new()
        {
            [TetrominoType.I] =
            [
                [(0, 1), (1, 1), (2, 1), (3, 1)],
                [(2, 0), (2, 1), (2, 2), (2, 3)],
                [(0, 2), (1, 2), (2, 2), (3, 2)],
                [(1, 0), (1, 1), (1, 2), (1, 3)],
            ],
            [TetrominoType.O] =
            [
                [(1, 0), (2, 0), (1, 1), (2, 1)],
                [(1, 0), (2, 0), (1, 1), (2, 1)],
                [(1, 0), (2, 0), (1, 1), (2, 1)],
                [(1, 0), (2, 0), (1, 1), (2, 1)],
            ],
            [TetrominoType.T] =
            [
                [(1, 0), (0, 1), (1, 1), (2, 1)],
                [(1, 0), (1, 1), (2, 1), (1, 2)],
                [(0, 1), (1, 1), (2, 1), (1, 2)],
                [(1, 0), (0, 1), (1, 1), (1, 2)],
            ],
            [TetrominoType.S] =
            [
                [(1, 0), (2, 0), (0, 1), (1, 1)],
                [(1, 0), (1, 1), (2, 1), (2, 2)],
                [(1, 1), (2, 1), (0, 2), (1, 2)],
                [(0, 0), (0, 1), (1, 1), (1, 2)],
            ],
            [TetrominoType.Z] =
            [
                [(0, 0), (1, 0), (1, 1), (2, 1)],
                [(2, 0), (1, 1), (2, 1), (1, 2)],
                [(0, 1), (1, 1), (1, 2), (2, 2)],
                [(1, 0), (0, 1), (1, 1), (0, 2)],
            ],
            [TetrominoType.J] =
            [
                [(0, 0), (0, 1), (1, 1), (2, 1)],
                [(1, 0), (2, 0), (1, 1), (1, 2)],
                [(0, 1), (1, 1), (2, 1), (2, 2)],
                [(1, 0), (1, 1), (0, 2), (1, 2)],
            ],
            [TetrominoType.L] =
            [
                [(2, 0), (0, 1), (1, 1), (2, 1)],
                [(1, 0), (1, 1), (1, 2), (2, 2)],
                [(0, 1), (1, 1), (2, 1), (0, 2)],
                [(0, 0), (1, 0), (1, 1), (1, 2)],
            ],
        };

    public TetrominoType Type { get; init; }
    public int Rotation { get; init; }
    public int OriginX { get; init; }
    public int OriginY { get; init; }

    public IReadOnlyList<(int dx, int dy)> ShapeOffsets =>
        ShapeTable[Type][Rotation];

    public Piece(TetrominoType type, int rotation = 0, int originX = 0, int originY = 0)
    {
        Type = type;
        // Normalize to [0,3] range, handling negative inputs
        Rotation = ((rotation % 4) + 4) % 4;
        OriginX = originX;
        OriginY = originY;
    }

    public Piece Move(int dx, int dy) =>
        this with { OriginX = OriginX + dx, OriginY = OriginY + dy };

    public Piece RotateClockwise() =>
        this with { Rotation = (Rotation + 1) % 4 };

    public IReadOnlyList<(int X, int Y)> GetAbsoluteCells() =>
        ShapeOffsets
            .Select(offset => (OriginX + offset.dx, OriginY + offset.dy))
            .ToList();
}
