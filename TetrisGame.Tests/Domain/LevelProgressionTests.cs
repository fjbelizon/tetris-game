using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class LevelProgressionTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(9, 1)]
    [InlineData(10, 2)]
    [InlineData(19, 2)]
    [InlineData(20, 3)]
    public void Level_CalculatedCorrectly(int totalLines, int expectedLevel)
    {
        var score = new ScoreSystem();
        // Manually clear lines in groups of 1
        for (int i = 0; i < totalLines; i++)
            score.ApplyLineClear(1);
        Assert.Equal(expectedLevel, score.Level);
    }

    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 900)]
    [InlineData(5, 600)]
    [InlineData(10, 100)]
    [InlineData(11, 100)]  // capped at minimum 100
    public void CurrentFallIntervalMs_FollowsFormula(int level, int expectedMs)
    {
        var score = new ScoreSystem();
        // Reach the target level by clearing 10*(level-1) lines
        int linesToClear = (level - 1) * 10;
        for (int i = 0; i < linesToClear; i++)
            score.ApplyLineClear(1);

        Assert.Equal(expectedMs, score.CurrentFallIntervalMs());
    }

    [Fact]
    public void FallInterval_NeverBelowMinimum()
    {
        var score = new ScoreSystem();
        // Clear many lines to push level very high
        for (int i = 0; i < 200; i++)
            score.ApplyLineClear(1);

        Assert.True(score.CurrentFallIntervalMs() >= 100);
    }
}
