namespace TetrisGame.Domain;

public sealed class GameEngine
{
    private readonly GameState _state;
    private int _gravityAccumulatorMs;

    public GameEngine(GameState state)
    {
        _state = state;
    }

    public void Tick(int elapsedMs, InputCommand input)
    {
        if (_state.IsGameOver) return;

        ProcessInput(input);

        _gravityAccumulatorMs += elapsedMs;
        int interval = _state.ScoreSystem.CurrentFallIntervalMs();
        while (_gravityAccumulatorMs >= interval)
        {
            _gravityAccumulatorMs -= interval;
            _state.TickGravity();
            if (_state.IsGameOver) return;
            // Recalculate in case level changed after a lock+clear
            interval = _state.ScoreSystem.CurrentFallIntervalMs();
        }
    }

    public void ResetAccumulator() => _gravityAccumulatorMs = 0;

    private void ProcessInput(InputCommand input)
    {
        switch (input)
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
