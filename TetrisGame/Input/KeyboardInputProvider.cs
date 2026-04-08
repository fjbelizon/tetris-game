using System;

namespace TetrisGame.Input;

public sealed class KeyboardInputProvider
{
    public static Domain.InputCommand Poll()
    {
        if (!Console.KeyAvailable)
            return Domain.InputCommand.None;

        var key = Console.ReadKey(intercept: true);
        return key.Key switch
        {
            ConsoleKey.LeftArrow => Domain.InputCommand.MoveLeft,
            ConsoleKey.RightArrow => Domain.InputCommand.MoveRight,
            ConsoleKey.DownArrow => Domain.InputCommand.SoftDrop,
            ConsoleKey.Spacebar => Domain.InputCommand.RotateClockwise,
            ConsoleKey.S => Domain.InputCommand.ReplayYes,
            ConsoleKey.N => Domain.InputCommand.ReplayNo,
            _ => Domain.InputCommand.None
        };
    }

    public static Domain.InputCommand PollGameOver()
    {
        if (!Console.KeyAvailable)
            return Domain.InputCommand.None;

        var key = Console.ReadKey(intercept: true);
        return key.Key switch
        {
            ConsoleKey.S => Domain.InputCommand.ReplayYes,
            ConsoleKey.N => Domain.InputCommand.ReplayNo,
            _ => Domain.InputCommand.None
        };
    }
}
