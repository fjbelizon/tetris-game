namespace TetrisGame.Domain;

/// <summary>
/// Tracks score, total lines cleared, and derived level / fall-interval.
/// Level rule  : level = 1 + floor(totalLines / 10)
/// Fall interval: max(1000 − (level−1) × 100, 100) ms
/// Scoring     : 1 line → 100 pts | 2 → 300 | 3 → 500 | 4 → 800
/// </summary>
public sealed class ScoreSystem
{
    public int Score { get; private set; }
    public int TotalLines { get; private set; }
    public int Level => 1 + TotalLines / 10;
    public int FallIntervalMs => Math.Max(1000 - (Level - 1) * 100, 100);

    public void ApplyLineClear(int lines)
    {
        if (lines <= 0) return;
        TotalLines += lines;
        Score += lines switch
        {
            1 => 100,
            2 => 300,
            3 => 500,
            4 => 800,
            _ => 0
        };
    }

    public void Reset()
    {
        Score = 0;
        TotalLines = 0;
    }
}
