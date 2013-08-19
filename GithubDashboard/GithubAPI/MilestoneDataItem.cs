using System;

namespace GithubAPI
{
	/// <summary>
	/// Describes a single milestone.
	/// see: see: http://developer.github.com/v3/issues/milestones/#list-milestones-for-a-repository
	/// </summary>
	public class MilestoneDataItem
	{
		public string state {get;set;}
		public string title {get;set;}
		public string description {get;set;}
		public int open_issues {get;set;}
		public int closed_issues {get;set;}
		public DateTime created_at {get;set;}
	}
}

