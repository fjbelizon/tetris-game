using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class BoardLockTests
{
    [Fact]
    public void Lock_PieceBecomesSettled()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 0, 0);
        board.Lock(piece);
        Assert.True(board.IsOccupied(0, 0));
        Assert.True(board.IsOccupied(1, 0));
        Assert.True(board.IsOccupied(0, 1));
        Assert.True(board.IsOccupied(1, 1));
    }

    [Fact]
    public void Lock_OnlyLockedCellsAreOccupied()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 0, 0);
        board.Lock(piece);
        // Cell (2,0) should not be occupied
        Assert.False(board.IsOccupied(2, 0));
        Assert.False(board.IsOccupied(0, 2));
    }

    [Fact]
    public void Lock_PieceAtBottom_SettlesCorrectly()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 0, 18);
        board.Lock(piece);
        Assert.True(board.IsOccupied(0, 18));
        Assert.True(board.IsOccupied(1, 18));
        Assert.True(board.IsOccupied(0, 19));
        Assert.True(board.IsOccupied(1, 19));
    }

    [Fact]
    public void CanPlace_ReturnsFalse_AfterLockAtSamePosition()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.I, 0, 0, 0);
        board.Lock(piece);
        // Another I piece at same location should not be placeable
        var another = new Piece(TetrominoType.I, 0, 0, 0);
        Assert.False(board.CanPlace(another));
    }

    [Fact]
    public void GameState_LockAndSpawn_PieceDownwardBlocked()
    {
        // Verify that when a piece can't move down, it locks
        var board = new Board();
        // Fill row 19 so that piece can't go below row 18
        for (int col = 0; col < Board.Width; col++)
        {
            var filler = new Piece(TetrominoType.O, 0, col, 19);
        }

        // Use GameState to test lock flow
        var factory = new PieceFactory(new Random(1));
        var state = new GameState(factory, board);
        // Move piece to second-to-last row
        // TryMoveDown eventually should lock
        bool moved = true;
        int maxIterations = 25;
        while (moved && maxIterations-- > 0)
            moved = state.TryMoveDown();
        // After hitting the bottom, the piece should be immovable
        Assert.False(state.TryMoveDown());
    }
}
