using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class LevelProgressionTests
{
    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 900)]
    [InlineData(5, 600)]
    [InlineData(10, 100)]
    [InlineData(11, 100)]   // minimum cap
    [InlineData(20, 100)]   // far beyond cap
    public void CurrentFallIntervalMs_FollowsFormula(int level, int expectedMs)
    {
        var score = new ScoreSystem();
        // Advance to the target level by clearing enough lines
        int linesToClear = (level - 1) * 10;
        for (int i = 0; i < linesToClear; i++) score.ApplyLineClear(1);
        Assert.Equal(level, score.Level);
        Assert.Equal(expectedMs, score.CurrentFallIntervalMs());
    }

    [Fact]
    public void FallInterval_NeverDropsBelowMinimum()
    {
        var score = new ScoreSystem();
        for (int i = 0; i < 200; i++) score.ApplyLineClear(1);
        Assert.True(score.CurrentFallIntervalMs() >= 100);
    }
}
