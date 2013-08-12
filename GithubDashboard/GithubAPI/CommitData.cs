using System;
using MonoTouch.Foundation;

namespace GithubAPI
{
	public class CommitData : NSObject
	{
		public string sha {get;set;}
		public Commit commit {get;set;}

		// accessors
		public string CommitMessage
		{
			get { return commit.message; }
		}

		public CommitData ()
		{

		}

		public class Commit
		{
			public string url {get;set;}
			public string message {get;set;}
			public Identity author {get;set;}
			public Identity committer {get;set;}
		}

		public class Identity
		{
			public string name {get;set;}
			public string email {get;set;}
			public DateTime date {get;set;}
		}
	}
}

