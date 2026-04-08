namespace TetrisGame.Domain;

public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    // Settled[row, col] — row 0 is top
    private readonly bool[,] _settled = new bool[Height, Width];

    public bool IsInside(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

    public bool IsOccupied(int x, int y) => IsInside(x, y) && _settled[y, x];

    public bool CanPlace(Piece piece)
    {
        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (!IsInside(x, y)) return false;
            if (_settled[y, x]) return false;
        }
        return true;
    }

    /// <summary>
    /// Returns true when the piece cannot move one row further down.
    /// </summary>
    public bool IsLocked(Piece piece) => !CanPlace(piece.Move(0, 1));

    /// <summary>
    /// Fixes all cells of <paramref name="piece"/> into the settled grid.
    /// </summary>
    public void Lock(Piece piece)
    {
        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (IsInside(x, y))
                _settled[y, x] = true;
        }
    }

    /// <summary>
    /// Clears every fully filled row and shifts rows above down.
    /// Returns the number of lines cleared.
    /// </summary>
    public int ClearCompleteLines()
    {
        int cleared = 0;
        for (int row = Height - 1; row >= 0; row--)
        {
            if (IsRowFull(row))
            {
                RemoveRow(row);
                row++;   // re-check the same index after shift
                cleared++;
            }
        }
        return cleared;
    }

    public bool GetCell(int x, int y) => IsInside(x, y) && _settled[y, x];

    private bool IsRowFull(int row)
    {
        for (int col = 0; col < Width; col++)
            if (!_settled[row, col]) return false;
        return true;
    }

    private void RemoveRow(int targetRow)
    {
        // Shift everything above down by one
        for (int row = targetRow; row > 0; row--)
            for (int col = 0; col < Width; col++)
                _settled[row, col] = _settled[row - 1, col];

        // Clear the top row
        for (int col = 0; col < Width; col++)
            _settled[0, col] = false;
    }
}
