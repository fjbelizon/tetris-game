namespace TetrisGame.Domain;

/// <summary>
/// Normalised input actions understood by the game engine.
/// </summary>
public enum InputCommand
{
    None,
    MoveLeft,
    MoveRight,
    SoftDrop,
    RotateClockwise,
    ReplayYes,
    ReplayNo
}
