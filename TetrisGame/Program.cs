using TetrisGame.Domain;
using TetrisGame.Input;
using TetrisGame.Rendering;

var renderer = new ConsoleRenderer();
var input = new KeyboardInputProvider();

while (true)
{
    var state = new GameState();
    var engine = new GameEngine(state);
    renderer.Initialize();

    // T034 – wire renderer update cycle in main runtime loop
    while (!state.IsGameOver)
    {
        var cmd = input.Poll();
        engine.Update(cmd);
        renderer.Render(state);
        renderer.RenderLevel(state.Score);
        Thread.Sleep(16); // ~60 fps cap
    }

    // T037 – game-over screen
    renderer.RenderGameOver(state.Score);

    // T038 – replay (S) and exit (N) control handling
    var reply = InputCommand.None;
    while (reply is not InputCommand.ReplayYes and not InputCommand.ReplayNo)
        reply = input.Poll();

    if (reply == InputCommand.ReplayNo) break;
    // loop back to start a new game (T039 – full reset via new GameState)
}

