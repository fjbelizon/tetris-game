namespace TetrisGame.Domain;

/// <summary>
/// 10 × 20 playfield that records settled (locked) blocks.
/// </summary>
public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    private bool[,] _settled = new bool[Height, Width];

    // ------------------------------------------------------------------
    // Queries
    // ------------------------------------------------------------------

    public bool IsInside(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height;

    public bool IsOccupied(int x, int y) =>
        IsInside(x, y) && _settled[y, x];

    /// <summary>Returns true if every cell of <paramref name="piece"/> is
    /// inside the board and not already occupied.</summary>
    public bool CanPlace(Piece piece)
    {
        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (!IsInside(x, y) || IsOccupied(x, y))
                return false;
        }
        return true;
    }

    // ------------------------------------------------------------------
    // Mutations
    // ------------------------------------------------------------------

    /// <summary>Permanently settles the piece's cells onto the board.</summary>
    public void Lock(Piece piece)
    {
        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (IsInside(x, y))
                _settled[y, x] = true;
        }
    }

    /// <summary>Removes every completely filled row, shifts rows above
    /// down by one for each removal, and returns the count cleared.</summary>
    public int ClearCompleteLines()
    {
        int cleared = 0;
        for (int row = Height - 1; row >= 0; row--)
        {
            if (IsRowComplete(row))
            {
                RemoveRow(row);
                row++;   // re-check the same index after shift
                cleared++;
            }
        }
        return cleared;
    }

    /// <summary>Resets the board to empty.</summary>
    public void Reset() => _settled = new bool[Height, Width];

    // ------------------------------------------------------------------
    // Internal test helper – allows unit tests to pre-settle specific cells
    // without going through the piece-lock flow.
    // ------------------------------------------------------------------
    internal void ForceSettle(int x, int y)
    {
        if (IsInside(x, y))
            _settled[y, x] = true;
    }

    // ------------------------------------------------------------------
    // Private helpers
    // ------------------------------------------------------------------

    private bool IsRowComplete(int row)
    {
        for (int col = 0; col < Width; col++)
        {
            if (!_settled[row, col])
                return false;
        }
        return true;
    }

    private void RemoveRow(int row)
    {
        for (int r = row; r > 0; r--)
        {
            for (int col = 0; col < Width; col++)
                _settled[r, col] = _settled[r - 1, col];
        }
        for (int col = 0; col < Width; col++)
            _settled[0, col] = false;
    }
}
