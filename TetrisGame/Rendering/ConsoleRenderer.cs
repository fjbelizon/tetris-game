namespace TetrisGame.Rendering;

using TetrisGame.Domain;

public static class ConsoleRenderer
{
    private const char FilledCell = '#';
    private const char EmptyCell = '.';
    private const char ActiveCell = 'X';

    public static void RenderFrame(GameState state)
    {
        Console.SetCursorPosition(0, 0);
        Console.CursorVisible = false;

        // Collect active piece cells for overlay
        var activeCells = state.IsGameOver
            ? []
            : state.ActivePiece.GetAbsoluteCells().ToHashSet();

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("+----------+  NEXT:");
        sb.Append(RenderNextPiece(state));

        for (int row = 0; row < Board.Height; row++)
        {
            sb.Append('|');
            for (int col = 0; col < Board.Width; col++)
            {
                if (activeCells.Contains((col, row)))
                    sb.Append(ActiveCell);
                else if (state.Board.GetCell(col, row))
                    sb.Append(FilledCell);
                else
                    sb.Append(EmptyCell);
            }
            sb.Append('|');
            if (row == 0) sb.Append($"  Score: {state.ScoreSystem.Score}");
            if (row == 1) sb.Append($"  Level: {state.ScoreSystem.Level}");
            if (row == 2) sb.Append($"  Lines: {state.ScoreSystem.TotalLines}");
            sb.AppendLine();
        }
        sb.AppendLine("+----------+");
        Console.Write(sb);
    }

    public static void RenderGameOver(GameState state)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("  *** GAME OVER ***");
        Console.WriteLine();
        Console.WriteLine($"  Final Score: {state.ScoreSystem.Score}");
        Console.WriteLine($"  Level reached: {state.ScoreSystem.Level}");
        Console.WriteLine($"  Lines cleared: {state.ScoreSystem.TotalLines}");
        Console.WriteLine();
        Console.WriteLine("  ¿Deseas jugar de nuevo? (S/N)");
    }

    private static string RenderNextPiece(GameState state)
    {
        // Render a 4x4 preview for the next piece
        var cells = state.NextPiece.GetAbsoluteCells();
        int minX = cells.Min(c => c.x);
        int minY = cells.Min(c => c.y);
        var normalized = cells.Select(c => (c.x - minX, c.y - minY)).ToHashSet();

        var sb = new System.Text.StringBuilder();
        for (int r = 0; r < 4; r++)
        {
            sb.Append("           ");
            for (int c = 0; c < 4; c++)
                sb.Append(normalized.Contains((c, r)) ? ActiveCell : ' ');
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
