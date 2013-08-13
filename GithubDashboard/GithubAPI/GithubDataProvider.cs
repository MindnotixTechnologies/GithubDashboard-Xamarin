using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;

namespace GithubAPI
{
	public class GithubDataProvider : IGithubDataProvider
	{	
		public static IGithubDataProvider Instance = new GithubDataProvider();

		private String BaseURL = "https://api.github.com/";

		#region API Methods to pull different data from the github API

		public void WeeklyCommitForRepo(string owner, string repo, Action<WeeklyCommitData> callback)
		{
			MakeAPIRequest<WeeklyCommitData> (owner, repo, "repos/{owner}/{repo}/stats/participation", callback);
		}

		public void CodeFrequencyEntries(string owner, string repo, Action<CodeFrequencyData> callback)
		{
			MakeAPIRequest<List<List<long>>, CodeFrequencyData>(owner, repo, "repos/{owner}/{repo}/stats/code_frequency", callback, responseData => {
				return new CodeFrequencyData(responseData.Select(dp => new CodeFrequencyDataItem (dp)));
			});
		}

		public void PunchCardEntries(string owner, string repo, Action<PunchCardData> callback)
		{
			MakeAPIRequest<List<List<int>>, PunchCardData>(owner, repo, "repos/{owner}/{repo}/stats/punch_card", callback, responseData => {
							return new PunchCardData(responseData.Select(dp => new PunchCardDataEntry(dp)));
						});
		}

		public void SummmaryForRepo(string owner, string repo, Action<RepoSummaryData> callback)
		{
			MakeAPIRequest<RepoSummaryData> (owner, repo, "repos/{owner}/{repo}", callback);
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

		// This method could be replaced with some IoC magic
		private IRestClient GetGithubRestClient()
		{
			return new RestClient (BaseURL);
		}

		private IRestRequest GetGithubRestRequest(String urlPart, String owner, String repo)
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

