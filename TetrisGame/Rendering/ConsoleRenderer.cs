namespace TetrisGame.Rendering;

using TetrisGame.Domain;

public sealed class ConsoleRenderer
{
    // Layout constants
    private const int BoardLeft = 1;
    private const int BoardTop = 1;
    private const int InfoLeft = BoardLeft + Board.Width * 2 + 3;
    private const int InfoTop = 1;
    private const int NextPiecePreviewRows = 6;

    public void Initialize()
    {
        Console.CursorVisible = false;
        Console.Clear();
        DrawBorders();
    }

    public void Render(GameState state)
    {
        RenderBoard(state);
        RenderNextPiece(state.NextPiece);
        RenderScore(state.Score);
    }

    // T029 – frame rendering for board + active piece overlay
    private void RenderBoard(GameState state)
    {
        var settled = state.Board.GetSettled();

        // Collect active piece cells for overlay
        var activeCells = state.ActivePiece?.GetAbsoluteCells()
            .ToHashSet() ?? [];

        for (int row = 0; row < Board.Height; row++)
        {
            Console.SetCursorPosition(BoardLeft, BoardTop + row);
            for (int col = 0; col < Board.Width; col++)
            {
                if (activeCells.Contains((col, row)))
                {
                    Console.BackgroundColor = PieceColor(state.ActivePiece!.Type);
                    Console.Write("  ");
                    Console.ResetColor();
                }
                else if (settled[row, col])
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("  ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write("  ");
                }
            }
        }
    }

    // T030 – next-piece preview panel
    private void RenderNextPiece(Piece? next)
    {
        Console.SetCursorPosition(InfoLeft, InfoTop);
        Console.Write("NEXT:     ");

        // Clear the 4×4 preview grid
        for (int r = 0; r < NextPiecePreviewRows; r++)
        {
            Console.SetCursorPosition(InfoLeft, InfoTop + 1 + r);
            Console.Write("          ");
        }

        if (next is null) return;

        // Find bounding box to center the preview
        var cells = next.GetAbsoluteCells();
        int minX = cells.Min(c => c.x);
        int minY = cells.Min(c => c.y);

        foreach (var (x, y) in cells)
        {
            int previewCol = x - minX;
            int previewRow = y - minY;
            Console.SetCursorPosition(InfoLeft + previewCol * 2, InfoTop + 1 + previewRow);
            Console.BackgroundColor = PieceColor(next.Type);
            Console.Write("  ");
            Console.ResetColor();
        }
    }

    // T031 – render current score in info panel
    public void RenderScore(ScoreSystem score)
    {
        int scoreRow = InfoTop + NextPiecePreviewRows + 2;
        Console.SetCursorPosition(InfoLeft, scoreRow);
        Console.Write($"SCORE: {score.Score,7}");
    }

    // T032 – render current level in info panel
    public void RenderLevel(ScoreSystem score)
    {
        int levelRow = InfoTop + NextPiecePreviewRows + 3;
        Console.SetCursorPosition(InfoLeft, levelRow);
        Console.Write($"LEVEL: {score.Level,7}");
    }

    // T037 – game-over screen
    public void RenderGameOver(ScoreSystem score)
    {
        Console.Clear();
        Console.SetCursorPosition(0, Board.Height / 2 - 2);
        Console.WriteLine("  *** GAME OVER ***");
        Console.WriteLine($"  Final score: {score.Score}");
        Console.WriteLine();
        Console.Write("  ¿Deseas jugar de nuevo? (S/N): ");
    }

    private void DrawBorders()
    {
        // Top and bottom border
        Console.SetCursorPosition(0, 0);
        Console.Write("+" + new string('-', Board.Width * 2) + "+");
        Console.SetCursorPosition(0, Board.Height + 1);
        Console.Write("+" + new string('-', Board.Width * 2) + "+");

        // Side borders
        for (int r = 1; r <= Board.Height; r++)
        {
            Console.SetCursorPosition(0, r);
            Console.Write("|");
            Console.SetCursorPosition(Board.Width * 2 + 1, r);
            Console.Write("|");
        }
    }

    private static ConsoleColor PieceColor(TetrominoType type) => type switch
    {
        TetrominoType.I => ConsoleColor.Cyan,
        TetrominoType.O => ConsoleColor.Yellow,
        TetrominoType.T => ConsoleColor.Magenta,
        TetrominoType.S => ConsoleColor.Green,
        TetrominoType.Z => ConsoleColor.Red,
        TetrominoType.J => ConsoleColor.Blue,
        TetrominoType.L => ConsoleColor.DarkYellow,
        _ => ConsoleColor.White,
    };
}
