using System;
using System.Diagnostics;
using TetrisGame.Domain;
using TetrisGame.Input;
using TetrisGame.Rendering;

var state = new GameState();
var engine = new GameEngine(state);
var renderer = new ConsoleRenderer();

Console.CursorVisible = false;
Console.Clear();

const int TickMs = 50;
var stopwatch = Stopwatch.StartNew();

while (true)
{
    if (state.IsGameOver)
    {
        renderer.DrawGameOver(state);
        var cmd = InputCommand.None;
        while (cmd == InputCommand.None)
        {
            cmd = KeyboardInputProvider.PollGameOver();
            System.Threading.Thread.Sleep(50);
        }
        if (cmd == InputCommand.ReplayNo)
            break;
        state.Reset();
        engine.ResetAccumulator();
        Console.Clear();
        stopwatch.Restart();
        continue;
    }

    long elapsed = stopwatch.ElapsedMilliseconds;
    stopwatch.Restart();

    // Process all pending input
    InputCommand input;
    while ((input = KeyboardInputProvider.Poll()) != InputCommand.None)
        state.ProcessInput(input);

    // Advance gravity
    engine.Tick(elapsed);

    // Render
    renderer.DrawFrame(state);

    System.Threading.Thread.Sleep(TickMs);
}

Console.CursorVisible = true;
Console.Clear();
