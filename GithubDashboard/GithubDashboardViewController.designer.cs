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
		GithubDashboard.RepoSummaryView repoSummary { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView scrollView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (repoSummary != null) {
				repoSummary.Dispose ();
				repoSummary = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
		}
	}
}
