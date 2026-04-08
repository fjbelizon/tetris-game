using TetrisGame.Domain;
using TetrisGame.Input;
using TetrisGame.Rendering;

// Configure console for rendering
Console.CursorVisible = false;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var factory  = new PieceFactory();
var state    = new GameState(factory);
var engine   = new GameEngine(state);
var renderer = new ConsoleRenderer();

const int TickMs = 16;   // ~60 fps polling cadence

var lastTick = Environment.TickCount64;

while (true)
{
    if (state.IsGameOver)
    {
        renderer.Render(state);

        // Wait for a replay decision (S / N), ignoring other keys.
        var cmd = InputCommand.None;
        while (cmd is not (InputCommand.ReplayYes or InputCommand.ReplayNo))
        {
            cmd = KeyboardInputProvider.Poll();
            Thread.Sleep(TickMs);
        }

        if (cmd == InputCommand.ReplayNo)
            break;

        // Replay: reset state and continue.
        state.Reset();
        lastTick = Environment.TickCount64;
        continue;
    }

    // Process all available key events without blocking.
    var input = KeyboardInputProvider.Poll();
    while (input != InputCommand.None)
    {
        engine.ProcessInput(input);
        input = KeyboardInputProvider.Poll();
    }

    // Advance gravity.
    var now     = Environment.TickCount64;
    var elapsed = (int)(now - lastTick);
    lastTick = now;
    engine.Tick(elapsed);

    // Render current frame.
    renderer.Render(state);

    Thread.Sleep(TickMs);
}

Console.CursorVisible = true;
Console.Clear();
