using System;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using ShinobiCharts;
using System.Collections.Generic;
using GithubAPI;
using System.Drawing;
using GithubDashboard.Utilities;

namespace GithubDashboard
{
	[Register("CodeFrequencyView")]
	public class CodeFrequencyView : LoadingIndicatorView, IDataView<CodeFrequencyData>
	{
		private class CodeFrequencyDataSource : SChartDataSource
		{
			private IList<SChartDataPoint> _additionDPs;
			private IList<SChartDataPoint> _removalDPs;

			public CodeFrequencyDataSource(CodeFrequencyData entries)
			{
				if(entries != null) {
					this.CreateDataPointsFromCodeFrequencies(entries);
				} else {
					// Create some empty lists
					_additionDPs = new List<SChartDataPoint>();
					_removalDPs  = new List<SChartDataPoint>();
				}
			}

			private void CreateDataPointsFromCodeFrequencies(CodeFrequencyData entries)
			{
				_additionDPs = new List<SChartDataPoint> ();
				_removalDPs  = new List<SChartDataPoint> ();

				foreach (CodeFrequencyDataItem entry in entries) {
					SChartDataPoint addPt = new SChartDataPoint ();
					SChartDataPoint remPt = new SChartDataPoint ();
					addPt.XValue = (NSDate)entry.WeekCommencing;
					remPt.XValue = (NSDate)entry.WeekCommencing;

					addPt.YValue = (NSNumber)entry.Additions;
					remPt.YValue = (NSNumber)entry.Deletions;

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
				series.Baseline = 0;
				return series;
			}

			#endregion
		}

		private ShinobiChart _lineChart;
		private CodeFrequencyDataSource _dataSource;

		public CodeFrequencyView (IntPtr p) : base(p)
		{
		}

		public void RenderData(CodeFrequencyData data)
		{
			_dataSource = new CodeFrequencyDataSource(data);

			// If we haven't got a chart, then create one
			if(_lineChart == null)
			{
				this.createChart ();
			}
			// Set the chart's datasource
			_lineChart.DataSource = _dataSource;
			// Redraw the chart
			_lineChart.RedrawChart ();
			// Get rid of the activity indicator
			HideIndicator ();
		}

		private void createChart()
		{
			_lineChart = new ShinobiChart (this.Bounds);
			// Get the license key from our JSON reading utility
			_lineChart.LicenseKey = ShinobiLicenseKeyProviderJson.Instance.ChartsLicenseKey;
			_lineChart.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			SChartDateTimeAxis xAxis = new SChartDateTimeAxis ();
			xAxis.Title = "Week Commencing";
			_lineChart.XAxis = xAxis;

			SChartNumberAxis yAxis = new SChartNumberAxis ();
			yAxis.RangePaddingHigh = (NSNumber)0.5;
			yAxis.Title = "Changes";
			_lineChart.YAxis = yAxis;

			// Display the legend
			_lineChart.Legend.Hidden = false;
			_lineChart.Legend.Placement = SChartLegendPlacement.InsidePlotArea;
			_lineChart.Legend.Position = SChartLegendPosition.TopRight;

			// Add it as a subview
			this.AddSubview (_lineChart);
		}
	}
}

