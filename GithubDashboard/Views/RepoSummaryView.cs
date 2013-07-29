using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using System.Drawing;
using GithubAPI;
using System.Net;


namespace GithubDashboard
{
	[Register("RepoSummaryView")]
	public partial class RepoSummaryView : UIView
	{
		private RepoSummaryView _viewFromNib;
		private RepoSummaryData _repoData;

		public RepoSummaryView (IntPtr h) : base(h)
		{
		}

		public void ChangeRepo(string owner, string repo)
		{
			// Ensure that we have already loaded the view from the layout nib
			if(_viewFromNib == null) {
				this.LoadFromNib ();
			}
			// Update the repo data
			_repoData = RepoSummary.SummmaryForRepo (owner, repo);
			// Refresh the view
			this.UpdateViewForRepoData ();
		}


		#region Utility Methods
		private void UpdateViewForRepoData()
		{
			// Set the repo name
			_viewFromNib.repoName.Text = _repoData.owner.login + " / " + _repoData.name;
			// Set the number of forks
			_viewFromNib.forks.Text = "Forks: " + _repoData.forks.ToString ();
			// Set the number of watchers
			_viewFromNib.watchers.Text = "Watchers: " + _repoData.watchers.ToString ();
			// Update the owner's image
			this.UpdateOwnerImage (_repoData.owner.avatar_url);
		}

		private void UpdateOwnerImage(string url)
		{
			var webClient = new WebClient ();
			webClient.DownloadDataCompleted += (sender, e) => 
			{
				UIImage image = this.GetImageFromByteArray(e.Result);
				InvokeOnMainThread (() => {
					_viewFromNib.ownerImage.Image = image;
				});
			};
			webClient.DownloadDataAsync(new Uri(url));
		}

		private UIImage GetImageFromByteArray (byte[] imageBuffer)
		{
			NSData imageData = NSData.FromArray (imageBuffer);
			return UIImage.LoadFromData (imageData);
		}

		private void LoadFromNib()
		{
			var arr = NSBundle.MainBundle.LoadNib("RepoSummaryView", this, null);
			_viewFromNib = Runtime.GetNSObject(arr.ValueAt(0)) as RepoSummaryView;
			_viewFromNib.Frame = new RectangleF(0, 0, Frame.Width, Frame.Height);
			AddSubview(_viewFromNib);
		}
		#endregion
	}
}

