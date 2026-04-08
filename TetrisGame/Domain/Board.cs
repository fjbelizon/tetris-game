namespace TetrisGame.Domain;

/// <summary>
/// Represents the 10x20 Tetris playfield that records settled blocks.
/// </summary>
public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    // settled[row, col] — row 0 is the top
    private readonly bool[,] _settled = new bool[Height, Width];

    /// <summary>Returns true if the given cell coordinates are inside the board.</summary>
    public bool IsInside(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

    /// <summary>Returns true if the cell is settled (occupied by a locked block).</summary>
    public bool IsOccupied(int x, int y) => IsInside(x, y) && _settled[y, x];

    /// <summary>Returns true when all cells of the piece fit inside the board and none overlaps a settled block.</summary>
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
    /// Locks the piece onto the board, settling its blocks in place.
    /// Call only after verifying CanPlace returns true.
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
    /// Removes every fully filled row and shifts all rows above it downward.
    /// Returns the number of lines cleared.
    /// </summary>
    public int ClearCompleteLines()
    {
        int cleared = 0;

        for (int row = Height - 1; row >= 0; row--)
        {
            if (IsRowComplete(row))
            {
                RemoveRow(row);
                cleared++;
                // After RemoveRow shifts every row above downward by one, the row
                // that was at index (row-1) is now at index (row). Incrementing here
                // counteracts the loop's decrement so we re-examine the same index
                // on the next iteration and don't accidentally skip a row.
                row++;
            }
        }

        return cleared;
    }

    /// <summary>Exposes the settled state for rendering purposes.</summary>
    public bool GetSettled(int x, int y) => IsOccupied(x, y);

    // --- private helpers ---

    private bool IsRowComplete(int row)
    {
        for (int col = 0; col < Width; col++)
        {
            if (!_settled[row, col]) return false;
        }
        return true;
    }

    /// <summary>Removes the given row and shifts every row above it down by one.</summary>
    private void RemoveRow(int row)
    {
        // Shift rows from <row-1> down to row 0 downward by one
        for (int r = row; r > 0; r--)
        {
            for (int col = 0; col < Width; col++)
                _settled[r, col] = _settled[r - 1, col];
        }

        // Clear the top row
        for (int col = 0; col < Width; col++)
            _settled[0, col] = false;
    }
}
