using TetrisGame.Domain;
using TetrisGame.Input;
using TetrisGame.Rendering;

const int TickMs = 16; // ~60 fps tick rate

var state = new GameState();
var engine = new GameEngine(state);
var input = new KeyboardInputProvider();
var renderer = new ConsoleRenderer();

renderer.Initialize();
renderer.RenderFrame(state);

while (true)
{
    if (state.IsGameOver)
    {
        renderer.RenderGameOver(state);

        // Block until S or N is pressed
        while (true)
        {
            var cmd = input.Poll();
            if (cmd == InputCommand.ReplayYes)
            {
                state.Reset();
                engine = new GameEngine(state);
                renderer.Initialize();
                renderer.RenderFrame(state);
                break;
            }
            else if (cmd == InputCommand.ReplayNo)
            {
                Console.Clear();
                Console.CursorVisible = true;
                return;
            }
            Thread.Sleep(TickMs);
        }
        continue;
    }

    // Process all pending input commands
    foreach (var cmd in input.DrainAll())
    {
        engine.ProcessInput(cmd);
    }

    // Advance gravity tick
    engine.Tick();

    // Render updated frame
    renderer.RenderFrame(state);

    Thread.Sleep(TickMs);
}
