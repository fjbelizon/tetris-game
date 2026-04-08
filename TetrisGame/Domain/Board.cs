namespace TetrisGame.Domain;

public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    private readonly bool[,] _settled = new bool[Height, Width];

    public bool IsOccupied(int x, int y) => _settled[y, x];

    public bool IsInside(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

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
            _settled[y, x] = true;
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
                row++;
            }
        }
        return cleared;
    }

    public bool IsRowComplete(int row)
    {
        for (int col = 0; col < Width; col++)
            if (!_settled[row, col]) return false;
        return true;
    }

    private void RemoveRow(int row)
    {
        for (int r = row; r > 0; r--)
            for (int col = 0; col < Width; col++)
                _settled[r, col] = _settled[r - 1, col];
        for (int col = 0; col < Width; col++)
            _settled[0, col] = false;
    }

    public bool[,] GetSettled() => _settled;

    public void Reset()
    {
        for (int r = 0; r < Height; r++)
            for (int c = 0; c < Width; c++)
                _settled[r, c] = false;
    }
}
