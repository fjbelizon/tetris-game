namespace TetrisGame.Domain;

public enum PieceType { I, O, T, S, Z, J, L }

public readonly record struct Cell(int X, int Y);

public sealed class Piece
{
    private static readonly Dictionary<PieceType, (int dx, int dy)[][]> Shapes =
        new()
        {
            [PieceType.I] =
            [
                [(-1, 0), (0, 0), (1, 0), (2, 0)],
                [(0, -1), (0, 0), (0, 1), (0, 2)],
                [(-1, 0), (0, 0), (1, 0), (2, 0)],
                [(0, -1), (0, 0), (0, 1), (0, 2)],
            ],
            [PieceType.O] =
            [
                [(0, 0), (1, 0), (0, 1), (1, 1)],
                [(0, 0), (1, 0), (0, 1), (1, 1)],
                [(0, 0), (1, 0), (0, 1), (1, 1)],
                [(0, 0), (1, 0), (0, 1), (1, 1)],
            ],
            [PieceType.T] =
            [
                [(0, -1), (-1, 0), (0, 0), (1, 0)],
                [(0, -1), (0, 0), (1, 0), (0, 1)],
                [(-1, 0), (0, 0), (1, 0), (0, 1)],
                [(0, -1), (-1, 0), (0, 0), (0, 1)],
            ],
            [PieceType.S] =
            [
                [(0, -1), (1, -1), (-1, 0), (0, 0)],
                [(0, -1), (0, 0), (1, 0), (1, 1)],
                [(0, 0), (1, 0), (-1, 1), (0, 1)],
                [(-1, -1), (-1, 0), (0, 0), (0, 1)],
            ],
            [PieceType.Z] =
            [
                [(-1, -1), (0, -1), (0, 0), (1, 0)],
                [(1, -1), (0, 0), (1, 0), (0, 1)],
                [(-1, 0), (0, 0), (0, 1), (1, 1)],
                [(0, -1), (-1, 0), (0, 0), (-1, 1)],
            ],
            [PieceType.J] =
            [
                [(-1, -1), (-1, 0), (0, 0), (1, 0)],
                [(0, -1), (1, -1), (0, 0), (0, 1)],
                [(-1, 0), (0, 0), (1, 0), (1, 1)],
                [(0, -1), (0, 0), (-1, 1), (0, 1)],
            ],
            [PieceType.L] =
            [
                [(1, -1), (-1, 0), (0, 0), (1, 0)],
                [(0, -1), (0, 0), (0, 1), (1, 1)],
                [(-1, 0), (0, 0), (1, 0), (-1, 1)],
                [(-1, -1), (0, -1), (0, 0), (0, 1)],
            ],
        };

    public PieceType Type { get; }
    public int Rotation { get; }
    public int OriginX { get; }
    public int OriginY { get; }

    public Piece(PieceType type, int rotation, int originX, int originY)
    {
        Type = type;
        Rotation = ((rotation % 4) + 4) % 4;
        OriginX = originX;
        OriginY = originY;
    }

    public IReadOnlyList<Cell> GetAbsoluteCells()
    {
        var offsets = Shapes[Type][Rotation];
        var cells = new Cell[offsets.Length];
        for (var i = 0; i < offsets.Length; i++)
            cells[i] = new Cell(OriginX + offsets[i].dx, OriginY + offsets[i].dy);
        return cells;
    }

    public Piece Move(int dx, int dy) =>
        new(Type, Rotation, OriginX + dx, OriginY + dy);

    public Piece RotateClockwise() =>
        new(Type, (Rotation + 1) % 4, OriginX, OriginY);
}
