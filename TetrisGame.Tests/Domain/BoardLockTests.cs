using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

/// <summary>
/// T015 — Lock/fixation tests when downward move is blocked.
/// Verifies that IsLocked correctly detects when a piece cannot move down,
/// and that Lock correctly settles the piece cells into the board.
/// </summary>
public class BoardLockTests
{
    // ──────────────────────────────────────────────────────────────
    // IsLocked detection
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public void IsLocked_ReturnsFalse_WhenPieceCanStillMoveDown()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 4, 0);
        Assert.False(board.IsLocked(piece));
    }

    [Fact]
    public void IsLocked_ReturnsTrue_WhenPieceHitsFloor()
    {
        var board = new Board();
        // O piece occupies rows y and y+1; at y=18 the bottom edge is at y=19 (last valid)
        var piece = new Piece(TetrominoType.O, 0, 4, 18);
        Assert.True(board.IsLocked(piece));
    }

    [Fact]
    public void IsLocked_ReturnsTrue_WhenBlockedBySettledPiece()
    {
        var board = new Board();
        // Place a settled O block at the bottom
        board.Lock(new Piece(TetrominoType.O, 0, 4, 16));
        // Active piece one row above — cannot move down
        var piece = new Piece(TetrominoType.O, 0, 4, 14);
        Assert.True(board.IsLocked(piece));
    }

    [Fact]
    public void IsLocked_ReturnsFalse_WhenSpaceAvailableBelow()
    {
        var board = new Board();
        // Settled piece far below
        board.Lock(new Piece(TetrominoType.O, 0, 4, 18));
        // Active piece still has room to move down
        var piece = new Piece(TetrominoType.O, 0, 4, 0);
        Assert.False(board.IsLocked(piece));
    }

    // ──────────────────────────────────────────────────────────────
    // Lock settles cells into board
    // ──────────────────────────────────────────────────────────────

    [Fact]
    public void Lock_SettlesCellsOnBoard()
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
    public void Lock_DoesNotAffectOtherCells()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 5, 10);
        board.Lock(piece);

        // Cells adjacent to the O piece should remain empty
        Assert.False(board.IsOccupied(4, 10));
        Assert.False(board.IsOccupied(7, 10));
        Assert.False(board.IsOccupied(5, 9));
        Assert.False(board.IsOccupied(5, 12));
    }

    [Fact]
    public void Lock_MakesPiecePositionUnavailableForNextPiece()
    {
        var board = new Board();
        var piece = new Piece(TetrominoType.O, 0, 4, 18);
        board.Lock(piece);

        var nextPiece = new Piece(TetrominoType.O, 0, 4, 18);
        Assert.False(board.CanPlace(nextPiece));
    }

    [Fact]
    public void Lock_IPieceAtFloor_SettlesAllFourCells()
    {
        var board = new Board();
        // I piece at rotation 0 is horizontal (offsets dy=0 for all cells)
        // place at row 19 — the only row that fits a horizontal I at the bottom
        var piece = new Piece(TetrominoType.I, 0, 0, 19);
        board.Lock(piece);

        Assert.True(board.IsOccupied(0, 19));
        Assert.True(board.IsOccupied(1, 19));
        Assert.True(board.IsOccupied(2, 19));
        Assert.True(board.IsOccupied(3, 19));
    }

    [Fact]
    public void IsLocked_IPieceAtRow19_IsLocked()
    {
        var board = new Board();
        // Horizontal I piece; the bottom-most cell is at y=19 (Height-1)
        // Moving down would put it at y=20 which is out of bounds → locked
        var piece = new Piece(TetrominoType.I, 0, 0, 19);
        Assert.True(board.IsLocked(piece));
    }
}
