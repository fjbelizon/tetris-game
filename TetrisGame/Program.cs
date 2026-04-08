using TetrisGame.Domain;
using TetrisGame.Input;
using TetrisGame.Rendering;

const int TickMs = 50;

Console.Clear();
Console.CursorVisible = false;

var state = new GameState();
var engine = new GameEngine(state);

ConsoleRenderer.RenderFrame(state);

var lastTick = Environment.TickCount64;

while (true)
{
    if (state.IsGameOver)
    {
        ConsoleRenderer.RenderGameOver(state);

        // Wait for replay or exit input
        var cmd = InputCommand.None;
        while (cmd == InputCommand.None)
        {
            cmd = KeyboardInputProvider.Poll();
            Thread.Sleep(50);
        }

        if (cmd == InputCommand.ReplayYes)
        {
            state.Reset();
            engine.ResetAccumulator();
            Console.Clear();
            lastTick = Environment.TickCount64;
        }
        else if (cmd == InputCommand.ReplayNo)
        {
            Console.CursorVisible = true;
            Console.Clear();
            break;
        }

        continue;
    }

    var now = Environment.TickCount64;
    var elapsed = (int)(now - lastTick);
    lastTick = now;

    var input = KeyboardInputProvider.Poll();
    engine.Tick(elapsed, input);

    ConsoleRenderer.RenderFrame(state);

    Thread.Sleep(TickMs);
}
