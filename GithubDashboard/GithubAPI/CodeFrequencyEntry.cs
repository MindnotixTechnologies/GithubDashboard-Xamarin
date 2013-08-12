using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using Utilities;

namespace GithubAPI
{
	public class CodeFrequencyEntry
	{
		public DateTime WeekCommencing { get; set; }
		public int Additions { get; set; }
		public int Deletions { get; set; }

		public CodeFrequencyEntry (IList<long> list)
		{
			if (list.Count != 3) {
				throw new ArgumentException ("The IList must have 3 integer elements");
			}

			this.WeekCommencing = list [0].FromUnixTime ();
			this.Additions = (int)list [1];
			this.Deletions = (int)list [2];
		}

		public override string ToString ()
		{
			return string.Format ("[CodeFrequencyEntry: WeekCommencing={0}, Additions={1}, Deletions={2}]", WeekCommencing, Additions, Deletions);
		}
	}
	
}
