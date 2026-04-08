namespace TetrisGame.Domain;

public sealed class ScoreSystem
{
    public int Score { get; private set; }
    public int TotalLines { get; private set; }
    public int Level => 1 + (TotalLines / 10);

    public void ApplyLineClear(int count)
    {
        TotalLines += count;
        Score += count switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            4 => 800,
            _ => 0
        };
    }

    public int CurrentFallIntervalMs() =>
        Math.Max(1000 - ((Level - 1) * 100), 100);

    public void Reset()
    {
        Score = 0;
        TotalLines = 0;
    }
}
