using TetrisGame.Domain;

namespace TetrisGame.Rendering;

public static class ConsoleRenderer
{
    private const char BlockChar = '#';
    private const char EmptyChar = '.';
    private const int InfoPanelColumn = Board.Width * 2 + 4;

    public static void RenderFrame(GameState state)
    {
        Console.SetCursorPosition(0, 0);

        var activeSet = state.ActivePiece is not null
            ? state.ActivePiece.GetAbsoluteCells().ToHashSet()
            : new HashSet<(int x, int y)>();

        for (int row = 0; row < Board.Height; row++)
        {
            Console.Write("|");
            for (int col = 0; col < Board.Width; col++)
            {
                bool settled = state.Board.GetCell(col, row);
                bool active = activeSet.Contains((col, row));
                Console.Write(settled || active ? $"{BlockChar} " : $"{EmptyChar} ");
            }
            Console.Write("|");
            Console.WriteLine();
        }

        Console.WriteLine(new string('-', Board.Width * 2 + 2));
        RenderInfoPanel(state);
    }

    private static void RenderInfoPanel(GameState state)
    {
        Console.SetCursorPosition(InfoPanelColumn, 0);
        Console.Write($"Score: {state.Score.Score,-8}");
        Console.SetCursorPosition(InfoPanelColumn, 1);
        Console.Write($"Level: {state.Score.Level,-8}");
        Console.SetCursorPosition(InfoPanelColumn, 2);
        Console.Write($"Lines: {state.Score.TotalLines,-8}");

        Console.SetCursorPosition(InfoPanelColumn, 4);
        Console.Write("Next:");
        RenderNextPiece(state.NextPiece, InfoPanelColumn, 5);
    }

    private static void RenderNextPiece(Piece? next, int startCol, int startRow)
    {
        for (int row = startRow; row < startRow + 4; row++)
        {
            Console.SetCursorPosition(startCol, row);
            Console.Write("          ");
        }

        if (next is null) return;

        var cells = next.GetAbsoluteCells().ToList();
        int minX = cells.Min(c => c.x);
        int minY = cells.Min(c => c.y);

        foreach (var (x, y) in cells)
        {
            Console.SetCursorPosition(startCol + (x - minX) * 2, startRow + (y - minY));
            Console.Write($"{BlockChar} ");
        }
    }

    public static void RenderGameOver(GameState state)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("  *** GAME OVER ***");
        Console.WriteLine();
        Console.WriteLine($"  Final Score: {state.Score.Score}");
        Console.WriteLine($"  Level reached: {state.Score.Level}");
        Console.WriteLine($"  Lines cleared: {state.Score.TotalLines}");
        Console.WriteLine();
        Console.WriteLine("  ¿Deseas jugar de nuevo? (S/N)");
    }
}
