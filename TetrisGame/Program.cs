using TetrisGame.Domain;
using TetrisGame.Input;
using TetrisGame.Rendering;

Console.Title = "Tetris";
Console.Clear();

var renderer = new ConsoleRenderer();
var keyboard = new KeyboardInputProvider();

bool keepPlaying = true;
while (keepPlaying)
{
    // Initialize game components
    var board = new Board();
    var factory = new PieceFactory();
    var score = new ScoreSystem();
    var state = new GameState(board, factory, score);
    var engine = new GameEngine(state);

    renderer.Clear();
    keyboard.Clear();

    // Main game loop
    while (!state.IsGameOver)
    {
        keyboard.Poll();
        while (keyboard.TryDequeue(out ConsoleKey key))
        {
            var cmd = MapKey(key);
            engine.ProcessCommand(cmd);
        }

        engine.Update();
        renderer.Render(state);

        Thread.Sleep(16); // ~60 fps polling
    }

    // Game over: show screen and wait for S or N
    renderer.RenderGameOver(state);
    keyboard.Clear();

    keepPlaying = WaitForReplayChoice(keyboard);
}

Console.Clear();
Console.WriteLine("Gracias por jugar. ¡Hasta la próxima!");

static InputCommand MapKey(ConsoleKey key) => key switch
{
    ConsoleKey.LeftArrow  => InputCommand.MoveLeft,
    ConsoleKey.RightArrow => InputCommand.MoveRight,
    ConsoleKey.DownArrow  => InputCommand.MoveDown,
    ConsoleKey.Spacebar   => InputCommand.Rotate,
    _                     => InputCommand.None
};

static bool WaitForReplayChoice(KeyboardInputProvider keyboard)
{
    while (true)
    {
        keyboard.Poll();
        while (keyboard.TryDequeue(out ConsoleKey key))
        {
            if (key == ConsoleKey.S) return true;
            if (key == ConsoleKey.N) return false;
        }
        Thread.Sleep(50);
    }
}
