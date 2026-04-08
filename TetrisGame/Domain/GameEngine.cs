namespace TetrisGame.Domain;

/// <summary>
/// Drives the game loop: accumulates elapsed time into a gravity timer
/// and dispatches input commands to the game state.
/// </summary>
public sealed class GameEngine
{
    private readonly GameState _state;
    private int _gravityAccumulatorMs;

    public GameEngine(GameState state)
    {
        _state = state;
    }

    /// <summary>Processes a single player input command.</summary>
    public void ProcessInput(InputCommand command)
    {
        if (_state.IsGameOver) return;

        switch (command)
        {
            case InputCommand.MoveLeft:
                _state.TryMoveLeft();
                break;
            case InputCommand.MoveRight:
                _state.TryMoveRight();
                break;
            case InputCommand.SoftDrop:
                if (!_state.TryMoveDown())
                    _state.LockAndSpawnNext();
                break;
            case InputCommand.RotateClockwise:
                _state.TryRotateClockwise();
                break;
        }
    }

    /// <summary>Advances the gravity timer by <paramref name="elapsedMs"/> milliseconds,
    /// performing automatic row drops whenever the fall interval is reached.</summary>
    public void Tick(int elapsedMs)
    {
        if (_state.IsGameOver) return;

        _gravityAccumulatorMs += elapsedMs;
        int interval = _state.Score.FallIntervalMs;

        while (_gravityAccumulatorMs >= interval)
        {
            _gravityAccumulatorMs -= interval;
            _state.TickGravity();
            interval = _state.Score.FallIntervalMs;
            if (_state.IsGameOver) break;
        }
    }
}
