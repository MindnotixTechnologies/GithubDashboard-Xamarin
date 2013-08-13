using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GithubAPI
{
	public class GithubDataProvider : IGithubDataProvider
	{	
		public static IGithubDataProvider Instance = new GithubDataProvider();
		private String BaseURL = "https://api.github.com/";
		private String authTokenJsonPath = "./GithubAPI/GithubAuthenticationToken.json";

		#region API Methods to pull different data from the github API

		public void WeeklyCommitForRepo(string owner, string repo, Action<WeeklyCommitData> callback)
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

		public void CodeFrequencyEntries(string owner, string repo, Action<CodeFrequencyData> callback)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/stats/code_frequency", owner, repo);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<List<List<long>>> (request, response => {
				// Let's convert the 2D array into an 'array' of PunchCardEntry objects
				var mapped = new CodeFrequencyData(response.Data.Select(dp => new CodeFrequencyDataItem (dp)));

				// And push the results back
				callback(new CodeFrequencyData(mapped));
			});
		}

		public void PunchCardEntries(string owner, string repo, Action<PunchCardData> callback)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}/stats/punch_card", owner, repo);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<List<List<int>>> (request, response => {
				// Let's convert the 2D array into an 'array' of PunchCardEntry objects
				var mapped = new PunchCardData(response.Data.Select(dp => new PunchCardDataEntry(dp)));

				// Send them back
				callback(mapped);
			});
		}

		public void SummmaryForRepo(string owner, string repo, Action<RepoSummaryDataItem> callback)
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();
			// And create the request
			var request = GetGithubRestRequest ("repos/{owner}/{repo}", owner, repo);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<RepoSummaryDataItem> (request, response => {
				callback(response.Data);
			});
		}

		public void RepoList(string owner, Action<RepoSummaryData> callback)
		{
			// Create a client
			var client = GetGithubRestClient ();
			// Create the request
			var request = GetGithubRestRequest ("users/{owner}/repos", owner);

			// Perform an async request call. Send the data back to the caller
			client.ExecuteAsync<List<RepoSummaryDataItem>> (request, response => {
				callback(new RepoSummaryData (response.Data));
			});
		}

		#endregion


		#region Utility methods

		// This method could be replaced with some IoC magic
		private IRestClient GetGithubRestClient()
		{
			var client = new RestClient (BaseURL);

			// Let's see whether we have some authentication details
			if(File.Exists (authTokenJsonPath)) {

				/* Without authentication GitHub limits your IP address to 60 requests per
				 * hour. This is a problem since changing the repo in the dashboard app requires
				 * at least 5 requests.
				 * Authenticating against github enables you to perform up to 5000 requests per
				 * hour. Therefore we check here whether or not credentials have been provided
				 * and if they have then we'll use them.
				 * 
				 * To create credentials for the app
				 * (based on https://help.github.com/articles/creating-an-access-token-for-command-line-use):
				 * 1. Visit https://github.com/settings/applications
				 * 2. Click create new token
				 * 3. Give it an appropriate name, and copy the generated token.
				 * 4. Using GithubAuthenticationToken.sample.json as a template create
				 *    GithubAuthenticationToken.json - with your username and token pasted in the
				 *    appropriate places.
				 * 5. Right click on the JSON file in Xamarin Studio, and ensure that under build
				 *    action "BundleResource" is selected.
				 * 6. Ensure that you don't commit the JSON file into source control.
				 * 
				 * Please note that this solution is not suitable for production applications - for
				 * these you should implement a user-based OAuth2 solution, as per the instructions
				 * on GitHub
				 */

				var parsedObjects = JObject.Parse (File.ReadAllText (authTokenJsonPath));
				string username = (string)parsedObjects["personal_access_token"]["user"];
				string token = (string)parsedObjects["personal_access_token"]["token"];
				if (!String.IsNullOrWhiteSpace (username) && !String.IsNullOrWhiteSpace (token)) {
					// We have credentials, so let's add them to the client
					client.Authenticator = new HttpBasicAuthenticator (username, token);
				}
			}
			return client;
		}

		private IRestRequest GetGithubRestRequest(String urlPart, String owner, String repo)
		{
			// Get the request for just this owner
			var request = GetGithubRestRequest (urlPart, owner);
			// Add the repo
			request.AddUrlSegment ("repo", repo);
			// Done
			return request;
		}

		private IRestRequest GetGithubRestRequest(String urlPart, String owner)
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

