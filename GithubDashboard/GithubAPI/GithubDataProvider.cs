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
			MakeAPIRequest<WeeklyCommitData> (owner, repo, "repos/{owner}/{repo}/stats/participation", callback);
		}

		public void CodeFrequencyEntries(string owner, string repo, Action<CodeFrequencyData> callback)
		{
			MakeAPIRequest<List<List<long>>, CodeFrequencyData> (owner, repo, "repos/{owner}/{repo}/stats/code_frequency", callback, responseData => {
				return new CodeFrequencyData (responseData.Select (dp => new CodeFrequencyDataItem (dp)));
			});
		}


		public void PunchCardEntries(string owner, string repo, Action<PunchCardData> callback)
		{
			MakeAPIRequest<List<List<int>>, PunchCardData>(owner, repo, "repos/{owner}/{repo}/stats/punch_card", callback, responseData => {
							return new PunchCardData(responseData.Select(dp => new PunchCardDataEntry(dp)));
						});
		}

		public void SummmaryForRepo(string owner, string repo, Action<RepoSummaryDataItem> callback)
		{
			MakeAPIRequest<RepoSummaryDataItem> (owner, repo, "repos/{owner}/{repo}", callback);
		}

		public void RepoList(string owner, Action<RepoSummaryData> callback)
		{
			MakeAPIRequest<List<RepoSummaryDataItem>, RepoSummaryData> (owner, null, "users/{owner}/repos", callback, responseData => {
				return new RepoSummaryData(responseData);
			});
		}

		public void LanguageStatsForRepo(string owner, string repo, Action<LanguageFrequencyData> callback)
		{
			MakeAPIRequest<Dictionary<string, long>, LanguageFrequencyData>(owner, repo, "repos/{owner}/{repo}/languages", callback, responseData => {
				return new LanguageFrequencyData(responseData);
			});
		}

		#endregion


		#region Utility methods

		/// <summary>
		/// Make a request to the github API, transforming the reponse with the given transform function. The result
		/// is sent to the given callback.
		/// </summary>
		private void MakeAPIRequest<TResponse, TCallback>(string owner, string repo, string apiMethod,
			                                                  Action<TCallback> callback, Func<TResponse, TCallback> transform) 
				where TResponse : new()
				where TCallback : class
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();

			// And create the request
			var request = GetGithubRestRequest (apiMethod, owner, repo);
			client.ExecuteAsync<TResponse> (request, response => {

				// Let's log the rate limit remaining for dev purposes
				var h = response.Headers.First(header => header.Name == "X-RateLimit-Remaining");
				Console.WriteLine(h);

				if (response.Data != null)
				{
					callback(transform(response.Data));
				}
				else
				{
					callback(null);
				}
			});
		}

		/// <summary>
		/// Make a request to the github API. The result
		/// is sent to the given callback.
		/// </summary>
		private void MakeAPIRequest<TResponse>(string owner, string repo, string apiMethod,
		                                                  Action<TResponse> callback) 
			where TResponse : class, new()
		{
			MakeAPIRequest<TResponse, TResponse> (owner, repo, apiMethod, callback, responseData => responseData);
		}

		/// <summary>
		/// Create a rest client to send requests to the github API. Will authenticate if
		/// authentication tokens have been provided in the specified file
		/// </summary>
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

		/// <summary>
		/// Creates a rest request of the form {user}/{repo}. The urlPart string must be of the correct
		/// form (i.e. containing the aforementioned parts). Owner is required, repo is optional.
		/// </summary>
		private IRestRequest GetGithubRestRequest(String urlPart, String owner, String repo)
		{
			// Get the request for just this owner
			var request = new RestRequest (urlPart, Method.GET);
			if (!String.IsNullOrEmpty (repo)) {
				// Not always going to be provided with a repo
				request.AddUrlSegment ("repo", repo);
			}
			// Add the owner parameter
			request.AddUrlSegment ("owner", owner);
			// Done
			return request;
		}
		#endregion
	}
}

