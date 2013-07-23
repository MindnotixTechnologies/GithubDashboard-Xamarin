using System;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using ShinobiCharts;
using System.Collections.Generic;
using GithubAPI;

namespace GithubDashboard
{
	[Register("PunchCardView")]
	public class PunchCardView : UIView
	{
		// Create a class to represent the chart datasource
		private class PunchCardViewDataSource : SChartDataSource
		{
			private IList<SChartBubbleDataPoint> _dataPoints;
			private string _owner;
			private string _repo;

			// Constructor takes an owner's name and a repo
			public PunchCardViewDataSource(string owner, string repo)
			{
				_owner = owner;
				_repo = repo;
				IEnumerable<PunchCardEntry> punchCardEntries = PunchCard.PunchCardEntries(_owner, _repo);
				_dataPoints = this.CreateDataPointsFromPunchCardEntries(punchCardEntries);
			}

			// Utility function to convert PunchCardEntries to SChartBubblePoints
			private IList<SChartBubbleDataPoint> CreateDataPointsFromPunchCardEntries(IEnumerable<PunchCardEntry> entries)
			{
				// We aren't interested in the entries which don't represent any commits
				List<SChartBubbleDataPoint> dps = new List<SChartBubbleDataPoint> (
					from entry in entries
					where entry.Commits != 0
					select this.CreateBubbleDataPointForPunchCardEntry(entry)
				);
				return dps;
			}

			// Utility Function to create punch card entry to a bubble data point
			private SChartBubbleDataPoint CreateBubbleDataPointForPunchCardEntry(PunchCardEntry entry)
			{
				SChartBubbleDataPoint dp = new SChartBubbleDataPoint ();
				dp.XValue = new NSNumber(entry.Hour);
				dp.YValue = new NSNumber(entry.Day);
				dp.Area = entry.Commits * 50.0;
				return dp;
			}

			#region SChartDatasourceDelegate methods
			public override int GetNumberOfSeries (ShinobiChart chart)
			{
				return 1;
			}

			public override SChartSeries GetSeries (ShinobiChart chart, int dataSeriesIndex)
			{
				SChartBubbleSeries chartSeries = new SChartBubbleSeries();
				chartSeries.AutoScalingBiggestBubbleDiameter = new NSNumber (40);
				return chartSeries;
			}

			public override int GetNumberOfDataPoints (ShinobiChart chart, int dataSeriesIndex)
			{
				return _dataPoints.Count ();
			}

			public override SChartData GetDataPoint (ShinobiChart chart, int dataIndex, int dataSeriesIndex)
			{
				return _dataPoints [dataIndex];
			}
			#endregion

		}

		// Variables to store chart and datasource
		private ShinobiChart _bubbleChart;
		private PunchCardViewDataSource _dataSource;

		public PunchCardView (System.Drawing.RectangleF frame) : base(frame)
		{
			this.createDatasource ();
			this.createChart ();
		}

		public PunchCardView (IntPtr p) : base(p)
		{
			this.createDatasource ();
			this.createChart ();
		}

		private void createDatasource()
		{
			// Create the data source for a sample repository
			_dataSource = new PunchCardViewDataSource("sammyd", "sammyd.github.com");
		}

		private void createChart()
		{
			_bubbleChart = new ShinobiChart (this.Bounds);

			SChartAxis xAxis = new SChartNumberAxis ();
			xAxis.RangePaddingHigh = new NSNumber (0.5);
			xAxis.RangePaddingLow = new NSNumber (0.5);
			xAxis.Title = "Hour";
			_bubbleChart.XAxis = xAxis;

			SChartAxis yAxis = new SChartNumberAxis ();
			yAxis.RangePaddingHigh = new NSNumber (0.5);
			yAxis.RangePaddingLow = new NSNumber (0.5);
			yAxis.Title = "Day";
			_bubbleChart.YAxis = yAxis;

			_bubbleChart.DataSource = _dataSource;
			this.AddSubview (_bubbleChart);
		}

	}
}

