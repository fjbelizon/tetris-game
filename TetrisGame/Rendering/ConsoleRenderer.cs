using TetrisGame.Domain;

namespace TetrisGame.Rendering;

/// <summary>
/// Console-based renderer for the Tetris game.
/// Renders board, active piece overlay, next-piece preview, score, and level.
/// On game over, shows the final score and the replay prompt.
/// </summary>
public sealed class ConsoleRenderer
{
    private const char FilledCell = '█';
    private const char EmptyCell  = '·';
    private const char Border     = '|';

    public void Render(GameState state)
    {
        Console.Clear();

        if (state.IsGameOver)
        {
            RenderGameOver(state);
            return;
        }

        RenderBoard(state);
        RenderInfoPanel(state);
    }

    // ------------------------------------------------------------------
    // Board + active piece overlay
    // ------------------------------------------------------------------

    private void RenderBoard(GameState state)
    {
        // Build a snapshot grid combining settled cells and the active piece.
        bool[,] grid = new bool[Board.Height, Board.Width];

        for (int row = 0; row < Board.Height; row++)
            for (int col = 0; col < Board.Width; col++)
                grid[row, col] = state.Board.IsOccupied(col, row);

        foreach (var (x, y) in state.ActivePiece.GetAbsoluteCells())
        {
            if (y >= 0 && y < Board.Height && x >= 0 && x < Board.Width)
                grid[y, x] = true;
        }

        for (int row = 0; row < Board.Height; row++)
        {
            Console.Write(Border);
            for (int col = 0; col < Board.Width; col++)
                Console.Write(grid[row, col] ? FilledCell : EmptyCell);
            Console.WriteLine(Border);
        }

        Console.WriteLine(new string('-', Board.Width + 2));
    }

    // ------------------------------------------------------------------
    // Info panel (score, level, next piece)
    // ------------------------------------------------------------------

    private void RenderInfoPanel(GameState state)
    {
        Console.WriteLine($"Score : {state.Score.Score}");
        Console.WriteLine($"Level : {state.Score.Level}");
        Console.WriteLine($"Lines : {state.Score.TotalLines}");
        Console.WriteLine();
        Console.WriteLine("Next:");
        RenderNextPiece(state.NextPiece);
        Console.WriteLine();
        Console.WriteLine("Controls: ← → ↓  Space=Rotate");
    }

    private void RenderNextPiece(Piece piece)
    {
        bool[,] preview = new bool[4, 4];
        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            int px = x - piece.OriginX;
            int py = y - piece.OriginY;
            if (px >= 0 && px < 4 && py >= 0 && py < 4)
                preview[py, px] = true;
        }

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
                Console.Write(preview[row, col] ? FilledCell : ' ');
            Console.WriteLine();
        }
    }

    // ------------------------------------------------------------------
    // Game-over screen  (T037)
    // ------------------------------------------------------------------

    private void RenderGameOver(GameState state)
    {
        Console.WriteLine("╔══════════════════╗");
        Console.WriteLine("║    GAME  OVER    ║");
        Console.WriteLine("╚══════════════════╝");
        Console.WriteLine();
        Console.WriteLine($"  Puntuación final : {state.Score.Score}");
        Console.WriteLine($"  Nivel alcanzado  : {state.Score.Level}");
        Console.WriteLine($"  Líneas borradas  : {state.Score.TotalLines}");
        Console.WriteLine();
        Console.WriteLine("  ¿Deseas jugar de nuevo? (S/N)");
    }
}
