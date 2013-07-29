using System;
using RestSharp;

namespace GithubAPI
{
	public class RepoSummaryData
	{
		public UserSummaryData owner {get;set;}
		public string name {get;set;}
		public int forks {get;set;}
		public int watchers {get;set;}
		public int open_issues {get;set;}
		public bool has_issues {get;set;}
	}

	public class UserSummaryData
	{
		public string login {get; set;}
		public string avatar_url { get; set;}
	}

	static public class RepoSummary
	{
		static public RepoSummaryData SummmaryForRepo(string owner, string repo)
		{
			// Create a client for the GitHub API
			var client = new RestClient ("https://api.github.com/");

			// The endpoint needs the owner and repo strings
			var request = new RestRequest ("repos/{owner}/{repo}", Method.GET);
			request.AddUrlSegment ("owner", owner);
			request.AddUrlSegment ("repo", repo);

			// The return is an 'array' of arrays of 3 ints each.
			IRestResponse<RepoSummaryData> response = client.Execute<RepoSummaryData> (request);

			// Send them back
			return response.Data;
		}
	}
}

