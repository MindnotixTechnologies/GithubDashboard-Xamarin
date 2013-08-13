using System;
using RestSharp;
using System.Collections.Generic;

namespace GithubAPI
{
	/// <summary>
	/// Details a list of repositories e.g. from:
	/// http://developer.github.com/v3/repos/#list-user-repositories
	/// </summary>
	public class RepoSummaryData : List<RepoSummaryDataItem>
	{
		public RepoSummaryData (IEnumerable<RepoSummaryDataItem> data) :
			base (data) {
		}
	}
}
