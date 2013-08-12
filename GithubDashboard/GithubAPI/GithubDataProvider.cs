using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;

namespace GithubAPI
{
	static public class GithubDataProvider
	{	
		static private String BaseURL = "https://api.github.com/";

		#region API Methods to pull different data from the github API
		public static void WeeklyCommitForRepo(string owner, string repo, Action<WeeklyCommitData> callback)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/stats/participation", owner, repo);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<WeeklyCommitData> (request, response => {
				callback(response.Data);
			});
		}

		public static void CodeFrequencyEntries(string owner, string repo, Action<IEnumerable<CodeFrequencyEntry>> callback)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/stats/code_frequency", owner, repo);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<List<List<long>>> (request, response => {
				// Let's convert the 2D array into an 'array' of PunchCardEntry objects
				var mapped =
					from dp in response.Data
						select new CodeFrequencyEntry (dp);
				// And push the results back
				callback(mapped);
			});
		}

		public static void PunchCardEntries(string owner, string repo, Action<IEnumerable<PunchCardEntry>> callback)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/stats/punch_card", owner, repo);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<List<List<int>>> (request, response => {
				// Let's convert the 2D array into an 'array' of PunchCardEntry objects
				var mapped =
					from dp in response.Data
						select new PunchCardEntry (dp);

				// Send them back
				callback(mapped);
			});
		}

		public static void SummmaryForRepo(string owner, string repo, Action<RepoSummaryData> callback)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}", owner, repo);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<RepoSummaryData> (request, response => {
				callback(response.Data);
			});
		}

		public static void RepoList(string owner, Action<IEnumerable<RepoSummaryData>> callback)
		{
			// Create a client
			var client = GetGithubRestClient ();
			// Create the request
			var request = GetGithubRestRequest ("users/{owner}/repos", owner);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<List<RepoSummaryData>> (request, response => {
				callback(response.Data);
			});
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
			// Get the request for just this owner
			var request = GetGithubRestRequest (urlPart, owner);
			// Add the repo
			request.AddUrlSegment ("repo", repo);
			// Done
			return request;
		}

		static private IRestRequest GetGithubRestRequest(String urlPart, String owner)
		{
			// Create a request
			var request = new RestRequest (urlPart, Method.GET);
			// Add the parameters
			request.AddUrlSegment ("owner", owner);
			// Done
			return request;
		}
		#endregion
	}
}

