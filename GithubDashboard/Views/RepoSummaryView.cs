using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using System.Drawing;


namespace GithubDashboard
{
	[Register("RepoSummaryView")]
	public partial class RepoSummaryView : UIView
	{
		public RepoSummaryView (IntPtr h) : base(h)
		{
		}

		public RepoSummaryView(NSCoder c) : base(c)
		{
			var arr = NSBundle.MainBundle.LoadNib("RepoSummaryView", this, null);
			var v = Runtime.GetNSObject(arr.ValueAt(0)) as UIView;
			v.Frame = new RectangleF(0, 0, Frame.Width, Frame.Height);
			AddSubview(v);
		}
	}
}

