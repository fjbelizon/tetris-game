namespace TetrisGame.Domain;

public sealed class Board
{
    public int Width { get; } = 10;
    public int Height { get; } = 20;

    // _settled[row, col]: row 0 = top, row Height-1 = bottom
    private readonly bool[,] _settled;

    public Board()
    {
        _settled = new bool[Height, Width];
    }

    private Board(bool[,] settled, int width, int height)
    {
        Width = width;
        Height = height;
        _settled = settled;
    }

    public bool IsInside(Cell cell) =>
        cell.X >= 0 && cell.X < Width && cell.Y >= 0 && cell.Y < Height;

    public bool IsOccupied(Cell cell) =>
        IsInside(cell) && _settled[cell.Y, cell.X];

    public bool CanPlace(Piece piece) =>
        piece.GetAbsoluteCells().All(c => IsInside(c) && !_settled[c.Y, c.X]);

    public void Lock(Piece piece)
    {
        foreach (var cell in piece.GetAbsoluteCells())
        {
            if (IsInside(cell))
                _settled[cell.Y, cell.X] = true;
        }
    }

    /// <summary>
    /// Removes all completely filled rows, shifts rows above down, and returns the count of cleared rows.
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
                // Recheck the same row index since rows shifted down
                row++;
            }
        }
        return cleared;
    }

    private bool IsRowComplete(int row)
    {
        for (int col = 0; col < Width; col++)
        {
            if (!_settled[row, col]) return false;
        }
        return true;
    }

    private void RemoveRow(int targetRow)
    {
        // Shift all rows above targetRow down by one
        for (int row = targetRow; row > 0; row--)
        {
            for (int col = 0; col < Width; col++)
                _settled[row, col] = _settled[row - 1, col];
        }
        // Clear the top row
        for (int col = 0; col < Width; col++)
            _settled[0, col] = false;
    }

    /// <summary>
    /// Sets a cell directly. Used for test setup.
    /// </summary>
    public void SetSettledCell(int row, int col, bool value)
    {
        if (row >= 0 && row < Height && col >= 0 && col < Width)
            _settled[row, col] = value;
    }

    /// <summary>
    /// Returns whether a specific settled cell is occupied. Used for assertions.
    /// </summary>
    public bool IsSettledAt(int row, int col) =>
        row >= 0 && row < Height && col >= 0 && col < Width && _settled[row, col];
}
