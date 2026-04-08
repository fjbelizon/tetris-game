namespace TetrisGame.Domain;

public sealed class GameEngine
{
    private readonly GameState _state;
    private long _gravityAccumulatorMs;

    public GameEngine(GameState state)
    {
        _state = state;
        _gravityAccumulatorMs = 0;
    }

    /// <summary>
    /// Advances game state by the given elapsed milliseconds.
    /// Applies gravity when accumulated time exceeds fall interval.
    /// Returns true if a gravity tick occurred.
    /// </summary>
    public bool Tick(long elapsedMs)
    {
        if (_state.IsGameOver) return false;

        _gravityAccumulatorMs += elapsedMs;
        int interval = _state.Score.CurrentFallIntervalMs();

        if (_gravityAccumulatorMs >= interval)
        {
            _gravityAccumulatorMs -= interval;
            _state.TickGravity();
            return true;
        }
        return false;
    }

    public void ResetAccumulator()
    {
        _gravityAccumulatorMs = 0;
    }
}
