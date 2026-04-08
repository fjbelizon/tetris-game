using TetrisGame.Domain;

namespace TetrisGame.Rendering;

public class ConsoleRenderer
{
    private const int BoardLeft = 1;
    private const int BoardTop = 1;
    private const int InfoLeft = Board.Width * 2 + 4;
    private const int InfoTop = 1;

    private bool _initialized;

    public void Initialize()
    {
        Console.CursorVisible = false;
        Console.Clear();
        DrawBorder();
        _initialized = true;
    }

    public void RenderFrame(GameState state)
    {
        if (!_initialized) Initialize();

        RenderBoard(state);
        RenderInfoPanel(state);
    }

    public void RenderGameOver(GameState state)
    {
        int midRow = Board.Height / 2;
        int col = BoardLeft;

        Console.SetCursorPosition(col, BoardTop + midRow - 2);
        Console.Write("  GAME OVER   ");

        Console.SetCursorPosition(col, BoardTop + midRow);
        Console.Write($"  Score: {state.ScoreSystem.Score,-6}");

        Console.SetCursorPosition(col, BoardTop + midRow + 2);
        Console.Write("¿Deseas jugar de nuevo? (S/N)");

        Console.SetCursorPosition(col, BoardTop + midRow + 3);
        Console.Write("> ");
    }

    private void DrawBorder()
    {
        // Top border
        Console.SetCursorPosition(0, 0);
        Console.Write("+" + new string('-', Board.Width * 2) + "+");

        // Side borders
        for (int row = 0; row < Board.Height; row++)
        {
            Console.SetCursorPosition(0, BoardTop + row);
            Console.Write("|");
            Console.SetCursorPosition(BoardLeft + Board.Width * 2, BoardTop + row);
            Console.Write("|");
        }

        // Bottom border
        Console.SetCursorPosition(0, BoardTop + Board.Height);
        Console.Write("+" + new string('-', Board.Width * 2) + "+");
    }

    private void RenderBoard(GameState state)
    {
        var settled = state.Board.GetSettled();
        var activeCells = state.ActivePiece.GetAbsoluteCells().ToHashSet();

        for (int row = 0; row < Board.Height; row++)
        {
            Console.SetCursorPosition(BoardLeft, BoardTop + row);
            for (int col = 0; col < Board.Width; col++)
            {
                if (activeCells.Contains((col, row)))
                {
                    Console.Write("[]");
                }
                else if (settled[row, col])
                {
                    Console.Write("##");
                }
                else
                {
                    Console.Write("  ");
                }
            }
        }
    }

    private void RenderInfoPanel(GameState state)
    {
        RenderNextPiece(state);
        RenderScore(state);
        RenderLevel(state);
    }

    private void RenderNextPiece(GameState state)
    {
        Console.SetCursorPosition(InfoLeft, InfoTop);
        Console.Write("NEXT:");

        // Clear preview area (4x4 cells, 2 chars each)
        for (int row = 0; row < 4; row++)
        {
            Console.SetCursorPosition(InfoLeft, InfoTop + 1 + row);
            Console.Write("        ");
        }

        // Render next piece preview
        var previewCells = state.NextPiece.GetAbsoluteCells().ToList();

        // Find bounding box to center
        int minX = previewCells.Min(c => c.x);
        int minY = previewCells.Min(c => c.y);

        foreach (var (x, y) in previewCells)
        {
            int drawX = InfoLeft + (x - minX) * 2;
            int drawY = InfoTop + 1 + (y - minY);
            Console.SetCursorPosition(drawX, drawY);
            Console.Write("[]");
        }
    }

    private void RenderScore(GameState state)
    {
        Console.SetCursorPosition(InfoLeft, InfoTop + 6);
        Console.Write("SCORE:");
        Console.SetCursorPosition(InfoLeft, InfoTop + 7);
        Console.Write($"{state.ScoreSystem.Score,-8}");
    }

    private void RenderLevel(GameState state)
    {
        Console.SetCursorPosition(InfoLeft, InfoTop + 9);
        Console.Write("LEVEL:");
        Console.SetCursorPosition(InfoLeft, InfoTop + 10);
        Console.Write($"{state.ScoreSystem.Level,-4}");
    }
}
