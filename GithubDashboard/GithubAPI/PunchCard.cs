using System;
using System.Linq;
using System.Collections.Generic;
using RestSharp;

namespace GithubAPI
{
	public class PunchCardEntry
	{
		private static string[] _daysOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
		public int Day { get; set; }
		public int Hour { get; set; }
		public int Commits { get; set; }

		// Convert the int to a day name
		public string DayName
		{
			get { return _daysOfWeek [this.Day]; }
		}

		public PunchCardEntry(IList<int> list)
		{
			if (list.Count != 3) {
				throw new ArgumentException ("The IList must have 3 integer elements");
			}

			this.Day = list [0];
			this.Hour = list [1];
			this.Commits = list [2];
		}

		public override string ToString ()
		{
			return string.Format ("[PunchCardEntry: Day={0}, Hour={1}, Commits={2}]", Day, Hour, Commits);
		}
	}

	static public class PunchCard
	{
		static public IEnumerable<PunchCardEntry> PunchCardEntries(string owner, string repo)
		{
			// Create a client for the GitHub API
			var client = new RestClient ("https://api.github.com/");

			// The endpoint needs the owner and repo strings
			var request = new RestRequest ("repos/{owner}/{repo}/stats/punch_card", Method.GET);
			request.AddUrlSegment ("owner", owner);
			request.AddUrlSegment ("repo", repo);

			// The return is an 'array' of arrays of 3 ints each.
			IRestResponse<List<List<int>>> response = client.Execute<List<List<int>>> (request);

			// Let's convert the 2D array into an 'array' of PunchCardEntry objects
			IEnumerable<PunchCardEntry> mapped =
				from dp in response.Data
					select new PunchCardEntry (dp);

			// Send them back
			return mapped;
		}
	}
}

