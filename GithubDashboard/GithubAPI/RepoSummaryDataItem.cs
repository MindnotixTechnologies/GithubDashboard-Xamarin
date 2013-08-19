using System;
using RestSharp;

namespace GithubAPI
{
	/// <summary>
	/// The summary information for a repo.
	/// see: http://developer.github.com/v3/repos/#get
	/// </summary>
	public class RepoSummaryDataItem
	{
		public UserSummaryData owner {get;set;}
		public string name {get;set;}
		public int forks {get;set;}
		public int watchers {get;set;}
		public int open_issues {get;set;}
		public bool has_issues {get;set;}
		public string language {get;set;}
	}
}
