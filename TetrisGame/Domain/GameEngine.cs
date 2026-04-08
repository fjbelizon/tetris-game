namespace TetrisGame.Domain;

public sealed class GameEngine
{
    private readonly GameState _state;
    private long _lastTickMs;

    public GameEngine(GameState state)
    {
        _state = state;
        _lastTickMs = Environment.TickCount64;
    }

    public GameState State => _state;

    public void Update(InputCommand command)
    {
        if (_state.IsGameOver) return;

        _state.ApplyCommand(command);

        long now = Environment.TickCount64;
        if (now - _lastTickMs >= _state.Score.CurrentFallIntervalMs())
        {
            _state.TickGravity();
            _lastTickMs = now;
        }
    }

    public void ResetTimer() => _lastTickMs = Environment.TickCount64;
}
