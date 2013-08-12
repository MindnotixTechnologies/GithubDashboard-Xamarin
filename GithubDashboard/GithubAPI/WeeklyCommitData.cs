using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;

namespace GithubAPI
{
	public class WeeklyCommitData
	{
		public List<int> All { get; set; }
		public List<int> Owner { get; set; }

		public List<int> Others {
			get { return new List<int>(this.All.Zip (Owner, (a, o) => (a - o))); }
		}

		public override string ToString ()
		{
			return string.Format ("[WeeklyCommitData: All={0}, Owner={1}, Others={2}]", string.Join (", ", All), string.Join (", ", Owner), string.Join (", ", Others));
		}
	}
	
}
