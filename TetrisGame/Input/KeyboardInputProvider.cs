namespace TetrisGame.Input;

using TetrisGame.Domain;

public sealed class KeyboardInputProvider
{
    public InputCommand Poll()
    {
        if (!Console.KeyAvailable) return InputCommand.None;
        var key = Console.ReadKey(intercept: true).Key;
        return key switch
        {
            ConsoleKey.LeftArrow => InputCommand.MoveLeft,
            ConsoleKey.RightArrow => InputCommand.MoveRight,
            ConsoleKey.DownArrow => InputCommand.SoftDrop,
            ConsoleKey.Spacebar => InputCommand.RotateClockwise,
            ConsoleKey.S => InputCommand.ReplayYes,
            ConsoleKey.N => InputCommand.ReplayNo,
            _ => InputCommand.None
        };
    }
}
