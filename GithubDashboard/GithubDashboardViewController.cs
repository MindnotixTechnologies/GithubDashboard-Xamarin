using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using GithubAPI;

namespace GithubDashboard
{
	public partial class GithubDashboardViewController : UIViewController
	{
		private UIPopoverController _popover;
		private RepoSelectorViewControllerController _repoSelectorVC;
		private string _githubUserName;

		public GithubDashboardViewController (IntPtr handle) : base (handle)
		{
		}

		#region View lifecycle
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Set the initial settings for the dashboard
			this.UpdateDashBoard ("sammyd", "sammyd.github.com");

			// Add the touch handler to the repo summary
			this.repoSummary.SetTapHandler ( nameLabelFrame => {
				if(_repoSelectorVC == null) {
					_repoSelectorVC = new RepoSelectorViewControllerController ("sammyd", repoName => {
						InvokeOnMainThread(delegate {
							_popover.Dismiss(true);
							UpdateDashBoard (_githubUserName, repoName);
						});
					});
				}
				_popover = new UIPopoverController(_repoSelectorVC);
				_popover.PresentFromRect (nameLabelFrame, this.repoSummary, UIPopoverArrowDirection.Up, true);
			});
		}
		#endregion

		private void UpdateDashBoard(string owner, string repo)
		{
			// Need to set the repo for our views
			this.punchCard.ChangeRepo (owner, repo);
			this.weeklyCommit.ChangeRepo (owner, repo);
			this.codeFrequency.ChangeRepo (owner, repo);
			this.repoSummary.ChangeRepo (owner, repo);
			// Check whether we need a new repo selector
			if (owner != _githubUserName) {
				_repoSelectorVC = null;
				_githubUserName = owner;
			}
		}


		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Landscape;
		}
	}
}

