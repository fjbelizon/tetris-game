namespace TetrisGame.Domain;

public class ScoreSystem
{
    public int Score { get; private set; }
    public int Level { get; private set; } = 1;
    public int TotalLinesCleared { get; private set; }

    private static readonly int[] PointTable = [0, 100, 300, 500, 800];

    public void AddLines(int lines)
    {
        if (lines <= 0) return;
        int points = lines < PointTable.Length ? PointTable[lines] : PointTable[^1];
        Score += points;
        TotalLinesCleared += lines;
        Level = (TotalLinesCleared / 10) + 1;
    }

    public int GravityInterval =>
        Math.Max(1000 - ((Level - 1) * 100), 100);

    public void Reset()
    {
        Score = 0;
        Level = 1;
        TotalLinesCleared = 0;
    }
}
