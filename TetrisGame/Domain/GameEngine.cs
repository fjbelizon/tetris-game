namespace TetrisGame.Domain;

public class GameEngine
{
    private readonly GameState _state;
    private long _lastTickMs;

    public GameEngine(GameState state)
    {
        _state = state;
        _lastTickMs = CurrentMs();
    }

    public int GravityIntervalMs => _state.Level switch
    {
        _ => Math.Max(1000 - ((_state.Level - 1) * 100), 100)
    };

    public void ProcessCommand(InputCommand command)
    {
        switch (command)
        {
            case InputCommand.MoveLeft:  _state.MoveLeft();  break;
            case InputCommand.MoveRight: _state.MoveRight(); break;
            case InputCommand.MoveDown:  _state.MoveDown();  break;
            case InputCommand.Rotate:    _state.Rotate();    break;
        }
    }

    public void Update()
    {
        long now = CurrentMs();
        if (now - _lastTickMs >= GravityIntervalMs)
        {
            _state.Tick();
            _lastTickMs = now;
        }
    }

    public void ResetTimer()
    {
        _lastTickMs = CurrentMs();
    }

    private static long CurrentMs() =>
        DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}
