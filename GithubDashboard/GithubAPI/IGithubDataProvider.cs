using System;
using GithubAPI;
using System.Collections.Generic;
using System.Collections;

namespace GithubAPI
{
	/// <summary>
	/// Provides access to repository data / statistics via the GitHub API. The data returned is potentially transformed an manipulated.
	/// see: http://developer.github.com/
	/// </summary>
	public interface IGithubDataProvider
	{
		/// <summary>
		/// Gets the weekly commit count for the given repo.
		/// see: http://developer.github.com/v3/repos/statistics/#get-the-weekly-commit-count-for-the-repo-owner-and-everyone-else
		/// </summary>
		void WeeklyCommitForRepo(string owner, string repo, Action<WeeklyCommitData> callback);

		/// <summary>
		/// Gets the language statistics for the given repo.
		/// see: http://developer.github.com/v3/repos/#list-languages
		/// </summary>
		void LanguageStatsForRepo(string owner, string repo, Action<LanguageFrequencyData> callback);

		/// <summary>
		/// Gets the number of additions / deletions each week for the given repo.
		/// see: http://developer.github.com/v3/repos/statistics/#get-the-number-of-additions-and-deletions-per-week
		/// </summary>
		void CodeFrequencyEntries(string owner, string repo, Action<CodeFrequencyData> callback);

		/// <summary>
		/// Gets the number of commits per hour each day for the given repo.
		/// see: http://developer.github.com/v3/repos/statistics/#get-the-number-of-commits-per-hour-in-each-day
		/// </summary>
		void PunchCardEntries(string owner, string repo, Action<PunchCardData> callback);

		/// <summary>
		/// Gets the summary information for the given repo.
		/// see: http://developer.github.com/v3/repos/#get
		/// </summary>
		void SummmaryForRepo(string owner, string repo, Action<RepoSummaryDataItem> callback);

		/// <summary>
		/// Gets a list of repos for a specified user
		/// see: http://developer.github.com/v3/repos/#list-user-repositories
		/// </summary>
		void RepoList (string owner, Action<RepoSummaryData> callback);
	}
}

