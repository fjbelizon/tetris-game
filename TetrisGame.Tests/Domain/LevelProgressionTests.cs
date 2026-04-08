using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class LevelProgressionTests
{
    [Fact]
    public void Level_StartsAt1()
    {
        var score = new ScoreSystem();
        Assert.Equal(1, score.Level);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(9, 1)]
    [InlineData(10, 2)]
    [InlineData(19, 2)]
    [InlineData(20, 3)]
    [InlineData(100, 11)]
    public void Level_EqualsOneFloorTotalLinesDividedBy10(int totalLines, int expectedLevel)
    {
        var score = new ScoreSystem();
        if (totalLines > 0)
            score.ApplyLineClear(totalLines);

        Assert.Equal(expectedLevel, score.Level);
    }

    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 900)]
    [InlineData(3, 800)]
    [InlineData(5, 600)]
    [InlineData(10, 100)]
    [InlineData(11, 100)]
    [InlineData(20, 100)]
    public void FallInterval_FollowsFormula_MaxOf1000MinusLevel1Times100And100(int level, int expectedMs)
    {
        var score = new ScoreSystem();
        int linesToReach = (level - 1) * 10;
        if (linesToReach > 0)
            score.ApplyLineClear(linesToReach);

        Assert.Equal(level, score.Level);
        Assert.Equal(expectedMs, score.CurrentFallIntervalMs());
    }

    [Fact]
    public void FallInterval_NeverDropsBelowMinimum100Ms()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(1000); // Very high level

        Assert.True(score.CurrentFallIntervalMs() >= 100);
    }

    [Fact]
    public void ApplyLineClear_IncreasesTotalLines()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(3);
        score.ApplyLineClear(2);

        Assert.Equal(5, score.TotalLines);
    }

    [Fact]
    public void Reset_ClearsScoreAndLines()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(10);

        score.Reset();

        Assert.Equal(0, score.Score);
        Assert.Equal(0, score.TotalLines);
        Assert.Equal(1, score.Level);
    }
}
