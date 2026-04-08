namespace TetrisGame.Domain;

public class Board
{
    public const int Width = 10;
    public const int Height = 20;

    private readonly bool[,] _cells = new bool[Height, Width];

    public bool IsOccupied(int row, int col) => _cells[row, col];

    public bool IsInBounds(int row, int col) =>
        row >= 0 && row < Height && col >= 0 && col < Width;

    public bool CanPlace(Piece piece)
    {
        foreach (var (r, c) in piece.GetCells())
        {
            if (!IsInBounds(r, c)) return false;
            if (_cells[r, c]) return false;
        }
        return true;
    }

    public void Lock(Piece piece)
    {
        foreach (var (r, c) in piece.GetCells())
        {
            if (IsInBounds(r, c))
                _cells[r, c] = true;
        }
    }

    public int ClearFullLines()
    {
        int cleared = 0;
        for (int r = Height - 1; r >= 0; r--)
        {
            if (IsRowFull(r))
            {
                RemoveRow(r);
                cleared++;
                r++; // re-check same index after shift
            }
        }
        return cleared;
    }

    public void Reset()
    {
        for (int r = 0; r < Height; r++)
            for (int c = 0; c < Width; c++)
                _cells[r, c] = false;
    }

    private bool IsRowFull(int row)
    {
        for (int c = 0; c < Width; c++)
            if (!_cells[row, c]) return false;
        return true;
    }

    private void RemoveRow(int row)
    {
        for (int r = row; r > 0; r--)
            for (int c = 0; c < Width; c++)
                _cells[r, c] = _cells[r - 1, c];
        for (int c = 0; c < Width; c++)
            _cells[0, c] = false;
    }

    public bool[,] GetSnapshot()
    {
        var snap = new bool[Height, Width];
        Array.Copy(_cells, snap, _cells.Length);
        return snap;
    }
}
