using FluentAssertions;
using NUnit.Framework;
using PointsPerGame.Core.Web;

namespace PointsPerGame.Tests;

public class GuardianTableParserTests
{
	private const string BirminghamRowHtml = """
		<td class="dcr-sz4gcj">1</td><th scope="row"><div class="dcr-2ny8cj"><picture class="dcr-twj9nh"><img srcset="https://i.guim.co.uk/img/sport/football/crests/45.png?width=20&amp;dpr=1&amp;s=none&amp;crop=none, https://i.guim.co.uk/img/sport/football/crests/45.png?width=20&amp;dpr=2&amp;s=none&amp;crop=none 2x" src="https://i.guim.co.uk/img/sport/football/crests/45.png?width=20&amp;dpr=1&amp;s=none&amp;crop=none" alt="" class="dcr-a1c492"></picture><a href="/football/birminghamcityfc" class="dcr-1d5op0q">Birmingham</a></div></th><td>0</td><td class="dcr-1nyjcuo">0</td><td class="dcr-1nyjcuo">0</td><td class="dcr-1nyjcuo">0</td><td class="dcr-1nyjcuo">0</td><td class="dcr-1nyjcuo">0</td><td>0</td><td><b class="dcr-or6g55">0</b></td><td><div class="dcr-1lbgi1b"></div></td>
		""";

	private readonly GuardianTableParser parser = new();

	[Test]
	public void Parse_Reads_Team_Details_From_Table_Structure_Not_Generated_Css_Classes()
	{
		var results = parser.Parse(TableHtml(BirminghamRowHtml));

		results.Should().ContainSingle();
		var team = results.Single();
		team.TeamName.Should().Be("Birmingham");
		team.TeamUrl.Should().Be("https://www.theguardian.com/football/birminghamcityfc/fixtures");
		team.TeamCrest.Should().Be("https://i.guim.co.uk/img/sport/football/crests/45.png?width=20&dpr=1&s=none&crop=none");
		team.Played.Should().Be(0);
		team.Won.Should().Be(0);
		team.Drawn.Should().Be(0);
		team.Lost.Should().Be(0);
		team.GoalsScored.Should().Be(0);
		team.GoalsConceded.Should().Be(0);
		team.Points.Should().Be(0);
	}

	[Test]
	public void Parse_Throws_When_Team_Link_Is_Missing()
	{
		var row = BirminghamRowHtml.Replace("<a href=\"/football/birminghamcityfc\" class=\"dcr-1d5op0q\">Birmingham</a>", string.Empty);

		var act = () => parser.Parse(TableHtml(row));

		act.Should().Throw<InvalidOperationException>()
			.WithMessage("*can't find the team link*");
	}

	[Test]
	public void Parse_Throws_When_A_Numeric_Cell_Is_Malformed()
	{
		var row = BirminghamRowHtml.Replace("<td>0</td><td class=\"dcr-1nyjcuo\">0</td>", "<td>nope</td><td class=\"dcr-1nyjcuo\">0</td>");

		var act = () => parser.Parse(TableHtml(row));

		act.Should().Throw<InvalidOperationException>()
			.WithMessage("*couldn't parse played value 'nope'*");
	}

	[Test]
	public void Parse_Decodes_Crest_Url()
	{
		var results = parser.Parse(TableHtml(BirminghamRowHtml));

		results.Single().TeamCrest.Should().NotContain("&amp;");
	}

    private static string TableHtml(string rowHtml) => $"<html><body><table><tbody><tr>{rowHtml}</tr></tbody></table></body></html>";
}