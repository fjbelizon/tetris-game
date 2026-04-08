using System.Collections.Concurrent;

namespace TetrisGame.Input;

public class KeyboardInputProvider
{
    private readonly ConcurrentQueue<ConsoleKey> _keys = new();

    public void Poll()
    {
        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(intercept: true).Key;
            _keys.Enqueue(key);
        }
    }

    public bool TryDequeue(out ConsoleKey key) => _keys.TryDequeue(out key);

    public void Clear()
    {
        while (_keys.TryDequeue(out _)) { }
    }
}
