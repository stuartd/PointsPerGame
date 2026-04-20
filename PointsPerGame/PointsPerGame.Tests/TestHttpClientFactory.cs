using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PointsPerGame.Core.Web;

namespace PointsPerGame.Tests;

public class IntegrationTests
{

//[SetUp]
//public void SetUp() {
//	services.AddSingleton<IDataSource, GuardianScraper>(); // Example service registration
//	// Build the ServiceProvider
//}

//[Test]
//public void Test_GuardianScraper() {
//	// Resolve the IDataSource service
//	var dataSource = _serviceProvider.GetRequiredService<IDataSource>();
//	// Use the resolved service in your test
//	Assert.NotNull(dataSource);
//}

///// <summary>
/////     Thinking this is not  the way to do this properly
/////     right now it's basically a PoC
/////     TODO
///// </summary>
//public class TestHttpClientFactory : IHttpClientFactory {
//	public static readonly IHttpClientFactory Instance = new TestHttpClientFactory();

//	private TestHttpClientFactory() {
//		var services = new ServiceCollection();
//		services.AddHttpClient();
//		var serviceProvider = services.BuildServiceProvider();
//		object httpClientFactory = serviceProvider.GetRequiredService(typeof(HttpClientFactory));
//	}

//	public HttpClient CreateClient(string name) {
//		return HttpClientFactory.Create();
//	}
}