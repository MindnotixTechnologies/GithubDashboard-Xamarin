// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace GithubDashboard
{
	[Register ("GithubDashboardViewController")]
	partial class GithubDashboardViewController
	{
		[Outlet]
		GithubDashboard.PunchCardView punchCard { get; set; }

		[Outlet]
		GithubDashboard.WeeklyCommitView weeklyCommit { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (punchCard != null) {
				punchCard.Dispose ();
				punchCard = null;
			}

			if (weeklyCommit != null) {
				weeklyCommit.Dispose ();
				weeklyCommit = null;
			}
		}
	}
}
