using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using GithubAPI.Json;

namespace GithubAPI
{
	public class GithubDataProvider : IGithubDataProvider
	{	
		public static IGithubDataProvider Instance = new GithubDataProvider(new GithubJsonFileDataProvider());

		private IGithubJsonDataProvider _dataProvider;

		public GithubDataProvider(IGithubJsonDataProvider dataProvider) 
		{
			_dataProvider = dataProvider;
		}

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

		public void OpenMilestones (string owner, string repo, Action<MilestoneData> callback)
		{
			MakeAPIRequest<List<MilestoneDataItem>, MilestoneData>(owner, repo, "repos/{owner}/{repo}/milestones", callback, responseData => {
				return new MilestoneData(responseData);
			});
		}

		public void Issues (string owner, string repo, Action<IssuesData> callback)
		{
			MakeAPIRequest<List<IssueDataItem>, IssuesData>(owner, repo, "repos/{owner}/{repo}/issues", callback, responseData => {
				return new IssuesData(responseData);
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

			_dataProvider.FetchData<TResponse> (owner, repo, apiMethod, response => {
				if (response != null)
				{
					callback(transform(response));
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

		#endregion
	}
}

