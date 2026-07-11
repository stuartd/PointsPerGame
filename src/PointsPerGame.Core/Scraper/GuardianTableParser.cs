using HtmlAgilityPack;
using PointsPerGame.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace PointsPerGame.Core.Web;

public sealed class GuardianTableParser
{
	public List<TeamResults> Parse(string html)
	{
		var document = new HtmlDocument();
		document.LoadHtml(html);

		return Parse(document);
	}

	public List<TeamResults> Parse(HtmlDocument document)
	{
		var table = document.DocumentNode.SelectNodes("//table").FirstOrDefault();

		if (table == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find table.");
		}

		var body = table.SelectNodes(".//tbody").FirstOrDefault();

		if (body == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find table body.");
		}

		var rows = body.SelectNodes(".//tr");

		if (rows == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find rows.");
		}

		return [.. rows.Select(ParseTeamResults)];
	}

	private static TeamResults ParseTeamResults(HtmlNode row)
	{
		var cells = row.SelectNodes("./td");

		if (cells == null || cells.Count < 9)
		{
			throw new InvalidOperationException("Guardian have changed their site again - expected at least 9 table cells in a team row.");
		}

		var teamHeader = row.SelectSingleNode("./th[@scope='row']") ?? row.SelectSingleNode("./th");

		if (teamHeader == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find the team row header.");
		}

		var anchor = teamHeader.SelectSingleNode(".//a[@href]");

		if (anchor == null)
		{
			throw new InvalidOperationException("Guardian have changed their site again - can't find the team link.");
		}

		var teamName = WebUtility.HtmlDecode(anchor.InnerText).Trim();

		if (string.IsNullOrWhiteSpace(teamName))
		{
			throw new InvalidOperationException("Guardian have changed their site again - found a team link without a team name.");
		}

		var crestSource = teamHeader.SelectSingleNode(".//img[@src]");
		var crest = crestSource?.GetAttributeValue("src", string.Empty) ?? string.Empty;
		var teamUrl = anchor.GetAttributeValue("href", string.Empty);

		return new()
		{
			TeamName = teamName,
			TeamUrl = CreateTeamFixturesUri(teamUrl).ToString(),
			TeamCrest = WebUtility.HtmlDecode(crest),
			Played = GetCellInt(cells, 1, "played"),
			Won = GetCellInt(cells, 2, "won"),
			Drawn = GetCellInt(cells, 3, "drawn"),
			Lost = GetCellInt(cells, 4, "lost"),
			GoalsScored = GetCellInt(cells, 5, "goals scored"),
			GoalsConceded = GetCellInt(cells, 6, "goals conceded"),
			Points = GetCellInt(cells, 8, "points"),
		};
	}

	private static int GetCellInt(HtmlNodeCollection cells, int index, string columnName)
	{
		var value = WebUtility.HtmlDecode(cells[index].InnerText).Trim();

		if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			throw new InvalidOperationException($"Guardian have changed their site again - couldn't parse {columnName} value '{value}'.");
		}

		return result;
	}

	private static Uri CreateTeamFixturesUri(string teamUrl)
	{
		if (string.IsNullOrWhiteSpace(teamUrl))
		{
			throw new InvalidOperationException("Guardian have changed their site again - found a team link without a URL.");
		}

		var baseUri = new Uri("https://www.theguardian.com/");
		var decodedTeamUrl = WebUtility.HtmlDecode(teamUrl).Trim();

		if (!Uri.TryCreate(baseUri, decodedTeamUrl, out var uri) ||
			(!uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) &&
			 !uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)))
		{
			throw new InvalidOperationException("Guardian have changed their site again - found an invalid team URL.");
		}

		if (!uri.AbsolutePath.EndsWith("/fixtures", StringComparison.OrdinalIgnoreCase))
		{
			var builder = new UriBuilder(uri)
			{
				Path = $"{uri.AbsolutePath.TrimEnd('/')}/fixtures",
			};

			uri = builder.Uri;
		}

		return NormalizeUri(uri.ToString());
	}

	private static Uri NormalizeUri(string url)
	{
		var uri = new Uri(url);
		var normalizedPath = string.Join("/",
			uri.AbsolutePath.Split(['/'], StringSplitOptions.RemoveEmptyEntries)
		);

		var builder = new UriBuilder(uri)
		{
			Path = normalizedPath,
		};

		return builder.Uri;
	}
}
