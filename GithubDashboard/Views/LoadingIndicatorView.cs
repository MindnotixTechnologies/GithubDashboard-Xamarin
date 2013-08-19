using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace GithubDashboard
{
	/// <summary>
	/// A view which has an activity indicator in the center. There are
	/// methods accessible to subclasses to show and hide the indicator.
	/// </summary>
	public class LoadingIndicatorView : UIView
	{
		private UIActivityIndicatorView _actIndicator;

		public LoadingIndicatorView (IntPtr p) : base(p)
		{
			// Create an activity indicator
			_actIndicator = new UIActivityIndicatorView ();
			// Ensure the indicator remains in the center of the view
			_actIndicator.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			_actIndicator.Center = new PointF (Bounds.Width / 2, Bounds.Height / 2);
			_actIndicator.StartAnimating ();
			// Add as a subview
			Add (_actIndicator);
			// Show at construction time
			ShowIndicator ();
		}

		#region Indicator visibility methods
		protected void ShowIndicator()
		{
			_actIndicator.Hidden = false;
			BringSubviewToFront (_actIndicator);
		}

		protected void HideIndicator()
		{
			_actIndicator.Hidden = true;
		}
		#endregion
	}
}

