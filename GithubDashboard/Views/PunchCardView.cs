using System;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using ShinobiCharts;
using System.Collections.Generic;
using GithubAPI;
using System.Drawing;

namespace GithubDashboard
{
	[Register("PunchCardView")]
	public class PunchCardView : UIView, IDataView<PunchCardData>
	{
		// Create a class to represent the chart datasource
		private class PunchCardViewDataSource : SChartDataSource
		{
			private IList<SChartBubbleDataPoint> _dataPoints;

			// Constructor takes an owner's name and a repo
			public PunchCardViewDataSource(IEnumerable<PunchCardDataEntry> punchCardEntries)
			{
				_dataPoints = this.CreateDataPointsFromPunchCardEntries(punchCardEntries);
			}

			// Utility function to convert PunchCardEntries to SChartBubblePoints
			private IList<SChartBubbleDataPoint> CreateDataPointsFromPunchCardEntries(IEnumerable<PunchCardDataEntry> entries)
			{
				// We aren't interested in the entries which don't represent any commits
				return entries.Where (entry => entry.Commits != 0)
						   	.Select (entry => CreateBubbleDataPointForPunchCardEntry(entry))
							.ToList ();
			}

			// Utility Function to create punch card entry to a bubble data point
			private SChartBubbleDataPoint CreateBubbleDataPointForPunchCardEntry(PunchCardDataEntry entry)
			{
				SChartBubbleDataPoint dp = new SChartBubbleDataPoint ();
				dp.XValue = new NSNumber(entry.Hour);
				dp.YValue = new NSString(entry.DayName);
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
		private UIActivityIndicatorView _actIndicator;

		public PunchCardView(IntPtr p) : base(p)
		{
			// Create an activity indicator
			_actIndicator = new UIActivityIndicatorView ();
			_actIndicator.Center = new PointF (Bounds.Width / 2, Bounds.Height / 2);
			_actIndicator.StartAnimating ();
			this.Add (_actIndicator);
		}

		public void RenderData(PunchCardData data)
		{
			_dataSource = new PunchCardViewDataSource (data);

			// If we haven't got a chart, then create one
			if(_bubbleChart == null)
			{
				this.CreateChart ();
			}
			// Set it for the chart
			_bubbleChart.DataSource = _dataSource;
			// Redraw the chart
			_bubbleChart.RedrawChart ();
			// Get rid of the activity indicator
			_actIndicator.RemoveFromSuperview ();
			_actIndicator.StopAnimating ();
		}

		private void CreateChart()
		{
			_bubbleChart = new ShinobiChart (this.Bounds);
			_bubbleChart.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			SChartAxis xAxis = new SChartNumberAxis ();
			xAxis.RangePaddingHigh = new NSNumber (0.5);
			xAxis.RangePaddingLow = new NSNumber (0.5);
			xAxis.Title = "Hour";
			_bubbleChart.XAxis = xAxis;

			SChartCategoryAxis yAxis = new SChartCategoryAxis ();
			yAxis.RangePaddingHigh = new NSNumber (0.5);
			yAxis.RangePaddingLow = new NSNumber (0.5);
			yAxis.Title = "Day";
			_bubbleChart.YAxis = yAxis;

			// Add it as a subview
			this.AddSubview (_bubbleChart);
		}

	}
}

