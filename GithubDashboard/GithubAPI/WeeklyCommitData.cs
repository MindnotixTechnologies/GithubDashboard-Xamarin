using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;

namespace GithubAPI
{
	/// <summary>
	/// The weekly commit statistics for a given repo.
	/// http://developer.github.com/v3/repos/statistics/#get-the-weekly-commit-count-for-the-repo-owner-and-everyone-else
	/// </summary>
	public class WeeklyCommitData
	{
		/// <summary>
		/// The number of commits by all contributors over the past 52 weeks.
		/// </summary>
		public List<int> All { get; set; }

		/// <summary>
		/// The number of commits by the repo owner over the past 52 weeks.
		/// </summary>
		public List<int> Owner { get; set; }

		/// <summary>
		/// The number of commits by the contributors other than the owner over the past 52 weeks.
		/// </summary>
		public List<int> Others
		{
			get { return new List<int>(this.All.Zip (Owner, (a, o) => (a - o))); }
		}

		public override string ToString ()
		{
			return string.Format ("[WeeklyCommitData: All={0}, Owner={1}, Others={2}]",
			                      string.Join (", ", All), string.Join (", ", Owner), string.Join (", ", Others));
		}
	}
	
}
