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
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
		#endregion


		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Landscape;
		}
	}
}

