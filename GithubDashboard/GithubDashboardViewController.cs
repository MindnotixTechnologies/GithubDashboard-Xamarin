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
			FetchDataForRepo ("tastejs", "PropertyCross");
		}

		#endregion

		private void FetchDataForRepo(string owner, string repo)
		{
			GithubDataProvider.Instance.LanguageStatsForRepo (owner, repo, data => {
				InvokeOnMainThread (() => {
					this.languageStats.RenderData(data);
				});
			});

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
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Landscape;
		}

	}
}

