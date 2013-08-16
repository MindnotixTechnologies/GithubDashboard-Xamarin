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
	public partial class RepoSummaryView : UIView, IDataView<RepoSummaryDataItem>
	{
		private RepoSummaryView _viewFromNib;
		private RepoSummaryDataItem _repoData;
		private UITapGestureRecognizer _tapRecogniser;
		private string _currentlyDisplayedOwnerImageURL;

		public RepoSummaryView (IntPtr h) : base(h)
		{
		}

		public void RenderData(RepoSummaryDataItem data)
		{
			// Ensure that we have already loaded the view from the layout nib
			if(_viewFromNib == null) {
				this.LoadFromNib ();
			}

			_repoData = data;

			// Refresh the view
			this.UpdateViewForRepoData ();
		}

		public void SetTapHandler(Action<RectangleF> tapHandler)
		{
			if (_viewFromNib == null) {
				LoadFromNib ();
			}

			if (_tapRecogniser != null) {
				// If we've already been called then remove the old one
				_viewFromNib.repoName.RemoveGestureRecognizer (_tapRecogniser);
			}
			// Create a tap recogniser which will call our tapHandler callback
			_tapRecogniser = new UITapGestureRecognizer (r => {
				tapHandler (_viewFromNib.repoName.Frame);
			});
			// Add to the repoName label
			_viewFromNib.repoName.AddGestureRecognizer ( _tapRecogniser );
			_viewFromNib.repoName.UserInteractionEnabled = true;
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
			if (url != _currentlyDisplayedOwnerImageURL) {
				var webClient = new WebClient ();
				webClient.DownloadDataCompleted += (sender, e) => 
				{
					UIImage image = this.GetImageFromByteArray (e.Result);
					InvokeOnMainThread (() => {
						_viewFromNib.ownerImage.Image = image;
						_currentlyDisplayedOwnerImageURL = url;
					});
				};
				webClient.DownloadDataAsync (new Uri (url));
			}
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

