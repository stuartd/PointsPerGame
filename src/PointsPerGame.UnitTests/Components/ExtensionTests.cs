using Shouldly;
using NUnit.Framework;
using PointsPerGame.Core.Extensions;

namespace PointsPerGame.Tests.Components;

internal class ExtensionTests
{
	private readonly string? nullString = null;
	private readonly string? emptyString = string.Empty;
	private readonly string? spacesOnly = new("      ");
	private readonly string? stringWithValue = nameof(stringWithValue);

	[Test]
	public void HasValue_Tests()
	{
		nullString.HasValue().ShouldBeFalse();
		emptyString.HasValue().ShouldBeFalse();
		stringWithValue.HasValue().ShouldBeTrue();
	}

	[Test]
	public void IsNullOrEmptyOrWhiteSpace_Tests()
	{
		nullString.IsNullOrEmpty().ShouldBeTrue();
		nullString.HasValue().ShouldBeFalse();
		nullString.IsNullOrWhiteSpace().ShouldBeTrue();

		emptyString.IsNullOrEmpty().ShouldBeTrue();
		emptyString.HasValue().ShouldBeFalse();
		emptyString.IsNullOrWhiteSpace().ShouldBeTrue();

		"       ".IsNullOrWhiteSpace().ShouldBeTrue();
		emptyString.IsNullOrWhiteSpace().ShouldBeTrue();
		emptyString.HasValue().ShouldBeFalse();
	}
}
