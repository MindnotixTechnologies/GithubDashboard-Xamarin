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

	static public class WeeklyCommitCount
	{
		static public WeeklyCommitData WeeklyCommitForRepo(string owner, string repo)
		{
			// Create a client for the GitHub API
			var client = new RestClient ("https://api.github.com/");

			// The endpoint needs the owner and repo strings
			var request = new RestRequest ("repos/{owner}/{repo}/stats/participation", Method.GET);
			request.AddUrlSegment ("owner", owner);
			request.AddUrlSegment ("repo", repo);

			// The return is an 'array' of arrays of 3 ints each.
			IRestResponse<WeeklyCommitData> response = client.Execute<WeeklyCommitData> (request);

			// Send them back
			return response.Data;
		}
	}
}

