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
    public void Level_EqualsOnePlusTotalLinesDivTen(int totalLines, int expectedLevel)
    {
        var score = new ScoreSystem();
        for (int i = 0; i < totalLines; i++)
            score.ApplyLineClear(1);
        Assert.Equal(expectedLevel, score.Level);
    }

    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 900)]
    [InlineData(5, 600)]
    [InlineData(10, 100)]
    [InlineData(11, 100)] // capped at 100
    public void CurrentFallIntervalMs_FollowsFormula(int level, int expectedMs)
    {
        var score = new ScoreSystem();
        int linesToClear = (level - 1) * 10;
        for (int i = 0; i < linesToClear; i++)
            score.ApplyLineClear(1);
        Assert.Equal(level, score.Level);
        Assert.Equal(expectedMs, score.CurrentFallIntervalMs());
    }
}
