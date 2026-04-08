using System;
using TetrisGame.Domain;

namespace TetrisGame.Rendering;

public sealed class ConsoleRenderer
{
    private const int BoardLeft = 1;
    private const int BoardTop = 1;
    private const int InfoLeft = 14;

    public void DrawFrame(GameState state)
    {
        Console.SetCursorPosition(0, 0);
        DrawBorder();
        DrawBoard(state.Board, state.ActivePiece);
        DrawInfoPanel(state);
    }

    public void DrawGameOver(GameState state)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("╔══════════╗");
        Console.WriteLine("║ GAME OVER║");
        Console.WriteLine("╚══════════╝");
        Console.WriteLine();
        Console.WriteLine($"  Score: {state.Score.Score}");
        Console.WriteLine($"  Level: {state.Score.Level}");
        Console.WriteLine($"  Lines: {state.Score.TotalLines}");
        Console.WriteLine();
        Console.WriteLine("¿Deseas jugar de nuevo? (S/N)");
    }

    private void DrawBorder()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write("╔");
        Console.Write(new string('═', Board.Width));
        Console.WriteLine("╗");
        for (int row = 0; row < Board.Height; row++)
        {
            Console.Write("║");
            Console.Write(new string(' ', Board.Width));
            Console.WriteLine("║");
        }
        Console.Write("╚");
        Console.Write(new string('═', Board.Width));
        Console.WriteLine("╝");
    }

    private void DrawBoard(Board board, Piece active)
    {
        var activeCells = active.GetAbsoluteCells();
        var activeSet = new HashSet<(int, int)>(activeCells);

        for (int row = 0; row < Board.Height; row++)
        {
            Console.SetCursorPosition(BoardLeft, BoardTop + row);
            for (int col = 0; col < Board.Width; col++)
            {
                if (activeSet.Contains((col, row)))
                    Console.Write("█");
                else if (board.IsSettled(col, row))
                    Console.Write("░");
                else
                    Console.Write(" ");
            }
        }
    }

    private void DrawInfoPanel(GameState state)
    {
        Console.SetCursorPosition(InfoLeft, 1);
        Console.Write("NEXT:");
        DrawNextPiece(state.NextPiece);

        Console.SetCursorPosition(InfoLeft, 7);
        Console.Write($"Score: {state.Score.Score,6}");
        Console.SetCursorPosition(InfoLeft, 8);
        Console.Write($"Level: {state.Score.Level,6}");
        Console.SetCursorPosition(InfoLeft, 9);
        Console.Write($"Lines: {state.Score.TotalLines,6}");
    }

    private void DrawNextPiece(Piece next)
    {
        // Clear a 4x4 preview area
        for (int row = 0; row < 4; row++)
        {
            Console.SetCursorPosition(InfoLeft, 2 + row);
            Console.Write("      ");
        }

        var cells = next.GetAbsoluteCells();
        // Normalize: find min x,y
        int minX = cells.Min(c => c.x);
        int minY = cells.Min(c => c.y);

        foreach (var (x, y) in cells)
        {
            int col = InfoLeft + (x - minX);
            int row = 2 + (y - minY);
            if (col >= 0 && row >= 0 && row < 6)
            {
                Console.SetCursorPosition(col, row);
                Console.Write("█");
            }
        }
    }
}
