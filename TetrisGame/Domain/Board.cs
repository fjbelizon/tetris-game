namespace TetrisGame.Domain;

public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    private readonly bool[,] _settled = new bool[Height, Width];

    public bool IsInside(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height;

    public bool IsOccupied(int x, int y) =>
        IsInside(x, y) && _settled[y, x];

    public bool CanPlace(Piece piece)
    {
        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (!IsInside(x, y)) return false;
            if (_settled[y, x]) return false;
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
                RemoveRow(row);
                cleared++;
                row++; // re-check the same index after shift
            }
        }
        return cleared;
    }

    public bool IsSettled(int x, int y) => IsOccupied(x, y);

    private bool IsRowComplete(int row)
    {
        for (int col = 0; col < Width; col++)
            if (!_settled[row, col]) return false;
        return true;
    }

    private void RemoveRow(int rowToRemove)
    {
        // Shift rows above down by 1
        for (int row = rowToRemove; row > 0; row--)
            for (int col = 0; col < Width; col++)
                _settled[row, col] = _settled[row - 1, col];

        // Clear top row
        for (int col = 0; col < Width; col++)
            _settled[0, col] = false;
    }
}
