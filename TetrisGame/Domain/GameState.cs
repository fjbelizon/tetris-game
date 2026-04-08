namespace TetrisGame.Domain;

/// <summary>
/// Aggregate runtime state of a Tetris game.
///
/// State transitions:
///   Running  → Running   on valid input / gravity tick
///   Running  → Running   on successful piece lock + spawn
///   Running  → GameOver  on spawn collision              (T036)
///   GameOver → Running   on replay reset                 (T039)
/// </summary>
public sealed class GameState
{
    private readonly PieceFactory _factory;

    public Board Board { get; }
    public Piece ActivePiece { get; private set; }
    public Piece NextPiece { get; private set; }
    public ScoreSystem Score { get; }

    /// <summary>True when a newly spawned piece cannot be placed on the board.</summary>
    public bool IsGameOver { get; private set; }

    public GameState(PieceFactory factory)
    {
        _factory = factory;
        Board = new Board();
        Score = new ScoreSystem();
        ActivePiece = _factory.Next();
        NextPiece = _factory.Next();
    }

    // ------------------------------------------------------------------
    // Input handlers  (all no-ops while game is over)
    // ------------------------------------------------------------------

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

    // ------------------------------------------------------------------
    // Gravity tick
    // ------------------------------------------------------------------

    /// <summary>Attempts to drop the active piece one row; locks it if blocked.</summary>
    public void TickGravity()
    {
        if (IsGameOver) return;
        if (!TryMoveDown())
            LockAndSpawnNext();
    }

    // ------------------------------------------------------------------
    // Lock + Spawn  (T028 / T036)
    // ------------------------------------------------------------------

    /// <summary>
    /// Locks the active piece, clears completed lines, updates score,
    /// promotes the next piece to active, draws a new next piece, and
    /// checks for a spawn collision.
    ///
    /// T036: If the newly promoted active piece cannot be placed on the board
    /// (spawn-collision), <see cref="IsGameOver"/> is set to true and all
    /// subsequent input / tick operations become no-ops.
    /// </summary>
    public void LockAndSpawnNext()
    {
        Board.Lock(ActivePiece);

        int cleared = Board.ClearCompleteLines();
        Score.ApplyLineClear(cleared);

        // Promote next piece to active and pull a fresh next from the bag.
        ActivePiece = NextPiece;
        NextPiece = _factory.Next();

        // T036: spawn-collision game-over check.
        if (!Board.CanPlace(ActivePiece))
            IsGameOver = true;
    }

    // ------------------------------------------------------------------
    // Full reset  (T039)
    // ------------------------------------------------------------------

    /// <summary>Resets board, score, level and piece bag to a fresh game state.</summary>
    public void Reset()
    {
        Board.Reset();
        Score.Reset();
        IsGameOver = false;
        ActivePiece = _factory.Next();
        NextPiece = _factory.Next();
    }
}
