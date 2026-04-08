namespace TetrisGame.Domain;

public sealed class ScoreSystem
{
    private static readonly int[] LinePoints = [0, 100, 300, 500, 800];

    public int Score { get; private set; }
    public int TotalLines { get; private set; }
    public int Level => 1 + (TotalLines / 10);

    public void ApplyLineClear(int linesCleared)
    {
        if (linesCleared < 0 || linesCleared > 4)
            throw new ArgumentOutOfRangeException(nameof(linesCleared), "Must be between 0 and 4.");
        TotalLines += linesCleared;
        Score += LinePoints[linesCleared];
    }

    public int CurrentFallIntervalMs()
    {
        int interval = 1000 - ((Level - 1) * 100);
        return Math.Max(interval, 100);
    }

    public void Reset()
    {
        Score = 0;
        TotalLines = 0;
    }
}
