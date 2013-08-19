using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace GithubDashboard
{
	[Register("DashboardPageTwoView")]
	public partial class DashboardPageTwoView : UIView
	{
		public IssuesDataGridView IssuesDataGrid { get{ return this.issuesDataGrid; } }

		public DashboardPageTwoView (IntPtr h) : base(h)
		{
		}
	}
}

