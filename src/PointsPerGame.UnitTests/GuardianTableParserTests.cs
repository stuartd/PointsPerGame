using NUnit.Framework;
using PointsPerGame.Core.Web;
using Shouldly;

namespace PointsPerGame.UnitTests;

public class GuardianTableParserTests
{
    [TestCase("/football/blackburn", "https://www.theguardian.com/football/blackburn/fixtures")]
    [TestCase("/football/blackburn/fixtures", "https://www.theguardian.com/football/blackburn/fixtures")]
    [TestCase("https://www.theguardian.com/football/blackburn", "https://www.theguardian.com/football/blackburn/fixtures")]
    public void Parse_Creates_An_Https_Fixtures_Link(string teamUrl, string expectedUrl)
    {
        var html = $$"""
            <table>
                <tbody>
                    <tr>
                        <th scope="row"><a href="{{teamUrl}}">Blackburn</a></th>
                        <td>0</td>
                        <td>1</td>
                        <td>2</td>
                        <td>3</td>
                        <td>4</td>
                        <td>5</td>
                        <td>6</td>
                        <td>7</td>
                        <td>8</td>
                    </tr>
                </tbody>
            </table>
            """;

        var result = new GuardianTableParser().Parse(html).Single();

        result.TeamUrl.ShouldBe(expectedUrl);
    }

    [Test]
    public void Parse_Allows_A_Team_Without_A_Link()
    {
        const string crestUrl = "https://i.guim.co.uk/img/sport/football/crests/26304.png?width=20&dpr=1&s=none&crop=none";
        var html = """
            <table>
                <tbody>
                    <tr>
                        <th scope="row">
                            <div>
                                <picture>
                                    <img src="https://i.guim.co.uk/img/sport/football/crests/26304.png?width=20&amp;dpr=1&amp;s=none&amp;crop=none" alt="">
                                </picture>
                                Racing Santander
                            </div>
                        </th>
                        <td>0</td>
                        <td>1</td>
                        <td>2</td>
                        <td>3</td>
                        <td>4</td>
                        <td>5</td>
                        <td>6</td>
                        <td>7</td>
                        <td>8</td>
                    </tr>
                </tbody>
            </table>
            """;

        var result = new GuardianTableParser().Parse(html).Single();

        result.TeamName.ShouldBe("Racing Santander");
        result.TeamUrl.ShouldBeEmpty();
        result.TeamCrest.ShouldBe(crestUrl);
    }
}
