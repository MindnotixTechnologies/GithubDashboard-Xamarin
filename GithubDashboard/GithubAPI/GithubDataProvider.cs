using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using GithubDashboard;

namespace GithubAPI
{
	static public class GithubDataProvider
	{	
		static private String BaseURL = "https://api.github.com/";

		#region API Methods to pull different data from the github API
		static public WeeklyCommitData WeeklyCommitForRepo(string owner, string repo)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/stats/participation", owner, repo);

			// The return is an 'array' of arrays of 3 ints each.
			IRestResponse<WeeklyCommitData> response = client.Execute<WeeklyCommitData> (request);

			// Send them back
			return response.Data;
		}

		static public IEnumerable<CommitData> CommitsForRepo(string owner, string repo)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/commits", owner, repo);

			var response = client.Execute<List<CommitData>>(request);

			return response.Data;
		}

		public static IEnumerable<CodeFrequencyEntry> CodeFrequencyEntries(string owner, string repo)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/stats/code_frequency", owner, repo);

			// The return is an 'array' of arrays of 3 ints each.
			IRestResponse<List<List<long>>> response = client.Execute<List<List<long>>> (request);

			// Let's convert the 2D array into an 'array' of PunchCardEntry objects
			IEnumerable<CodeFrequencyEntry> mapped =
				from dp in response.Data
					select new CodeFrequencyEntry (dp);

			// Send them back
			return mapped;
		}

		static public IEnumerable<PunchCardEntry> PunchCardEntries(string owner, string repo)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/stats/punch_card", owner, repo);

			// The return is an 'array' of arrays of 3 ints each.
			IRestResponse<List<List<int>>> response = client.Execute<List<List<int>>> (request);

			// Let's convert the 2D array into an 'array' of PunchCardEntry objects
			IEnumerable<PunchCardEntry> mapped =
				from dp in response.Data
					select new PunchCardEntry (dp);

			// Send them back
			return mapped;
		}

		static public RepoSummaryData SummmaryForRepo(string owner, string repo)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}", owner, repo);

			// The return is an 'array' of arrays of 3 ints each.
			IRestResponse<RepoSummaryData> response = client.Execute<RepoSummaryData> (request);

			// Send them back
			return response.Data;
		}
		#endregion


		#region Utility methods
		// This method could be replaced with some IoC magic
		static private IRestClient GetGithubRestClient()
		{
			return new RestClient (BaseURL);
		}

		static private IRestRequest GetGithubRestRequest(String urlPart, String owner, String repo)
		{
			// Create a request
			var request = new RestRequest (urlPart, Method.GET);
			// Add the parameters
			request.AddUrlSegment ("owner", owner);
			request.AddUrlSegment ("repo", repo);
			// Done
			return request;
		}
		#endregion
	}
}

