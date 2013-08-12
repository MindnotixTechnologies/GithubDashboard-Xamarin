using System;
using System.Collections.Generic;

namespace GithubAPI
{
	/// <summary>
	/// Details the number of additions / deletions each week for the given repo.
	/// see: http://developer.github.com/v3/repos/statistics/#get-the-number-of-additions-and-deletions-per-week
	/// </summary>
	public class CodeFrequencyData : List<CodeFrequencyDataItem>
	{
		public CodeFrequencyData (IEnumerable<CodeFrequencyDataItem> data)
			: base(data)
		{
		}
	}
}

