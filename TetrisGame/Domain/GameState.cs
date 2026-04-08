namespace TetrisGame.Domain;

public sealed class GameState
{
    public Board Board { get; }
    public Piece? ActivePiece { get; private set; }
    public Piece? NextPiece { get; private set; }
    public ScoreSystem Score { get; }
    public bool IsGameOver { get; private set; }

    private readonly PieceFactory _factory;

    public GameState(PieceFactory factory)
    {
        _factory = factory;
        Board = new Board();
        Score = new ScoreSystem();
        SpawnNext();
    }

    public bool TryMoveLeft()
    {
        if (IsGameOver || ActivePiece is null) return false;
        var moved = ActivePiece.Move(-1, 0);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryMoveRight()
    {
        if (IsGameOver || ActivePiece is null) return false;
        var moved = ActivePiece.Move(1, 0);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryMoveDown()
    {
        if (IsGameOver || ActivePiece is null) return false;
        var moved = ActivePiece.Move(0, 1);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryRotateClockwise()
    {
        if (IsGameOver || ActivePiece is null) return false;
        var rotated = ActivePiece.RotateClockwise();
        if (!Board.CanPlace(rotated)) return false;
        ActivePiece = rotated;
        return true;
    }

    public bool TickGravity()
    {
        if (IsGameOver || ActivePiece is null) return false;

        if (TryMoveDown()) return true;

        LockAndSpawnNext();
        return false;
    }

    public void LockAndSpawnNext()
    {
        if (ActivePiece is null) return;

        Board.Lock(ActivePiece);
        int lines = Board.ClearCompleteLines();
        if (lines > 0) Score.ApplyLineClear(lines);

        SpawnNext();
    }

    private void SpawnNext()
    {
        var piece = NextPiece ?? _factory.TakeNext();
        NextPiece = _factory.TakeNext();

        if (!Board.CanPlace(piece))
        {
            IsGameOver = true;
            ActivePiece = piece;
            return;
        }

        ActivePiece = piece;
    }

    public void Reset()
    {
        IsGameOver = false;
        Score.Reset();
        Board.Reset();
        NextPiece = null;
        SpawnNext();
    }
}
