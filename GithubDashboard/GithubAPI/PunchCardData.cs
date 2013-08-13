using System;
using System.Collections.Generic;

namespace GithubAPI
{
	/// <summary>
	/// Details the number of commits for over each hour for each day of the week.
	/// see: http://developer.github.com/v3/repos/statistics/#get-the-number-of-commits-per-hour-in-each-day
	/// </summary>
	public class PunchCardData : List<PunchCardDataEntry>
	{
		public PunchCardData (IEnumerable<PunchCardDataEntry> data)
			: base(data)
		{
		}
	}
}

