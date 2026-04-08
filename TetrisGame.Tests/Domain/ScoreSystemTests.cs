using TetrisGame.Domain;

namespace TetrisGame.Tests.Domain;

public class ScoreSystemTests
{
    [Fact]
    public void InitialScore_IsZero()
    {
        var score = new ScoreSystem();
        Assert.Equal(0, score.Score);
    }

    [Fact]
    public void InitialTotalLines_IsZero()
    {
        var score = new ScoreSystem();
        Assert.Equal(0, score.TotalLines);
    }

    [Fact]
    public void InitialLevel_IsOne()
    {
        var score = new ScoreSystem();
        Assert.Equal(1, score.Level);
    }

    [Theory]
    [InlineData(1, 100)]
    [InlineData(2, 300)]
    [InlineData(3, 500)]
    [InlineData(4, 800)]
    public void ApplyLineClear_AddsCorrectScore(int lines, int expectedScore)
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(lines);
        Assert.Equal(expectedScore, score.Score);
    }

    [Fact]
    public void ApplyLineClear_AccumulatesScore()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(1); // +100
        score.ApplyLineClear(2); // +300
        Assert.Equal(400, score.Score);
    }

    [Fact]
    public void ApplyLineClear_UpdatesTotalLines()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(2);
        Assert.Equal(2, score.TotalLines);
    }

    [Fact]
    public void ApplyLineClear_ZeroLines_NoChange()
    {
        var score = new ScoreSystem();
        score.ApplyLineClear(0);
        Assert.Equal(0, score.Score);
        Assert.Equal(0, score.TotalLines);
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
