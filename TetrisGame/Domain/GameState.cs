namespace TetrisGame.Domain;

/// <summary>
/// Aggregates all runtime game state: board, active piece, next piece, score system, and game-over flag.
/// </summary>
public sealed class GameState
{
    public Board Board { get; private set; }
    public Piece ActivePiece { get; private set; }
    public Piece NextPiece { get; private set; }
    public ScoreSystem ScoreSystem { get; }
    public bool IsGameOver { get; private set; }

    private PieceFactory _factory;

    public GameState()
    {
        ScoreSystem = new ScoreSystem();
        _factory = new PieceFactory();
        Board = new Board();
        ActivePiece = _factory.TakeNext();
        NextPiece = _factory.TakeNext();
        IsGameOver = false;
    }

    public void TryMoveLeft()
    {
        if (IsGameOver) return;
        var moved = ActivePiece.Move(-1, 0);
        if (Board.CanPlace(moved))
            ActivePiece = moved;
    }

    public void TryMoveRight()
    {
        if (IsGameOver) return;
        var moved = ActivePiece.Move(1, 0);
        if (Board.CanPlace(moved))
            ActivePiece = moved;
    }

    /// <summary>
    /// Attempts to move the active piece down one row.
    /// Returns true if successful, false if blocked.
    /// </summary>
    public bool TryMoveDown()
    {
        if (IsGameOver) return false;
        var moved = ActivePiece.Move(0, 1);
        if (Board.CanPlace(moved))
        {
            ActivePiece = moved;
            return true;
        }
        return false;
    }

    public void TryRotateClockwise()
    {
        if (IsGameOver) return;
        var rotated = ActivePiece.RotateClockwise();
        if (Board.CanPlace(rotated))
            ActivePiece = rotated;
    }

    /// <summary>
    /// Advances gravity by one step. Locks the piece and spawns the next if blocked.
    /// </summary>
    public void TickGravity()
    {
        if (IsGameOver) return;
        if (!TryMoveDown())
            LockAndSpawnNext();
    }

    /// <summary>
    /// Locks the active piece, clears complete lines, updates score, and spawns the next piece.
    /// Sets IsGameOver if the new active piece cannot be placed (spawn collision — T036).
    /// </summary>
    public void LockAndSpawnNext()
    {
        if (IsGameOver) return;

        Board.Lock(ActivePiece);

        int cleared = Board.ClearCompleteLines();
        if (cleared > 0)
            ScoreSystem.ApplyLineClear(cleared);

        ActivePiece = NextPiece;
        NextPiece = _factory.TakeNext();

        // T036: spawn-collision game-over detection
        if (!Board.CanPlace(ActivePiece))
            IsGameOver = true;
    }

    /// <summary>
    /// T039: Resets all game state — board, score, level, and piece bag — for a new game.
    /// Clears IsGameOver and spawns a fresh active piece and next piece.
    /// </summary>
    public void Reset()
    {
        _factory = new PieceFactory();
        Board = new Board();
        ScoreSystem.Reset();
        ActivePiece = _factory.TakeNext();
        NextPiece = _factory.TakeNext();
        IsGameOver = false;
    }
}
