namespace TetrisGame.Domain;

public sealed class Board
{
    public const int Width = 10;
    public const int Height = 20;

    private readonly bool[,] _settled = new bool[Height, Width];

    public bool IsInside(Cell cell) =>
        cell.X >= 0 && cell.X < Width && cell.Y >= 0 && cell.Y < Height;

    public bool IsOccupied(Cell cell) =>
        IsInside(cell) && _settled[cell.Y, cell.X];

    public bool CanPlace(Piece piece) =>
        piece.GetAbsoluteCells().All(c => IsInside(c) && !IsOccupied(c));
}
