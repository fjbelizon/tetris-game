namespace TetrisGame.Domain;

/// <summary>
/// Drives the synchronous game loop: polls input, advances gravity, and triggers
/// lock-and-spawn transitions. The gravity interval is refreshed from
/// <see cref="ScoreSystem.CurrentFallIntervalMs"/> after every piece lock so that
/// a level-up takes effect immediately for the next falling piece (T033).
/// </summary>
public sealed class GameEngine
{
    private readonly GameState _state;
    private readonly int _tickMs;

    private int _gravityAccumulatorMs;
    private int _currentFallIntervalMs;

    public GameEngine(GameState state, int tickMs = 16)
    {
        _state = state;
        _tickMs = tickMs;
        _currentFallIntervalMs = _state.Score.CurrentFallIntervalMs();
    }

    /// <summary>
    /// Advances one game tick by the configured <see cref="_tickMs"/> amount.
    /// Applies any queued input command and triggers automatic gravity.
    /// </summary>
    /// <param name="command">Input command from the current tick, or <see cref="InputCommand.None"/>.</param>
    public void Tick(InputCommand command)
    {
        if (_state.IsGameOver) return;

        ProcessInput(command);
        AdvanceGravity(_tickMs);
    }

    /// <summary>
    /// Refreshes <see cref="_currentFallIntervalMs"/> from the score system.
    /// Call this after a level-up to ensure the new speed takes effect immediately.
    /// </summary>
    public void RefreshFallInterval()
    {
        _currentFallIntervalMs = _state.Score.CurrentFallIntervalMs();
    }

    private void ProcessInput(InputCommand command)
    {
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
                    LockAndRefresh();
                break;
            case InputCommand.RotateClockwise:
                _state.TryRotateClockwise();
                break;
        }
    }

    private void AdvanceGravity(int elapsedMs)
    {
        _gravityAccumulatorMs += elapsedMs;

        while (_gravityAccumulatorMs >= _currentFallIntervalMs)
        {
            _gravityAccumulatorMs -= _currentFallIntervalMs;

            if (_state.IsGameOver) break;

            int levelBefore = _state.Score.Level;

            if (!_state.TryMoveDown())
            {
                LockAndRefresh();
                break;
            }

            // After a successful gravity drop, also check for level-up
            // (lines may have been cleared if a lock happened earlier this tick).
            if (_state.Score.Level != levelBefore)
                RefreshFallInterval();
        }
    }

    private void LockAndRefresh()
    {
        int levelBefore = _state.Score.Level;
        _state.LockAndSpawnNext();

        // T033: Apply updated gravity interval immediately after level-up.
        if (_state.Score.Level != levelBefore)
            RefreshFallInterval();
    }
}
