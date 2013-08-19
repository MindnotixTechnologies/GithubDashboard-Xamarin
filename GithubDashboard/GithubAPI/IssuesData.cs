using System;
using System.Collections.Generic;

namespace GithubAPI
{
	/// <summary>
	/// see: http://developer.github.com/v3/issues/#list-issues
	/// </summary>
	public class IssuesData : List<IssueDataItem>
	{
		public IssuesData (IEnumerable<IssueDataItem> data)
			: base(data)
		{
		}
	}
}

