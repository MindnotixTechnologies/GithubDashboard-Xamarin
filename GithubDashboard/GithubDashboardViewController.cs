using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using GithubAPI;
using MonoTouch.ObjCRuntime;

namespace GithubDashboard
{
	public partial class GithubDashboardViewController : UIViewController
	{
		private UIPopoverController _popover;
		private RepoSelectorViewControllerController _repoSelectorVC;
		private string _githubUserName;
		private DashboardPageOneView _pageOne;
		private DashboardPageTwoView _pageTwo;

		public GithubDashboardViewController (IntPtr handle) : base (handle)
		{

		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// make the main content area scroll over two pages
			this.scrollView.ContentSize = new SizeF (this.scrollView.Bounds.Width * 2, this.scrollView.Bounds.Height);
			this.scrollView.PagingEnabled = true;

			// add page one
			var nibObjects = NSBundle.MainBundle.LoadNib("DashboardPageOneView", this, null);
			_pageOne = (DashboardPageOneView)Runtime.GetNSObject(nibObjects.ValueAt(0));
			_pageOne.Frame = this.scrollView.Bounds;
			this.scrollView.Add(_pageOne);

			// add page two
			nibObjects = NSBundle.MainBundle.LoadNib("DashboardPageTwoView", this, null);
			_pageTwo = (DashboardPageTwoView)Runtime.GetNSObject(nibObjects.ValueAt(0));
			_pageTwo.Frame = new RectangleF (new PointF(this.scrollView.Bounds.Width, 0), this.scrollView.Bounds.Size);
			this.scrollView.Add(_pageTwo);

			// Need to set the repo for our views
			FetchDataForRepo ("tastejs", "PropertyCross");

			// Add the touch handler to the repo summary
			this.repoSummary.SetTapHandler ( nameLabelFrame => {
				if(_repoSelectorVC == null) {
					_repoSelectorVC = new RepoSelectorViewControllerController (_githubUserName, repoName => {
						InvokeOnMainThread(() => {
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
			GithubDataProvider.Instance.LanguageStatsForRepo (owner, repo, data => {
				InvokeOnMainThread (() => {
					this._pageOne.LanguageStats.RenderData(data);
				});
			});

			GithubDataProvider.Instance.PunchCardEntries (owner, repo, data => {
				InvokeOnMainThread (() => {
					this._pageOne.PunchCard.RenderData(data);
				});
			});

			GithubDataProvider.Instance.WeeklyCommitForRepo (owner, repo, data => {
				InvokeOnMainThread (() => {
					this._pageOne.WeeklyCommits.RenderData(data);
				});
			});

			GithubDataProvider.Instance.CodeFrequencyEntries (owner, repo, data => {
				InvokeOnMainThread (() => {
					this._pageOne.CodeFrequency.RenderData(data);
				});
			});

			GithubDataProvider.Instance.SummmaryForRepo (owner, repo, data => {
				InvokeOnMainThread (() => {
					this.repoSummary.RenderData(data);
				});
			});

			GithubDataProvider.Instance.Issues (owner, repo, data => {
				InvokeOnMainThread (() => {
					this._pageTwo.IssuesDataGrid.RenderData(data);
				});
			});

			GithubDataProvider.Instance.OpenMilestones (owner, repo, data => {
				InvokeOnMainThread (() => {
					this._pageTwo.MilestoneProgress.RenderData(data);
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

