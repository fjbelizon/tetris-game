namespace TetrisGame.Domain;

public sealed class GameState
{
    public Board Board { get; private set; }
    public Piece ActivePiece { get; private set; }
    public Piece NextPiece { get; private set; }
    public ScoreSystem Score { get; private set; }
    public bool IsGameOver { get; private set; }

    private PieceFactory _factory;

    public GameState()
    {
        Board = new Board();
        Score = new ScoreSystem();
        _factory = new PieceFactory();
        ActivePiece = _factory.Next();
        NextPiece = _factory.Next();
        IsGameOver = false;
    }

    // Constructor for testing with a specific factory/board
    public GameState(PieceFactory factory, Board? board = null, ScoreSystem? score = null)
    {
        _factory = factory;
        Board = board ?? new Board();
        Score = score ?? new ScoreSystem();
        ActivePiece = _factory.Next();
        NextPiece = _factory.Next();
        IsGameOver = false;
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
        Board.Lock(ActivePiece);
        int cleared = Board.ClearCompleteLines();
        if (cleared > 0)
            Score.ApplyLineClear(cleared);

        ActivePiece = NextPiece;
        NextPiece = _factory.Next();

        if (!Board.CanPlace(ActivePiece))
            IsGameOver = true;
    }

    public void ProcessInput(InputCommand command)
    {
        if (IsGameOver) return;
        switch (command)
        {
            case InputCommand.MoveLeft: TryMoveLeft(); break;
            case InputCommand.MoveRight: TryMoveRight(); break;
            case InputCommand.SoftDrop:
                if (!TryMoveDown()) LockAndSpawnNext();
                break;
            case InputCommand.RotateClockwise: TryRotateClockwise(); break;
        }
    }

    public void Reset()
    {
        Board = new Board();
        Score = new ScoreSystem();
        _factory = new PieceFactory();
        ActivePiece = _factory.Next();
        NextPiece = _factory.Next();
        IsGameOver = false;
    }
}
