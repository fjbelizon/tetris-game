namespace TetrisGame.Domain;

public class GameState
{
    public Board Board { get; private set; } = new();
    public Piece ActivePiece { get; private set; }
    public Piece NextPiece { get; private set; }
    public ScoreSystem ScoreSystem { get; } = new();
    public bool IsGameOver { get; private set; }

    private readonly PieceFactory _factory;

    public GameState(PieceFactory? factory = null)
    {
        _factory = factory ?? new PieceFactory();
        ActivePiece = _factory.CreateNext();
        // Pre-create next piece preview
        NextPiece = CreatePreviewPiece(_factory.PeekNext());
    }

    public bool TryMoveLeft()
    {
        var moved = ActivePiece.Move(-1, 0);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryMoveRight()
    {
        var moved = ActivePiece.Move(1, 0);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryMoveDown()
    {
        var moved = ActivePiece.Move(0, 1);
        if (!Board.CanPlace(moved)) return false;
        ActivePiece = moved;
        return true;
    }

    public bool TryRotateClockwise()
    {
        var rotated = ActivePiece.RotateClockwise();
        if (!Board.CanPlace(rotated)) return false;
        ActivePiece = rotated;
        return true;
    }

    public void TickGravity()
    {
        if (!TryMoveDown())
        {
            LockAndSpawnNext();
        }
    }

    public void LockAndSpawnNext()
    {
        Board.Lock(ActivePiece);
        int cleared = Board.ClearCompleteLines();
        if (cleared > 0)
            ScoreSystem.ApplyLineClear(cleared);

        var next = _factory.CreateNext();
        if (!Board.CanPlace(next))
        {
            IsGameOver = true;
            ActivePiece = next;
            return;
        }

        ActivePiece = next;
        NextPiece = CreatePreviewPiece(_factory.PeekNext());
    }

    public void Reset()
    {
        Board = new Board();
        ScoreSystem.Reset();
        IsGameOver = false;
        ActivePiece = _factory.CreateNext();
        NextPiece = CreatePreviewPiece(_factory.PeekNext());
    }

    private static Piece CreatePreviewPiece(TetrominoType type)
    {
        // Preview shown at a fixed position (origin 0,0 used; rendering offsets it)
        return new Piece(type, 0, 0, 0);
    }
}
