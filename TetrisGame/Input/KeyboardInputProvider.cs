using TetrisGame.Domain;

namespace TetrisGame.Input;

public class KeyboardInputProvider
{
    public InputCommand Poll()
    {
        if (!Console.KeyAvailable)
            return InputCommand.None;

        var key = Console.ReadKey(intercept: true);

        return key.Key switch
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

    public IEnumerable<InputCommand> DrainAll()
    {
        while (Console.KeyAvailable)
        {
            var cmd = Poll();
            if (cmd != InputCommand.None)
                yield return cmd;
        }
    }
}
