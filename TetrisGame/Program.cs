using TetrisGame.Domain;
using TetrisGame.Input;
using TetrisGame.Rendering;

const int TickMs = 16;

Console.CursorVisible = false;
Console.Clear();

var factory = new PieceFactory();
var state = new GameState(factory);
var engine = new GameEngine(state, TickMs);

RunGame(state, engine);

static void RunGame(GameState state, GameEngine engine)
{
    while (true)
    {
        if (state.IsGameOver)
        {
            ConsoleRenderer.RenderGameOver(state);
            HandleGameOver(state, engine);
            continue;
        }

        var command = KeyboardInputProvider.Poll();
        engine.Tick(command);
        ConsoleRenderer.RenderFrame(state);

        Thread.Sleep(16);
    }
}

static void HandleGameOver(GameState state, GameEngine engine)
{
    while (true)
    {
        var command = KeyboardInputProvider.Poll();
        if (command == InputCommand.ReplayYes)
        {
            state.Reset();
            Console.Clear();
            return;
        }
        if (command == InputCommand.ReplayNo)
        {
            Console.CursorVisible = true;
            Environment.Exit(0);
        }
        Thread.Sleep(50);
    }
}

