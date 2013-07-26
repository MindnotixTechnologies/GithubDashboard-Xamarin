using System;
using MonoTouch.Foundation;

namespace Utilities
{
	// Date extensions from http://www.fusonic.net/en/blog/2011/07/06/monotouch-tips-and-tricks-part-1/
	public static class DateExtensions
	{
		public static NSDate ToNSDate (this DateTime dateTime)
		{
			return NSDate.FromTimeIntervalSinceReferenceDate ((dateTime - (new DateTime (2001, 1, 1, 0, 0, 0))).TotalSeconds);
		}

		public static DateTime ToDateTime(this NSDate nsDate)
		{
			return (new DateTime (2001, 1, 1, 0, 0, 0)).AddSeconds (nsDate.SecondsSinceReferenceDate);
		}

		public static DateTime FromUnixTime(this long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddSeconds(unixTime);
		}

		public static long ToUnixTime(this DateTime date)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return Convert.ToInt64((date - epoch).TotalSeconds);
		}
	}
}

