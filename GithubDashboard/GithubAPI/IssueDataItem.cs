using System;
using MonoTouch.Foundation;

namespace GithubAPI
{
	/// <summary>
	/// see: http://developer.github.com/v3/issues/#list-issues
	/// </summary>
	public class IssueDataItem : NSObject
	{
		public UserSummaryData user {get;set;}
		public string title {get;set;}
		public string body {get;set;}
		public DateTime created_at {get;set;}

		public IssueDataItem ()
		{

		}
	}
}

