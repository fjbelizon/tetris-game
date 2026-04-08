using TetrisGame.Domain;

namespace TetrisGame.Input;

/// <summary>
/// Non-blocking keyboard polling using <see cref="Console.KeyAvailable"/>.
/// Maps <see cref="ConsoleKey"/> values to domain <see cref="InputCommand"/> values.
/// </summary>
public static class KeyboardInputProvider
{
    /// <summary>Returns the next available input command without blocking.
    /// Returns <see cref="InputCommand.None"/> if no key is pending.</summary>
    public static InputCommand Poll()
    {
        if (!Console.KeyAvailable)
            return InputCommand.None;

        var key = Console.ReadKey(intercept: true).Key;
        return key switch
        {
            ConsoleKey.LeftArrow  => InputCommand.MoveLeft,
            ConsoleKey.RightArrow => InputCommand.MoveRight,
            ConsoleKey.DownArrow  => InputCommand.SoftDrop,
            ConsoleKey.Spacebar   => InputCommand.RotateClockwise,
            ConsoleKey.S          => InputCommand.ReplayYes,
            ConsoleKey.N          => InputCommand.ReplayNo,
            _                     => InputCommand.None
        };
    }
}
