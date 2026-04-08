using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class GameStateGameOverTests
{
    private static GameState MakeState() =>
        new(new Board(), new PieceFactory(), new ScoreSystem());

    [Fact]
    public void GameState_NotGameOver_Initially()
    {
        var state = MakeState();
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void GameState_IsGameOver_WhenSpawnBlocked()
    {
        var board = new Board();
        // Fill top rows so that any spawn position is blocked
        // Pieces spawn around row 0-1 at cols 3-6; fill those
        for (int c = 0; c < Board.Width; c++)
        {
            // Lock cells at rows 0 and 1 by placing I pieces
            var iPiece = new Piece(PieceType.I, 0, c);
            board.Lock(iPiece);
        }

        var factory = new PieceFactory();
        var score = new ScoreSystem();
        var state = new GameState(board, factory, score);

        // Trigger spawn — state should detect game over on construction or next tick
        Assert.True(state.IsGameOver);
    }

    [Fact]
    public void GameState_Reset_ClearsGameOver()
    {
        var board = new Board();
        for (int c = 0; c < Board.Width; c++)
            board.Lock(new Piece(PieceType.I, 0, c));

        var factory = new PieceFactory();
        var score = new ScoreSystem();
        var state = new GameState(board, factory, score);

        Assert.True(state.IsGameOver);

        state.Reset();

        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void GameState_Reset_ClearsScore()
    {
        var board = new Board();
        var factory = new PieceFactory();
        var score = new ScoreSystem();
        score.AddLines(4); // 800 points
        var state = new GameState(board, factory, score);

        state.Reset();

        Assert.Equal(0, state.Score);
    }

    [Fact]
    public void GameState_Reset_ResetsLevel()
    {
        var board = new Board();
        var factory = new PieceFactory();
        var score = new ScoreSystem();
        score.AddLines(10); // level 2
        var state = new GameState(board, factory, score);

        state.Reset();

        Assert.Equal(1, state.Level);
    }

    [Fact]
    public void GameState_NoMoves_WhenGameOver()
    {
        var board = new Board();
        for (int c = 0; c < Board.Width; c++)
            board.Lock(new Piece(PieceType.I, 0, c));

        var state = new GameState(board, new PieceFactory(), new ScoreSystem());
        Assert.True(state.IsGameOver);

        var prevRow = state.ActivePiece.Row;
        state.MoveDown();
        Assert.Equal(prevRow, state.ActivePiece.Row);
    }

    [Fact]
    public void GameState_Tick_ReturnsFalse_WhenGameOver()
    {
        var board = new Board();
        for (int c = 0; c < Board.Width; c++)
            board.Lock(new Piece(PieceType.I, 0, c));

        var state = new GameState(board, new PieceFactory(), new ScoreSystem());
        bool result = state.Tick();
        Assert.False(result);
    }
}
