namespace TetrisGame.Domain;

public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    private readonly bool[,] _settled = new bool[Height, Width];

    public bool IsInside(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

    public bool IsOccupied(int x, int y) => IsInside(x, y) && _settled[y, x];

    public bool CanPlace(Piece piece)
    {
        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (!IsInside(x, y) || IsOccupied(x, y))
                return false;
        }
        return true;
    }

    public void Lock(Piece piece)
    {
        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (IsInside(x, y))
                _settled[y, x] = true;
        }
    }

    public int ClearCompleteLines()
    {
        int cleared = 0;
        for (int row = Height - 1; row >= 0; row--)
        {
            if (IsRowComplete(row))
            {
                ShiftDown(row);
                cleared++;
                row++; // re-check same row index after shift
            }
        }
        return cleared;
    }

    public void Reset()
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                _settled[y, x] = false;
    }

    public bool GetCell(int x, int y) => IsInside(x, y) && _settled[y, x];

    private bool IsRowComplete(int row)
    {
        for (int x = 0; x < Width; x++)
            if (!_settled[row, x]) return false;
        return true;
    }

    private void ShiftDown(int clearedRow)
    {
        for (int row = clearedRow; row > 0; row--)
            for (int x = 0; x < Width; x++)
                _settled[row, x] = _settled[row - 1, x];
        for (int x = 0; x < Width; x++)
            _settled[0, x] = false;
    }
}
