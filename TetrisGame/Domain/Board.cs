namespace TetrisGame.Domain;

/// <summary>
/// Represents the 10-column × 20-row Tetris game board.
/// Tracks which cells are occupied by settled blocks.
/// </summary>
public sealed class Board
{
    public const int Columns = 10;
    public const int Rows = 20;

    private readonly bool[,] _cells = new bool[Rows, Columns];

    /// <summary>
    /// Returns whether the cell at (row, col) is occupied by a settled block.
    /// </summary>
    public bool IsOccupied(int row, int col) => _cells[row, col];

    /// <summary>
    /// Returns whether the given (row, col) coordinates fall within the board boundaries.
    /// </summary>
    public static bool IsInBounds(int row, int col) =>
        row >= 0 && row < Rows && col >= 0 && col < Columns;

    /// <summary>
    /// Settles a block at the given (row, col). Throws if out of bounds.
    /// </summary>
    public void SetOccupied(int row, int col)
    {
        if (!IsInBounds(row, col))
            throw new ArgumentOutOfRangeException(nameof(row), $"Position ({row}, {col}) is outside the board.");
        _cells[row, col] = true;
    }

    /// <summary>
    /// Clears a settled block at the given (row, col). Throws if out of bounds.
    /// </summary>
    public void ClearCell(int row, int col)
    {
        if (!IsInBounds(row, col))
            throw new ArgumentOutOfRangeException(nameof(row), $"Position ({row}, {col}) is outside the board.");
        _cells[row, col] = false;
    }
}
