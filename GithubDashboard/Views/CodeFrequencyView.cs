using System;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using ShinobiCharts;
using System.Collections.Generic;
using GithubAPI;
using Utilities;

namespace GithubDashboard
{
	[Register("CodeFrequencyView")]
	public class CodeFrequencyView : UIView
	{
		private class CodeFrequencyDataSource : SChartDataSource
		{
			private IList<SChartDataPoint> _additionDPs;
			private IList<SChartDataPoint> _removalDPs;

			public CodeFrequencyDataSource(string owner, string repo)
			{
				var data = GithubDataProvider.CodeFrequencyEntries(owner, repo);
				this.CreateDataPointsFromCodeFrequencies(data);
			}

			private void CreateDataPointsFromCodeFrequencies(IEnumerable<CodeFrequencyEntry> entries)
			{
				_additionDPs = new List<SChartDataPoint> ();
				_removalDPs = new List<SChartDataPoint> ();

				foreach (CodeFrequencyEntry entry in entries) {
					SChartDataPoint addPt = new SChartDataPoint ();
					SChartDataPoint remPt = new SChartDataPoint ();
					addPt.XValue = entry.WeekCommencing.ToNSDate ();
					remPt.XValue = entry.WeekCommencing.ToNSDate ();

					addPt.YValue = new NSNumber (entry.Additions);
					remPt.YValue = new NSNumber (entry.Deletions);

					_additionDPs.Add (addPt);
					_removalDPs.Add (remPt);
				}
			}

			#region SChartDataSource methods
			public override SChartData GetDataPoint (ShinobiChart chart, int dataIndex, int dataSeriesIndex)
			{
				if (dataSeriesIndex == 0) {
					return _additionDPs [dataIndex];
				}
				else {
					return _removalDPs [dataIndex];
				}
			}

			public override int GetNumberOfSeries (ShinobiChart chart)
			{
				return 2;
			}

			public override int GetNumberOfDataPoints (ShinobiChart chart, int dataSeriesIndex)
			{
				return _additionDPs.Count ();
			}

			public override SChartSeries GetSeries (ShinobiChart chart, int dataSeriesIndex)
			{
				SChartLineSeries series = new SChartLineSeries ();
				if (dataSeriesIndex == 0) {
					series.Title = "Additions";
				} else {
					series.Title = "Deletions";
				}
				series.Style.ShowFill = true;
				series.Baseline = new NSNumber (0);
				return series;
			}

			#endregion
		}

		private ShinobiChart _columnChart;
		private CodeFrequencyDataSource _dataSource;
		private string _owner;
		private string _repo;

		public CodeFrequencyView (IntPtr p) : base(p)
		{
		}

		// Use this to specify the repo owner and name
		public void ChangeRepo(string owner, string repo)
		{
			_owner = owner;
			_repo = repo;
			// Make a new datasource
			this.createDatasource ();
			// If we haven't got a chart, then create one
			if(_columnChart == null)
			{
				this.createChart ();
			}
			// Adds set the new datasource
			_columnChart.DataSource = _dataSource;
		}

		private void createDatasource()
		{
			// Create the data source for a sample repository
			_dataSource = new CodeFrequencyDataSource(_owner, _repo);
		}

		private void createChart()
		{
			_columnChart = new ShinobiChart (this.Bounds);
			_columnChart.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			SChartDateTimeAxis xAxis = new SChartDateTimeAxis ();
			xAxis.Title = "Week Commencing";
			_columnChart.XAxis = xAxis;

			SChartNumberAxis yAxis = new SChartNumberAxis ();
			yAxis.RangePaddingHigh = new NSNumber (0.5);
			yAxis.Title = "Changes";
			_columnChart.YAxis = yAxis;

			// Display the legend
			_columnChart.Legend.Hidden = false;
			_columnChart.Legend.Placement = SChartLegendPlacement.InsidePlotArea;
			_columnChart.Legend.Position = SChartLegendPosition.TopRight;

			// Add it as a subview
			this.AddSubview (_columnChart);
		}
	}
}

