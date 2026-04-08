namespace TetrisGame.Domain;

public sealed class ScoreSystem
{
    private static readonly int[] LinePoints = { 0, 100, 300, 500, 800 };

    public int Score { get; private set; }
    public int TotalLines { get; private set; }
    public int Level => 1 + TotalLines / 10;

    public void ApplyLineClear(int linesCleared)
    {
        if (linesCleared <= 0) return;
        int points = linesCleared < LinePoints.Length ? LinePoints[linesCleared] : LinePoints[^1];
        Score += points;
        TotalLines += linesCleared;
    }

    public int CurrentFallIntervalMs() => Math.Max(1000 - (Level - 1) * 100, 100);

    public void Reset()
    {
        Score = 0;
        TotalLines = 0;
    }
}
