namespace TetrisGame.Domain;

public sealed class ScoreSystem
{
    public int Score { get; private set; }
    public int TotalLines { get; private set; }
    public int Level => 1 + TotalLines / 10;

    private static readonly int[] PointsTable = [0, 100, 300, 500, 800];

    public void ApplyLineClear(int count)
    {
        if (count <= 0) return;
        TotalLines += count;
        Score += count < PointsTable.Length ? PointsTable[count] : PointsTable[^1];
    }

    public int CurrentFallIntervalMs() =>
        Math.Max(1000 - (Level - 1) * 100, 100);

    public void Reset()
    {
        Score = 0;
        TotalLines = 0;
    }
}
