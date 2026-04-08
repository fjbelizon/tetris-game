namespace TetrisGame.Domain;

public class GameEngine
{
    private readonly GameState _state;
    private long _lastGravityTick;
    private int _fallIntervalMs;

    public GameState State => _state;

    public GameEngine(GameState state)
    {
        _state = state;
        _fallIntervalMs = _state.ScoreSystem.CurrentFallIntervalMs();
        _lastGravityTick = Environment.TickCount64;
    }

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

        // Update fall interval in case level changed
        UpdateFallInterval();
    }

    public void Tick()
    {
        if (_state.IsGameOver) return;

        long now = Environment.TickCount64;
        if (now - _lastGravityTick >= _fallIntervalMs)
        {
            _state.TickGravity();
            _lastGravityTick = now;
            UpdateFallInterval();
        }
    }

    private void UpdateFallInterval()
    {
        _fallIntervalMs = _state.ScoreSystem.CurrentFallIntervalMs();
    }
}
