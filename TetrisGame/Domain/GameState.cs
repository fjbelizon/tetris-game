namespace TetrisGame.Domain;

public sealed class GameState
{
    public Board Board { get; } = new();
    public Piece? ActivePiece { get; private set; }
    public Piece? NextPiece { get; private set; }
    public ScoreSystem Score { get; } = new();
    public bool IsGameOver { get; private set; }

    private readonly PieceFactory _factory;

    public GameState(PieceFactory? factory = null)
    {
        _factory = factory ?? new PieceFactory();
        SpawnNext();
    }

    private void SpawnNext()
    {
        var piece = NextPiece ?? _factory.Next();
        NextPiece = _factory.Next();
        if (!Board.CanPlace(piece))
        {
            IsGameOver = true;
            ActivePiece = piece;
            return;
        }
        ActivePiece = piece;
    }

    public void TryMoveLeft()
    {
        if (IsGameOver || ActivePiece is null) return;
        var moved = ActivePiece.Move(-1, 0);
        if (Board.CanPlace(moved)) ActivePiece = moved;
    }

    public void TryMoveRight()
    {
        if (IsGameOver || ActivePiece is null) return;
        var moved = ActivePiece.Move(1, 0);
        if (Board.CanPlace(moved)) ActivePiece = moved;
    }

    public void TryMoveDown()
    {
        if (IsGameOver || ActivePiece is null) return;
        var moved = ActivePiece.Move(0, 1);
        if (Board.CanPlace(moved))
            ActivePiece = moved;
        else
            LockAndSpawnNext();
    }

    public void TryRotateClockwise()
    {
        if (IsGameOver || ActivePiece is null) return;
        var rotated = ActivePiece.RotateClockwise();
        if (Board.CanPlace(rotated)) ActivePiece = rotated;
    }

    public void TickGravity() => TryMoveDown();

    public void LockAndSpawnNext()
    {
        if (ActivePiece is null) return;
        Board.Lock(ActivePiece);
        int cleared = Board.ClearCompleteLines();
        Score.ApplyLineClear(cleared);
        SpawnNext();
    }

    public void ApplyCommand(InputCommand cmd)
    {
        switch (cmd)
        {
            case InputCommand.MoveLeft: TryMoveLeft(); break;
            case InputCommand.MoveRight: TryMoveRight(); break;
            case InputCommand.SoftDrop: TryMoveDown(); break;
            case InputCommand.RotateClockwise: TryRotateClockwise(); break;
        }
    }

    public void Reset()
    {
        Board.Reset();
        Score.Reset();
        _factory.Reset();
        IsGameOver = false;
        NextPiece = null;
        ActivePiece = null;
        SpawnNext();
    }
}
