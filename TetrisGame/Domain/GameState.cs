namespace TetrisGame.Domain;

public sealed class GameState
{
    private readonly PieceFactory _factory;

    public Board Board { get; private set; }
    public Piece ActivePiece { get; private set; }
    public Piece NextPiece { get; private set; }
    public ScoreSystem Score { get; private set; }
    public bool IsGameOver { get; private set; }

    public GameState(PieceFactory? factory = null)
    {
        _factory = factory ?? new PieceFactory();
        Board = new Board();
        Score = new ScoreSystem();
        var firstType = _factory.TakeNext();
        var nextType = _factory.TakeNext();
        ActivePiece = _factory.CreateSpawnPiece(firstType);
        NextPiece = _factory.CreateSpawnPiece(nextType);
    }

    public bool TryMoveLeft()
    {
        if (IsGameOver) return false;
        var moved = ActivePiece.Move(-1, 0);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryMoveRight()
    {
        if (IsGameOver) return false;
        var moved = ActivePiece.Move(1, 0);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryMoveDown()
    {
        if (IsGameOver) return false;
        var moved = ActivePiece.Move(0, 1);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryRotateClockwise()
    {
        if (IsGameOver) return false;
        var rotated = ActivePiece.RotateClockwise();
        if (!Board.CanPlace(rotated)) return false;
        ActivePiece = rotated;
        return true;
    }

    /// <summary>
    /// Performs gravity tick: tries to drop active piece by one row.
    /// If blocked, locks and spawns next. Returns true if piece was locked.
    /// </summary>
    public bool TickGravity()
    {
        if (IsGameOver) return false;
        if (!TryMoveDown())
        {
            LockAndSpawnNext();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Locks active piece, clears lines, and spawns next.
    /// Sets IsGameOver if spawn overlaps.
    /// </summary>
    public void LockAndSpawnNext()
    {
        Board.Lock(ActivePiece);
        int linesCleared = Board.ClearCompleteLines();
        if (linesCleared > 0)
            Score.ApplyLineClear(linesCleared);

        // Prepare next piece
        var newActive = NextPiece;
        var newNextType = _factory.TakeNext();
        NextPiece = _factory.CreateSpawnPiece(newNextType);
        ActivePiece = newActive;

        if (!Board.CanPlace(ActivePiece))
            IsGameOver = true;
    }

    /// <summary>
    /// Resets game to initial state.
    /// </summary>
    public void Reset()
    {
        Board = new Board();
        Score.Reset();
        _factory.Reset();
        IsGameOver = false;
        var firstType = _factory.TakeNext();
        var nextType = _factory.TakeNext();
        ActivePiece = _factory.CreateSpawnPiece(firstType);
        NextPiece = _factory.CreateSpawnPiece(nextType);
    }
}
