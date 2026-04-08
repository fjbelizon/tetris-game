namespace TetrisGame.Domain;

public class GameState
{
    private readonly Board _board;
    private readonly PieceFactory _factory;
    private readonly ScoreSystem _score;

    public Board Board => _board;
    public Piece ActivePiece { get; private set; }
    public Piece NextPiece { get; private set; }
    public int Score => _score.Score;
    public int Level => _score.Level;
    public bool IsGameOver { get; private set; }

    public GameState(Board board, PieceFactory factory, ScoreSystem score)
    {
        _board = board;
        _factory = factory;
        _score = score;

        NextPiece = _factory.Next();
        ActivePiece = SpawnNext();
        if (!_board.CanPlace(ActivePiece))
            IsGameOver = true;
    }

    public void MoveLeft()
    {
        if (IsGameOver) return;
        TryMove(ActivePiece.WithOffset(0, -1));
    }

    public void MoveRight()
    {
        if (IsGameOver) return;
        TryMove(ActivePiece.WithOffset(0, 1));
    }

    public void MoveDown()
    {
        if (IsGameOver) return;
        if (!TryMove(ActivePiece.WithOffset(1, 0)))
            LockAndSpawn();
    }

    public void Rotate()
    {
        if (IsGameOver) return;
        TryMove(ActivePiece.Rotated());
    }

    public bool Tick()
    {
        if (IsGameOver) return false;
        if (!TryMove(ActivePiece.WithOffset(1, 0)))
        {
            LockAndSpawn();
            return false;
        }
        return true;
    }

    public void Reset()
    {
        _board.Reset();
        _score.Reset();
        _factory.Reset();
        IsGameOver = false;
        NextPiece = _factory.Next();
        ActivePiece = SpawnNext();
        if (!_board.CanPlace(ActivePiece))
            IsGameOver = true;
    }

    private bool TryMove(Piece candidate)
    {
        if (_board.CanPlace(candidate))
        {
            ActivePiece = candidate;
            return true;
        }
        return false;
    }

    private void LockAndSpawn()
    {
        _board.Lock(ActivePiece);
        int cleared = _board.ClearFullLines();
        if (cleared > 0)
            _score.AddLines(cleared);

        ActivePiece = SpawnNext();
        if (!_board.CanPlace(ActivePiece))
            IsGameOver = true;
    }

    private Piece SpawnNext()
    {
        var piece = NextPiece;
        NextPiece = _factory.Next();
        return piece;
    }
}
