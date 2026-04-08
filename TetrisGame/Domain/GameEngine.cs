namespace TetrisGame.Domain;

/// <summary>
/// Manages the synchronous game loop: tick cadence and gravity accumulation.
/// </summary>
public sealed class GameEngine
{
    private readonly GameState _state;
    private long _gravityAccumulatorMs;
    private long _lastTickMs;

    public GameEngine(GameState state)
    {
        _state = state;
        _lastTickMs = Environment.TickCount64;
    }

    /// <summary>
    /// Advances the engine by real elapsed time. Applies gravity when interval is reached.
    /// </summary>
    public void Tick()
    {
        if (_state.IsGameOver) return;

        long now = Environment.TickCount64;
        long elapsed = now - _lastTickMs;
        _lastTickMs = now;

        _gravityAccumulatorMs += elapsed;
        int intervalMs = _state.Score.CurrentFallIntervalMs();

        while (_gravityAccumulatorMs >= intervalMs)
        {
            _gravityAccumulatorMs -= intervalMs;
            _state.TickGravity();
            // Recalculate interval in case level changed
            intervalMs = _state.Score.CurrentFallIntervalMs();
        }
    }

    public void ApplyCommand(InputCommand command)
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
}
