using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class LevelProgressionTests
{
    [Theory]
    [InlineData(1, 1000)]
    [InlineData(2, 900)]
    [InlineData(5, 600)]
    [InlineData(9, 200)]
    [InlineData(10, 100)]
    [InlineData(11, 100)] // capped at minimum
    public void GravityInterval_MatchesFormula(int level, int expected)
    {
        var ss = new ScoreSystem();
        // Advance to desired level by adding lines
        if (level > 1)
            ss.AddLines((level - 1) * 10);

        Assert.Equal(level, ss.Level);
        Assert.Equal(expected, ss.GravityInterval);
    }

    [Fact]
    public void Level_IncrementsEvery10Lines()
    {
        var ss = new ScoreSystem();
        for (int i = 0; i < 10; i++)
            ss.AddLines(1);

        Assert.Equal(2, ss.Level);
    }
}
