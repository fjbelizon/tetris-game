namespace TetrisGame.Input;

using TetrisGame.Domain;

public sealed class KeyboardInputProvider
{
    /// <summary>
    /// Polls for available key presses without blocking. Returns InputCommand.None if no key.
    /// </summary>
    public InputCommand Poll()
    {
        if (!Console.KeyAvailable) return InputCommand.None;
        var key = Console.ReadKey(intercept: true);
        return key.Key switch
        {
            ConsoleKey.LeftArrow => InputCommand.MoveLeft,
            ConsoleKey.RightArrow => InputCommand.MoveRight,
            ConsoleKey.DownArrow => InputCommand.SoftDrop,
            ConsoleKey.Spacebar => InputCommand.RotateClockwise,
            ConsoleKey.S => InputCommand.ReplayYes,
            ConsoleKey.N => InputCommand.ReplayNo,
            _ => InputCommand.None,
        };
    }
}
