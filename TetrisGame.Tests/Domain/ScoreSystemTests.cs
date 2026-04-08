using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class ScoreSystemTests
{
    [Theory]
    [InlineData(1, 100)]
    [InlineData(2, 300)]
    [InlineData(3, 500)]
    [InlineData(4, 800)]
    public void ApplyLineClear_AwardsCorrectPoints(int lines, int expectedScore)
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(lines);
        Assert.Equal(expectedScore, score.Score);
    }

    [Fact]
    public void ApplyLineClear_AccumulatesMultipleClearEvents()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(1); // +100
        score.ApplyLineClear(2); // +300
        Assert.Equal(400, score.Score);
    }

    [Fact]
    public void ApplyLineClear_ZeroLines_DoesNothing()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(0);
        Assert.Equal(0, score.Score);
        Assert.Equal(0, score.TotalLines);
    }

    [Fact]
    public void Level_StartsAtOne()
    {
        var score = new ScoreSystem();
        Assert.Equal(1, score.Level);
    }

    [Fact]
    public void Level_IncrementsEveryTenLines()
    {
        var score = new ScoreSystem();
        for (int i = 0; i < 10; i++) score.ApplyLineClear(1);
        Assert.Equal(2, score.Level);
    }

    [Fact]
    public void Reset_ClearsScoreAndLines()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(4);
        score.Reset();
        Assert.Equal(0, score.Score);
        Assert.Equal(0, score.TotalLines);
        Assert.Equal(1, score.Level);
    }
}
