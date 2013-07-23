using System;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using ShinobiCharts;
using System.Collections.Generic;
using GithubAPI;

namespace GithubDashboard
{
	[Register("WeeklyCommitView")]
	public class WeeklyCommitView : UIView
	{
		private class WeeklyCommitViewDatasource : SChartDataSource
		{
			private IList<SChartDataPoint> _ownerDPs;
			private IList<SChartDataPoint> _nonOwnerDPs;

			public WeeklyCommitViewDatasource(string owner, string repo)
			{
				WeeklyCommitData data = WeeklyCommitCount.WeeklyCommitForRepo(owner, repo);
				_ownerDPs = this.ConvertToDataPoints(data.Owner);
				_nonOwnerDPs = this.ConvertToDataPoints(data.Others);
			}

			#region Utility Methods
			private IList<SChartDataPoint>ConvertToDataPoints(IEnumerable<int> inp)
			{
				return new List<SChartDataPoint> (
					inp.Select ((x, i) => this.CreateDataPoint(i, x))
				);
			}

			private SChartDataPoint CreateDataPoint(int x, int y)
			{
				SChartDataPoint dp = new SChartDataPoint ();
				dp.XValue = new NSNumber (x);
				dp.YValue = new NSNumber (y);
				return dp;
			}
			#endregion

			#region SChartDatasource Protocol Methods
			public override int GetNumberOfSeries (ShinobiChart chart)
			{
				return 2;
			}

			public override int GetNumberOfDataPoints (ShinobiChart chart, int dataSeriesIndex)
			{
				return _ownerDPs.Count ();
			}

			public override SChartData GetDataPoint (ShinobiChart chart, int dataIndex, int dataSeriesIndex)
			{
				if(dataSeriesIndex == 0) {
					return _nonOwnerDPs [dataIndex];
				} else {
					return _ownerDPs [dataIndex];
				}
			}

			public override SChartSeries GetSeries (ShinobiChart chart, int dataSeriesIndex)
			{

			}
			#endregion
		}

		public WeeklyCommitView ()
		{
		}
	}
}

