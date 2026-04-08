namespace TetrisGame.Rendering;

using TetrisGame.Domain;

/// <summary>
/// Renders the Tetris game state to the console.
/// Responsibilities:
///   - Full board + active piece overlay (T029)
///   - Next-piece preview panel (T030)
///   - Score info panel (T031)
///   - Level info panel (T032)
///   - Game-over screen with final score and replay prompt (T037)
/// </summary>
public sealed class ConsoleRenderer
{
    private const int BoardLeft = 1;
    private const int BoardTop = 1;
    private const int InfoCol = Board.Width * 2 + 4; // right of the board

    // Color mapping per piece type
    private static readonly Dictionary<TetrominoType, ConsoleColor> PieceColors = new()
    {
        [TetrominoType.I] = ConsoleColor.Cyan,
        [TetrominoType.O] = ConsoleColor.Yellow,
        [TetrominoType.T] = ConsoleColor.Magenta,
        [TetrominoType.S] = ConsoleColor.Green,
        [TetrominoType.Z] = ConsoleColor.Red,
        [TetrominoType.J] = ConsoleColor.Blue,
        [TetrominoType.L] = ConsoleColor.DarkYellow,
    };

    public void Initialize()
    {
        Console.CursorVisible = false;
        Console.Clear();
    }

    /// <summary>
    /// Renders one full frame: board, active piece, next preview, score, level.
    /// </summary>
    public void RenderFrame(GameState state)
    {
        DrawBorder();
        DrawSettledBoard(state.Board);
        DrawActivePiece(state.ActivePiece);
        DrawInfoPanel(state);
    }

    /// <summary>
    /// Renders the game-over screen: freezes board, shows final score, and displays replay prompt.
    /// Spec: FR-017 — display final score and ask "¿Deseas jugar de nuevo? (S/N)".
    /// </summary>
    public void RenderGameOver(GameState state)
    {
        // Show the final board state (settled blocks only, no active piece)
        DrawBorder();
        DrawSettledBoard(state.Board);
        DrawInfoPanel(state);

        // Overlay game-over message in the centre of the board
        int msgRow = BoardTop + Board.Height / 2 - 1;
        int msgCol = BoardLeft;

        WriteAt(msgCol, msgRow, "  GAME  OVER  ", ConsoleColor.White, ConsoleColor.DarkRed);
        WriteAt(msgCol, msgRow + 1,
            $"  Score: {state.Score.Score,6}  ",
            ConsoleColor.White, ConsoleColor.DarkRed);
        WriteAt(msgCol, msgRow + 2,
            "¿Deseas jugar de nuevo? (S/N)",
            ConsoleColor.Yellow, ConsoleColor.Black);

        // Place cursor below prompt so it does not flicker on the board
        Console.SetCursorPosition(0, BoardTop + Board.Height + 2);
    }

    // -------------------------------------------------------------------------
    // Private helpers
    // -------------------------------------------------------------------------

    private static void DrawBorder()
    {
        // Top border
        Console.SetCursorPosition(0, 0);
        Console.Write("+" + new string('-', Board.Width * 2) + "+");

        // Side borders
        for (int row = 0; row < Board.Height; row++)
        {
            Console.SetCursorPosition(0, row + BoardTop);
            Console.Write("|");
            Console.SetCursorPosition(Board.Width * 2 + 1, row + BoardTop);
            Console.Write("|");
        }

        // Bottom border
        Console.SetCursorPosition(0, Board.Height + BoardTop);
        Console.Write("+" + new string('-', Board.Width * 2) + "+");
    }

    private static void DrawSettledBoard(Board board)
    {
        for (int row = 0; row < Board.Height; row++)
        {
            Console.SetCursorPosition(BoardLeft, row + BoardTop);
            for (int col = 0; col < Board.Width; col++)
            {
                if (board.GetSettled(col, row))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("[]");
                }
                else
                {
                    Console.Write("  ");
                }
            }
        }
        Console.ResetColor();
    }

    private static void DrawActivePiece(Piece piece)
    {
        if (PieceColors.TryGetValue(piece.Type, out var color))
            Console.ForegroundColor = color;
        else
            Console.ForegroundColor = ConsoleColor.White;

        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (y < 0 || y >= Board.Height || x < 0 || x >= Board.Width)
                continue;
            Console.SetCursorPosition(BoardLeft + x * 2, y + BoardTop);
            Console.Write("[]");
        }
        Console.ResetColor();
    }

    private void DrawInfoPanel(GameState state)
    {
        int row = BoardTop;

        // Score
        Console.SetCursorPosition(InfoCol, row);
        Console.Write($"Score : {state.Score.Score,6}");

        // Level
        Console.SetCursorPosition(InfoCol, row + 1);
        Console.Write($"Level : {state.Score.Level,6}");

        // Lines
        Console.SetCursorPosition(InfoCol, row + 2);
        Console.Write($"Lines : {state.Score.TotalLines,6}");

        // Next piece label
        Console.SetCursorPosition(InfoCol, row + 4);
        Console.Write("Next:  ");

        // Next piece preview (4x4 grid)
        DrawNextPiecePreview(state.NextPiece, InfoCol, row + 5);
    }

    private static void DrawNextPiecePreview(Piece piece, int startCol, int startRow)
    {
        // Clear preview area (4 cols x 4 rows, each cell = 2 chars wide)
        for (int r = 0; r < 4; r++)
        {
            Console.SetCursorPosition(startCol, startRow + r);
            Console.Write("        "); // 8 spaces for 4 cells
        }

        if (PieceColors.TryGetValue(piece.Type, out var color))
            Console.ForegroundColor = color;
        else
            Console.ForegroundColor = ConsoleColor.White;

        foreach (var (x, y) in piece.GetAbsoluteCells())
        {
            if (y < 0 || y >= 4 || x < 0 || x >= 4)
                continue;
            Console.SetCursorPosition(startCol + x * 2, startRow + y);
            Console.Write("[]");
        }
        Console.ResetColor();
    }

    private static void WriteAt(int col, int row, string text, ConsoleColor fg, ConsoleColor bg)
    {
        Console.SetCursorPosition(col, row);
        Console.ForegroundColor = fg;
        Console.BackgroundColor = bg;
        Console.Write(text);
        Console.ResetColor();
    }
}
