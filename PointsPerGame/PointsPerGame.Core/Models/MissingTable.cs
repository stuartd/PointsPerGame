using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointsPerGame.Core.Names;

namespace PointsPerGame.Core.Models {
	public class MissingTable {
		public MissingTable(string league) {
			League = league;
		}

		public string League { get; set; }
	}
}
