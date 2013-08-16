using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using RestSharp;

namespace GithubAPI.Json
{
	/// <summary>
	/// Provides Github API data via web requests.
	/// </summary>
	public class GithubJsonWebDataProvider : IGithubJsonDataProvider
	{
		private String _baseURL = "https://api.github.com/";
		private String _authTokenJsonPath = "./AppSecrets.json";

		public GithubJsonWebDataProvider ()
		{
		}

		#region IGithubRawDataProvider implementation

		public void FetchData<TCallback> (string owner, string repo, string apiMethod, Action<TCallback> callback)
			where TCallback : new()
		{
			// Create a client using the utility method
			var client = GetGithubRestClient ();

			// And create the request
			var request = GetGithubRestRequest (apiMethod, owner, repo);
			client.ExecuteAsync<TCallback> (request, response => {

				// lets log the rate limit
				var h = response.Headers.First(header => header.Name == "X-RateLimit-Remaining");
				Console.WriteLine(h);

				callback(response.Data);
			});
		}

		/// <summary>
		/// Create a rest client to send requests to the github API. Will authenticate if
		/// authentication tokens have been provided in the specified file
		/// </summary>
		private IRestClient GetGithubRestClient()
		{
			var client = new RestClient (_baseURL);

			// Let's see whether we have some authentication details
			if(File.Exists (_authTokenJsonPath)) {

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
				 * 4. Using AppSecrets.sample.json as a template create
				 *    AppSecrets.json - with your username and token pasted in the
				 *    appropriate places (as part of the github_authentication_token dictionary).
				 * 5. Right click on the JSON file in Xamarin Studio, and ensure that under build
				 *    action "BundleResource" is selected.
				 * 6. Ensure that you don't commit the JSON file into source control.
				 * 
				 * Please note that this solution is not suitable for production applications - for
				 * these you should implement a user-based OAuth2 solution, as per the instructions
				 * on GitHub
				 */


				var parsedObjects = JObject.Parse (File.ReadAllText (_authTokenJsonPath));
				string username = (string)parsedObjects["github_authentication_token"]["user"];
				string token = (string)parsedObjects["github_authentication_token"]["token"];
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

