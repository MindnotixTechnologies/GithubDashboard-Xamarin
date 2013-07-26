// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace GithubDashboard
{
	partial class RepoSummaryView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel forks { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ownerImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel repoName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel watchers { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ownerImage != null) {
				ownerImage.Dispose ();
				ownerImage = null;
			}

			if (repoName != null) {
				repoName.Dispose ();
				repoName = null;
			}

			if (forks != null) {
				forks.Dispose ();
				forks = null;
			}

			if (watchers != null) {
				watchers.Dispose ();
				watchers = null;
			}
		}
	}
}
