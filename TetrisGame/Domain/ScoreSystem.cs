namespace TetrisGame.Domain;

public class ScoreSystem
{
    public int Score { get; private set; }
    public int TotalLines { get; private set; }
    public int Level => 1 + TotalLines / 10;

    public void ApplyLineClear(int count)
    {
        if (count <= 0) return;

        int points = count switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            4 => 800,
            _ => 800
        };

        Score += points;
        TotalLines += count;
    }

    public int CurrentFallIntervalMs()
    {
        int interval = 1000 - (Level - 1) * 100;
        return Math.Max(interval, 100);
    }

    public void Reset()
    {
        Score = 0;
        TotalLines = 0;
    }
}
