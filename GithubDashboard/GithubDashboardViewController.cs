using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using GithubAPI;

namespace GithubDashboard
{
	public partial class GithubDashboardViewController : UIViewController
	{
		public GithubDashboardViewController (IntPtr handle) : base (handle)
		{
		}

		#region View lifecycle
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Need to set the repo for our views
			this.punchCard.ChangeRepo ("sammyd", "sammyd.github.com");
			this.weeklyCommit.ChangeRepo ("sammyd", "sammyd.github.com");
			this.codeFrequency.ChangeRepo ("sammyd", "sammyd.github.com");
			this.repoSummary.ChangeRepo ("sammyd", "sammyd.github.com");
		}
		#endregion


		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Landscape;
		}
	}
}

