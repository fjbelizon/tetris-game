using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class GameStateGameOverTests
{
    [Fact]
    public void GameState_InitialState_IsNotGameOver()
    {
        var state = new GameState();
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void GameState_Reset_ClearsGameOver()
    {
        var state = new GameState();

        // Force game over by repeatedly dropping pieces until spawn collision
        for (int i = 0; i < 1000 && !state.IsGameOver; i++)
            state.TickGravity();

        Assert.True(state.IsGameOver);

        state.Reset();
        Assert.False(state.IsGameOver);
    }

    [Fact]
    public void GameState_Reset_ClearsScore()
    {
        var state = new GameState();
        // Manually apply some score
        state.Score.ApplyLineClear(4);
        Assert.Equal(800, state.Score.Score);

        state.Reset();
        Assert.Equal(0, state.Score.Score);
    }

    [Fact]
    public void GameState_Reset_ClearsBoard()
    {
        var state = new GameState();

        // Lock a piece on the board
        state.Board.Lock(new Piece(TetrominoType.O, 0, 0, 18));
        Assert.True(state.Board.IsOccupied(0, 18));

        state.Reset();
        Assert.False(state.Board.IsOccupied(0, 18));
    }

    [Fact]
    public void GameState_Reset_ProvidesNewActivePiece()
    {
        var state = new GameState();
        state.Reset();
        Assert.NotNull(state.ActivePiece);
        Assert.NotNull(state.NextPiece);
    }

    [Fact]
    public void GameState_ProcessInput_IgnoredWhenGameOver()
    {
        var state = new GameState();

        // Force game over
        for (int i = 0; i < 1000 && !state.IsGameOver; i++)
            state.TickGravity();

        Assert.True(state.IsGameOver);

        int pieceX = state.ActivePiece.OriginX;
        state.ProcessInput(InputCommand.MoveLeft);
        // Position should not change when game over
        Assert.Equal(pieceX, state.ActivePiece.OriginX);
    }

    [Fact]
    public void GameState_TickGravity_IgnoredWhenGameOver()
    {
        var state = new GameState();

        // Force game over
        for (int i = 0; i < 1000 && !state.IsGameOver; i++)
            state.TickGravity();

        Assert.True(state.IsGameOver);
        int score = state.Score.Score;

        // Ticking further should not change state
        state.TickGravity();
        Assert.Equal(score, state.Score.Score);
    }

    [Fact]
    public void GameState_LockAndSpawn_TransitionsToGameOverOnCollision()
    {
        // Fill specific cells to block all possible spawn positions without creating complete rows
        // (complete rows would be cleared and prevent the game over trigger)
        var board = new Board();

        // Fill cols 3-6 in rows 0-3 - blocks spawn area without creating complete rows
        // Row 0: occupied cols 3,4,5,6 (4 of 10 - not complete)
        board.Lock(new Piece(TetrominoType.I, 0, 3, 0));  // cols 3,4,5,6 row 0
        board.Lock(new Piece(TetrominoType.I, 0, 3, 1));  // cols 3,4,5,6 row 1
        board.Lock(new Piece(TetrominoType.I, 0, 3, 2));  // cols 3,4,5,6 row 2
        board.Lock(new Piece(TetrominoType.I, 0, 3, 3));  // cols 3,4,5,6 row 3

        var factory = new PieceFactory(new Random(1));
        var state = new GameState(factory, board);

        // Keep locking and spawning until game over is detected
        int maxIterations = 20;
        while (!state.IsGameOver && maxIterations-- > 0)
            state.LockAndSpawnNext();

        Assert.True(state.IsGameOver);
    }
}
