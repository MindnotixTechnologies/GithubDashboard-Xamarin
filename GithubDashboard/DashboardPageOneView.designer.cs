// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace GithubDashboard
{
	partial class DashboardPageOneView
	{
		[Outlet]
		GithubDashboard.LanguageStatsView languageStats { get; set; }

		[Outlet]
		GithubDashboard.WeeklyCommitView weeklyCommits { get; set; }

		[Outlet]
		GithubDashboard.PunchCardView punchCard { get; set; }

		[Outlet]
		GithubDashboard.CodeFrequencyView codeFrequency { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (languageStats != null) {
				languageStats.Dispose ();
				languageStats = null;
			}

			if (weeklyCommits != null) {
				weeklyCommits.Dispose ();
				weeklyCommits = null;
			}

			if (punchCard != null) {
				punchCard.Dispose ();
				punchCard = null;
			}

			if (codeFrequency != null) {
				codeFrequency.Dispose ();
				codeFrequency = null;
			}
		}
	}
}
