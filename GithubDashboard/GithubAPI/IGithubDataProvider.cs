using System;
using GithubAPI;
using System.Collections.Generic;

namespace GithubAPI
{
	public interface IGithubDataProvider
	{
		void WeeklyCommitForRepo(string owner, string repo, Action<WeeklyCommitData> callback);

		void CodeFrequencyEntries(string owner, string repo, Action<IEnumerable<CodeFrequencyEntry>> callback);

		void PunchCardEntries(string owner, string repo, Action<IEnumerable<PunchCardEntry>> callback);

		void SummmaryForRepo(string owner, string repo, Action<RepoSummaryData> callback);
	}
}

