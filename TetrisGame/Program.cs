using TetrisGame.Domain;
using TetrisGame.Input;
using TetrisGame.Rendering;

const int TickMs = 16; // ~60 fps poll interval

bool keepRunning = true;

while (keepRunning)
{
    var state = new GameState();
    var engine = new GameEngine(state);
    var input = new KeyboardInputProvider();
    var renderer = new ConsoleRenderer();

    renderer.Initialize();
    renderer.RenderFrame(state);

    // Game loop
    while (!state.IsGameOver)
    {
        // Process all buffered key events
        InputCommand cmd;
        while ((cmd = input.Poll()) != InputCommand.None)
        {
            engine.ApplyCommand(cmd);
            if (state.IsGameOver) break;
        }

        // Advance gravity
        engine.Tick();

        // Render frame
        renderer.RenderFrame(state);

        Thread.Sleep(TickMs);
    }

    // Game over: show screen and wait for S or N
    renderer.RenderGameOver(state);

    bool answered = false;
    while (!answered)
    {
        var replayCmd = input.Poll();
        if (replayCmd == InputCommand.ReplayYes)
        {
            answered = true;
            keepRunning = true;
        }
        else if (replayCmd == InputCommand.ReplayNo)
        {
            answered = true;
            keepRunning = false;
        }
        else
        {
            Thread.Sleep(50);
        }
    }
}

Console.ResetColor();
Console.CursorVisible = true;
Console.Clear();
