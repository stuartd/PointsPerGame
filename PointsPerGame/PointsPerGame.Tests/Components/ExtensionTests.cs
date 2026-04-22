using FluentAssertions;
using NUnit.Framework;
using PointsPerGame.Core.Extensions;

namespace PointsPerGame.Tests.Components
{
	internal class ExtensionTests
	{
		private readonly string? nullString = null;
		private readonly string? emptyString = string.Empty;
		private readonly string? spacesOnly = new ("      ");
		private readonly string? stringWithValue = nameof(stringWithValue);

		[Test]
		public void HasValue_Tests() {
			nullString.HasValue().Should().BeFalse();
			emptyString.HasValue().Should().BeFalse();
			stringWithValue.HasValue().Should().BeTrue();
			
			
		}

		[Test]
		public void IsNullOrEmptyOrWhiteSpace_Tests() {
			nullString.IsNullOrEmpty().Should().BeTrue();
			nullString.HasValue().Should().BeFalse();
			nullString.IsNullOrWhiteSpace().Should().BeTrue();

			emptyString.IsNullOrEmpty().Should().BeTrue();
			emptyString.HasValue().Should().BeFalse();
			emptyString.IsNullOrWhiteSpace().Should().BeTrue();

			"       ".IsNullOrWhiteSpace().Should().BeTrue();
			emptyString.IsNullOrWhiteSpace().Should().BeTrue();
			emptyString.HasValue().Should().BeFalse();
		}
	}
}
