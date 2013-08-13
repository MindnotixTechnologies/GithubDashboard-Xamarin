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

			// Need to set the repo for our views
			FetchDataForRepo ("tastejs", "todomvc");

			// Add the touch handler to the repo summary
			this.repoSummary.SetTapHandler ( nameLabelFrame => {
				if(_repoSelectorVC == null) {
					_repoSelectorVC = new RepoSelectorViewControllerController (_githubUserName, repoName => {
						InvokeOnMainThread(delegate {
							_popover.Dismiss(true);
							FetchDataForRepo (_githubUserName, repoName);
						});
					});
				}
				_popover = new UIPopoverController(_repoSelectorVC);
				_popover.PresentFromRect (nameLabelFrame, this.repoSummary, UIPopoverArrowDirection.Up, true);
			});

		}
		#endregion


		private void FetchDataForRepo(string owner, string repo)
		{
			GithubDataProvider.Instance.PunchCardEntries (owner, repo, data => {
				InvokeOnMainThread (() => {
					this.punchCard.RenderData(data);
				});
			});

			GithubDataProvider.Instance.WeeklyCommitForRepo (owner, repo, data => {
				InvokeOnMainThread (() => {
					this.weeklyCommit.RenderData(data);
				});
			});

			GithubDataProvider.Instance.CodeFrequencyEntries (owner, repo, data => {
				InvokeOnMainThread (() => {
					this.codeFrequency.RenderData(data);
				});
			});

			GithubDataProvider.Instance.SummmaryForRepo (owner, repo, data => {
				InvokeOnMainThread (() => {
					this.repoSummary.RenderData(data);
				});
			});

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

