using System;
using GithubAPI;
using System.Collections.Generic;

namespace GithubAPI
{
	/// <summary>
	/// Provides access to repository data / statistics via the GitHub API	/// 
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
		/// Gets the number of additions / deletions each week for the given repo.
		/// see: http://developer.github.com/v3/repos/statistics/#get-the-number-of-additions-and-deletions-per-week
		/// </summary>
		void CodeFrequencyEntries(string owner, string repo, Action<IEnumerable<CodeFrequencyEntry>> callback);

		/// <summary>
		/// Gets the number of commits per hour each day for the given repo.
		/// see: http://developer.github.com/v3/repos/statistics/#get-the-number-of-commits-per-hour-in-each-day
		/// </summary>
		void PunchCardEntries(string owner, string repo, Action<IEnumerable<PunchCardEntry>> callback);

		/// <summary>
		/// Gets the summary information for the given repo.
		/// see: http://developer.github.com/v3/repos/#get
		/// </summary>
		void SummmaryForRepo(string owner, string repo, Action<RepoSummaryData> callback);
	}
}

