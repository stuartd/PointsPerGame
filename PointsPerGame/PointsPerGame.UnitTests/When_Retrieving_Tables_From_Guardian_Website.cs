﻿using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using PointsPerGame.Core.Names;
using PointsPerGame.Core.Web;

namespace PointsPerGame.UnitTests {
	[Explicit]
	[TestFixture]
	public class When_Retrieving_Tables_From_Guardian_Website {
		[TestCaseSource(typeof(TableTestCaseSource), nameof(TableTestCaseSource.Tables))]
		[Test]
		public async Task All_Tables_Should_Be_Retrievable(League league) {
			var teams = await GuardianScraper.GetResults(league);

			// If the link is wrong, the table list page is returned, which then only returns 4 values
			Assert.Greater(teams.Count, 4);
		}

		private class TableTestCaseSource {
			public static IEnumerable Tables {
				get {
					foreach (var league in Enum.GetValues(typeof(League)).Cast<League>()) {
						if (league == League.AllTopDivisions || league == League.All) {
							continue;
						}

						yield return league;
					}
				}
			}
		}
	}
}