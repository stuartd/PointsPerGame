using PointsPerGame.Core.Models;
using PointsPerGame.Core.Names;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PointsPerGame.Core.Web;

public interface IDataSource
{
	// data source gets leagues, tables etc
	// base scraper is a *type* of data source which grabs the data from a web page
	// and guardian scraper is an implementation of that

	public Task<List<TeamResultDisplaySet>> GetResultsAsync(League league);

	// The other part is the URL that the team links to. 
	// Currently just the guardian
	// public abstract Uri GetTeamUri(string teamName);
}

public abstract class BaseScraper : IDataSource
{
	private readonly IHttpClientFactory httpClientFactory;

	protected BaseScraper(IHttpClientFactory httpClientFactory)
	{
		this.httpClientFactory = httpClientFactory;
	}

	protected async Task<Stream> GetPageStreamAsync(string url)
	{
		var httpClient = httpClientFactory.CreateClient();
		httpClient.DefaultRequestHeaders.Accept.Clear();
		httpClient.DefaultRequestHeaders.Accept.Add(new("text/html"));
		httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:128.0) Gecko/20100101 Firefox/128.0");

		var uri = new Uri(url);

		var response = await httpClient.GetAsync(uri);

		response.EnsureSuccessStatusCode();

		return await response.Content.ReadAsStreamAsync();
	}

	public abstract Task<List<TeamResultDisplaySet>> GetResultsAsync(League league);
}