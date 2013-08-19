using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;

namespace GithubDashboard
{
	/// <summary>
	/// A simple 'container' view that houses a few charts
	/// </summary>
	[Register("DashboardPageOneView")]
	public partial class DashboardPageOneView : UIView
	{
		public LanguageStatsView LanguageStats { get { return this.languageStats; } }

		public WeeklyCommitView WeeklyCommits {  get { return this.weeklyCommits; } }

		public PunchCardView PunchCard {  get { return this.punchCard; } }

		public CodeFrequencyView CodeFrequency {  get { return this.codeFrequency; } }

		public DashboardPageOneView (IntPtr h) : base(h)
		{
		}
	}
}

