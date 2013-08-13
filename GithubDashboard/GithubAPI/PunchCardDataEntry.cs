using System;
using System.Linq;
using System.Collections.Generic;
using RestSharp;

namespace GithubAPI
{
	/// <summary>
	/// Details the number of commits for a given day and hour.
	/// see: http://developer.github.com/v3/repos/statistics/#get-the-number-of-commits-per-hour-in-each-day
	/// </summary>
	public class PunchCardDataEntry
	{
		private static string[] _daysOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
		public int Day { get; set; }
		public int Hour { get; set; }
		public int Commits { get; set; }

		// Convert the int to a day name
		public string DayName
		{
			get { return _daysOfWeek [this.Day]; }
		}

		public PunchCardDataEntry(IList<int> list)
		{
			if (list.Count != 3) {
				throw new ArgumentException ("The IList must have 3 integer elements");
			}

			this.Day = list [0];
			this.Hour = list [1];
			this.Commits = list [2];
		}

		public override string ToString ()
		{
			return string.Format ("[PunchCardEntry: Day={0}, Hour={1}, Commits={2}]", Day, Hour, Commits);
		}
	}
	
}
