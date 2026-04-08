namespace TetrisGame.Domain;

public enum PieceType { I, O, T, S, Z, J, L }

public class Piece
{
    public PieceType Type { get; }
    public int Rotation { get; private set; }
    public int Row { get; private set; }
    public int Col { get; private set; }

    // Shapes[type][rotation] = array of (rowOffset, colOffset)
    private static readonly Dictionary<PieceType, (int row, int col)[][]> Shapes =
        new()
        {
            [PieceType.I] =
            [
                [(0,0),(0,1),(0,2),(0,3)],
                [(0,2),(1,2),(2,2),(3,2)],
                [(1,0),(1,1),(1,2),(1,3)],
                [(0,1),(1,1),(2,1),(3,1)],
            ],
            [PieceType.O] =
            [
                [(0,0),(0,1),(1,0),(1,1)],
                [(0,0),(0,1),(1,0),(1,1)],
                [(0,0),(0,1),(1,0),(1,1)],
                [(0,0),(0,1),(1,0),(1,1)],
            ],
            [PieceType.T] =
            [
                [(0,1),(1,0),(1,1),(1,2)],
                [(0,1),(1,1),(1,2),(2,1)],
                [(1,0),(1,1),(1,2),(2,1)],
                [(0,1),(1,0),(1,1),(2,1)],
            ],
            [PieceType.S] =
            [
                [(0,1),(0,2),(1,0),(1,1)],
                [(0,1),(1,1),(1,2),(2,2)],
                [(1,1),(1,2),(2,0),(2,1)],
                [(0,0),(1,0),(1,1),(2,1)],
            ],
            [PieceType.Z] =
            [
                [(0,0),(0,1),(1,1),(1,2)],
                [(0,2),(1,1),(1,2),(2,1)],
                [(1,0),(1,1),(2,1),(2,2)],
                [(0,1),(1,0),(1,1),(2,0)],
            ],
            [PieceType.J] =
            [
                [(0,0),(1,0),(1,1),(1,2)],
                [(0,1),(0,2),(1,1),(2,1)],
                [(1,0),(1,1),(1,2),(2,2)],
                [(0,1),(1,1),(2,0),(2,1)],
            ],
            [PieceType.L] =
            [
                [(0,2),(1,0),(1,1),(1,2)],
                [(0,1),(1,1),(2,1),(2,2)],
                [(1,0),(1,1),(1,2),(2,0)],
                [(0,0),(0,1),(1,1),(2,1)],
            ],
        };

    public Piece(PieceType type, int row = 0, int col = 3)
    {
        Type = type;
        Row = row;
        Col = col;
        Rotation = 0;
    }

    private Piece(PieceType type, int row, int col, int rotation)
    {
        Type = type;
        Row = row;
        Col = col;
        Rotation = rotation;
    }

    public IEnumerable<(int row, int col)> GetCells()
    {
        foreach (var (dr, dc) in Shapes[Type][Rotation])
            yield return (Row + dr, Col + dc);
    }

    public Piece WithOffset(int dRow, int dCol) =>
        new(Type, Row + dRow, Col + dCol, Rotation);

    public Piece Rotated()
    {
        int nextRotation = (Rotation + 1) % 4;
        return new Piece(Type, Row, Col, nextRotation);
    }
}
