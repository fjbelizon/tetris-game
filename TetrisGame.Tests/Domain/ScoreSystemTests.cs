using TetrisGame.Domain;
using Xunit;

namespace TetrisGame.Tests.Domain;

public class ScoreSystemTests
{
    [Fact]
    public void AddLines_1Line_Scores100()
    {
        var ss = new ScoreSystem();
        ss.AddLines(1);
        Assert.Equal(100, ss.Score);
    }

    [Fact]
    public void AddLines_2Lines_Scores300()
    {
        var ss = new ScoreSystem();
        ss.AddLines(2);
        Assert.Equal(300, ss.Score);
    }

    [Fact]
    public void AddLines_3Lines_Scores500()
    {
        var ss = new ScoreSystem();
        ss.AddLines(3);
        Assert.Equal(500, ss.Score);
    }

    [Fact]
    public void AddLines_4Lines_Scores800()
    {
        var ss = new ScoreSystem();
        ss.AddLines(4);
        Assert.Equal(800, ss.Score);
    }

    [Fact]
    public void Reset_ClearsScoreAndLevel()
    {
        var ss = new ScoreSystem();
        ss.AddLines(4);
        ss.Reset();
        Assert.Equal(0, ss.Score);
        Assert.Equal(1, ss.Level);
        Assert.Equal(0, ss.TotalLinesCleared);
    }
}
