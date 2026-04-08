using TetrisGame.Domain;

namespace TetrisGame.Rendering;

public class ConsoleRenderer
{
    private const char FilledCell = '█';
    private const char EmptyCell = '·';
    private const char BorderV = '│';
    private const char BorderH = '─';
    private const char BorderTL = '┌';
    private const char BorderTR = '┐';
    private const char BorderBL = '└';
    private const char BorderBR = '┘';

    private int _renderRow;
    private int _renderCol;

    public ConsoleRenderer()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;
        _renderRow = 0;
        _renderCol = 0;
    }

    public void Render(GameState state)
    {
        Console.SetCursorPosition(_renderCol, _renderRow);
        var snapshot = state.Board.GetSnapshot();
        var activeCells = new HashSet<(int, int)>(state.ActivePiece.GetCells());

        DrawBoardTop();
        for (int r = 0; r < Board.Height; r++)
        {
            Console.SetCursorPosition(_renderCol, _renderRow + 1 + r);
            Console.Write(BorderV);
            for (int c = 0; c < Board.Width; c++)
            {
                bool active = activeCells.Contains((r, c));
                bool settled = snapshot[r, c];
                Console.Write(active || settled ? FilledCell : EmptyCell);
            }
            Console.Write(BorderV);

            // Side panel
            DrawSidePanel(r, state);
        }
        DrawBoardBottom();
    }

    public void RenderGameOver(GameState state)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("  ╔══════════════════════╗");
        Console.WriteLine("  ║      GAME  OVER       ║");
        Console.WriteLine("  ╚══════════════════════╝");
        Console.WriteLine();
        Console.WriteLine($"  Puntuación final: {state.Score}");
        Console.WriteLine($"  Nivel alcanzado:  {state.Level}");
        Console.WriteLine();
        Console.WriteLine("  ¿Deseas jugar de nuevo? (S/N)");
    }

    public void Clear()
    {
        Console.Clear();
    }

    private void DrawBoardTop()
    {
        Console.SetCursorPosition(_renderCol, _renderRow);
        Console.Write(BorderTL);
        for (int c = 0; c < Board.Width; c++) Console.Write(BorderH);
        Console.Write(BorderTR);
    }

    private void DrawBoardBottom()
    {
        Console.SetCursorPosition(_renderCol, _renderRow + Board.Height + 1);
        Console.Write(BorderBL);
        for (int c = 0; c < Board.Width; c++) Console.Write(BorderH);
        Console.Write(BorderBR);
    }

    private void DrawSidePanel(int row, GameState state)
    {
        Console.SetCursorPosition(_renderCol + Board.Width + 3, _renderRow + 1 + row);
        switch (row)
        {
            case 0:
                Console.Write("NEXT:");
                break;
            case 1:
            case 2:
            case 3:
                DrawNextPieceRow(row - 1, state.NextPiece);
                break;
            case 5:
                Console.Write($"Score: {state.Score}");
                break;
            case 7:
                Console.Write($"Level: {state.Level}");
                break;
            case 9:
                Console.Write("Controls:");
                break;
            case 10:
                Console.Write("← → Move");
                break;
            case 11:
                Console.Write("↓  Drop");
                break;
            case 12:
                Console.Write("Space Rotate");
                break;
        }
    }

    private void DrawNextPieceRow(int previewRow, Piece next)
    {
        var cells = new HashSet<(int, int)>(next.GetCells()
            .Select(c => (c.row - next.GetCells().Min(x => x.row),
                          c.col - next.GetCells().Min(x => x.col))));

        const int previewWidth = 4;
        var sb = new System.Text.StringBuilder();
        for (int c = 0; c < previewWidth; c++)
            sb.Append(cells.Contains((previewRow, c)) ? FilledCell : ' ');
        Console.Write(sb.ToString());
    }
}
