using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using Utilities;

namespace GithubAPI
{
	public class CodeFrequencyEntry
	{
		public DateTime WeekCommencing { get; set; }
		public int Additions { get; set; }
		public int Deletions { get; set; }

		public CodeFrequencyEntry (IList<long> list)
		{
			if (list.Count != 3) {
				throw new ArgumentException ("The IList must have 3 integer elements");
			}

			this.WeekCommencing = list [0].FromUnixTime ();
			this.Additions = (int)list [1];
			this.Deletions = (int)list [2];
		}

		public override string ToString ()
		{
			return string.Format ("[CodeFrequencyEntry: WeekCommencing={0}, Additions={1}, Deletions={2}]", WeekCommencing, Additions, Deletions);
		}
	}

	public static class CodeFrequency
	{
		public static IEnumerable<CodeFrequencyEntry> CodeFrequencyEntries(string owner, string repo)
		{
			// Create a client for the GitHub API
			var client = new RestClient ("https://api.github.com/");

			// The endpoint needs the owner and repo strings
			var request = new RestRequest ("repos/{owner}/{repo}/stats/code_frequency", Method.GET);
			request.AddUrlSegment ("owner", owner);
			request.AddUrlSegment ("repo", repo);

			// The return is an 'array' of arrays of 3 ints each.
			IRestResponse<List<List<long>>> response = client.Execute<List<List<long>>> (request);

			// Let's convert the 2D array into an 'array' of PunchCardEntry objects
			IEnumerable<CodeFrequencyEntry> mapped =
				from dp in response.Data
					select new CodeFrequencyEntry (dp);

			// Send them back
			return mapped;
		}
	}
}

