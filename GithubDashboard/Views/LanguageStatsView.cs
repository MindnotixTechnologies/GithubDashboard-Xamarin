using System;
using System.Linq;
using MonoTouch.Foundation;
using GithubAPI;
using ShinobiCharts;
using System.Collections.Generic;
using MonoTouch.UIKit;
using System.Drawing;

namespace GithubDashboard
{
	[Register("LanguageStatsView")]
	public class LanguageStatsView : UIView, IDataView<LanguageFrequencyData>
	{
		private class LanguageFrequencyDatasource : SChartDataSource
		{
			private List<SChartDataPoint>  _languageFrequency;

			public LanguageFrequencyDatasource(LanguageFrequencyData data)
			{
				_languageFrequency = data
					.OrderByDescending(kvp => kvp.Value)
					.Take (5)
					.Select(kvp => CreateDataPoint(kvp.Key, kvp.Value))
					.ToList();
			}

			#region Utility Methods

			private SChartDataPoint CreateDataPoint(string x, long y)
			{
				return new SChartRadialDataPoint () {
					Name = x,
					Value = new NSNumber(y)
				};
			}
			#endregion

			#region SChartDatasource Protocol Methods
			public override int GetNumberOfSeries (ShinobiChart chart)
			{
				return 1;
			}

			public override int GetNumberOfDataPoints (ShinobiChart chart, int dataSeriesIndex)
			{
				return _languageFrequency.Count;
			}

			public override SChartData GetDataPoint (ShinobiChart chart, int dataIndex, int dataSeriesIndex)
			{
				return _languageFrequency [dataIndex];
			}

			public override SChartSeries GetSeries (ShinobiChart chart, int dataSeriesIndex)
			{
				SChartPieSeries series = new SChartPieSeries ();
				series.Style.ShowLabels = false;
				return series;
			}
			#endregion
		}


		private ShinobiChart _chart;
		private LanguageFrequencyDatasource _dataSource;
		private UIActivityIndicatorView _actIndicator;

		public LanguageStatsView (IntPtr p) : base(p)
		{
			// Create an activity indicator
			_actIndicator = new UIActivityIndicatorView ();
			_actIndicator.Center = new PointF (Bounds.Width / 2, Bounds.Height / 2);
			_actIndicator.StartAnimating ();
			this.Add (_actIndicator);
		}

		public void RenderData(LanguageFrequencyData data)
		{
			_dataSource = new LanguageFrequencyDatasource(data);

			// If we haven't got a chart, then create one
			if(_chart == null)
			{
				this.CreateChart ();
			}
			// Assign it to this chart
			_chart.DataSource = _dataSource;
			// And then redraw the chart
			_chart.RedrawChart();
			_chart.Legend.Hidden = false;

			// Get rid of the activity indicator
			_actIndicator.RemoveFromSuperview ();
			_actIndicator.StopAnimating ();
		}

		private void CreateChart()
		{
			_chart = new ShinobiChart (this.Bounds);
			_chart.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			// Add it as a subview
			this.Add (_chart);
		}
	}
}
