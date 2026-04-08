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
    [InlineData(100, 11)]
    public void Level_CorrectlyCalculatedFromTotalLines(int totalLines, int expectedLevel)
    {
        var score = new ScoreSystem();
        // Clear enough lines to reach the target total
        if (totalLines > 0)
            score.ApplyLineClear(totalLines);
        Assert.Equal(expectedLevel, score.Level);
    }

    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 900)]
    [InlineData(3, 800)]
    [InlineData(4, 700)]
    [InlineData(5, 600)]
    [InlineData(6, 500)]
    [InlineData(7, 400)]
    [InlineData(8, 300)]
    [InlineData(9, 200)]
    [InlineData(10, 100)]
    [InlineData(11, 100)] // capped at 100
    [InlineData(20, 100)] // still capped
    public void CurrentFallIntervalMs_CorrectForLevel(int level, int expectedMs)
    {
        var score = new ScoreSystem();
        // Advance to the desired level by clearing 10*(level-1) lines
        int linesToClear = (level - 1) * 10;
        if (linesToClear > 0)
            score.ApplyLineClear(linesToClear);

        Assert.Equal(level, score.Level);
        Assert.Equal(expectedMs, score.CurrentFallIntervalMs());
    }

    [Fact]
    public void FallInterval_NeverBelowMinimum100Ms()
    {
        var score = new ScoreSystem();
        // Clear many lines to get to a very high level
        score.ApplyLineClear(1000);
        Assert.True(score.CurrentFallIntervalMs() >= 100);
    }

    [Fact]
    public void Level_IncreasesAfterEvery10Lines()
    {
        var score = new ScoreSystem();
        Assert.Equal(1, score.Level);
        score.ApplyLineClear(10);
        Assert.Equal(2, score.Level);
        score.ApplyLineClear(10);
        Assert.Equal(3, score.Level);
    }
}
