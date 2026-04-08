namespace TetrisGame.Domain;

public sealed class GameState
{
    private readonly PieceFactory _factory;

    public Board Board { get; }
    public Piece ActivePiece { get; private set; }
    public Piece NextPiece { get; private set; }
    public ScoreSystem Score { get; }
    public bool IsGameOver { get; private set; }

    public GameState(PieceFactory? factory = null)
    {
        _factory = factory ?? new PieceFactory();
        Board = new Board();
        Score = new ScoreSystem();
        NextPiece = _factory.Next();
        ActivePiece = SpawnNext();
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

    public void TickGravity()
    {
        if (IsGameOver) return;
        if (!TryMoveDown())
            LockAndSpawnNext();
    }

    public void LockAndSpawnNext()
    {
        if (IsGameOver) return;
        Board.Lock(ActivePiece);
        int cleared = Board.ClearCompleteLines();
        if (cleared > 0)
            Score.ApplyLineClear(cleared);
        ActivePiece = SpawnNext();
        if (!Board.CanPlace(ActivePiece))
            IsGameOver = true;
    }

    public void Reset()
    {
        Board.Reset();
        Score.Reset();
        _factory.Reset();
        IsGameOver = false;
        NextPiece = _factory.Next();
        ActivePiece = SpawnNext();
    }

    private Piece SpawnNext()
    {
        var piece = NextPiece;
        NextPiece = _factory.Next();
        return piece;
    }
}
