using System;
using System.Collections.Generic;

namespace GithubAPI
{
	/// <summary>
	/// A collection of milsetones.
	/// see: see: http://developer.github.com/v3/issues/milestones/#list-milestones-for-a-repository
	/// </summary>
	public class MilestoneData : List<MilestoneDataItem>
	{
		public MilestoneData (IEnumerable<MilestoneDataItem> data)
			: base(data)
		{
		}
	}
}

