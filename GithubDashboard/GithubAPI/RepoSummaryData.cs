using System;
using RestSharp;

namespace GithubAPI
{
	/// <summary>
	/// The summary information for a repo.
	/// see: http://developer.github.com/v3/repos/#get
	/// </summary>
	public class RepoSummaryData
	{
		public UserSummaryData owner {get;set;}
		public string name {get;set;}
		public int forks {get;set;}
		public int watchers {get;set;}
		public int open_issues {get;set;}
		public bool has_issues {get;set;}
		public string language {get;set;}

		public class UserSummaryData
		{
			public string login {get; set;}
			public string avatar_url { get; set;}
		}
	}
	
}
