using System;
using System.Linq;
using System.Collections.Generic;
using RestSharp;

namespace GithubAPI
{
	public class PunchCardEntry
	{
		public int Day { get; set; }
		public int Hour { get; set; }
		public int Commits { get; set; }

		public PunchCardEntry(IList<int> list)
		{
			if (list.Count != 3) {
				throw new ArgumentException ("The IList must have 3 integer elements");
			}

			this.Day = list [0];
			this.Hour = list [1];
			this.Commits = list [2];
		}

	}

	static public class PunchCard
	{
		static public IList<PunchCardEntry> PunchCardEntries(string owner, string repo)
		{
			var client = new RestClient ("https://api.github.com/");

			var request = new RestRequest ("repos/{owner}/{repo}/stats/punch_card", Method.GET);
			request.AddUrlSegment ("owner", owner);
			request.AddUrlSegment ("repo", repo);

			IRestResponse<List<List<int>>> response = client.Execute<List<List<int>>> (request);

			IEnumerable<PunchCardEntry> mapped =
				from dp in response.Data
					select new PunchCardEntry (dp);


			System.Console.WriteLine (mapped);

			return null;
		}
	}
}

