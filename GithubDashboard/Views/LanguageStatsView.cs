using System;
using System.Linq;
using MonoTouch.Foundation;
using GithubAPI;
using ShinobiCharts;
using System.Collections.Generic;
using MonoTouch.UIKit;
using System.Drawing;
using GithubDashboard.Utilities;

namespace GithubDashboard
{
	[Register("LanguageStatsView")]
	public class LanguageStatsView : LoadingIndicatorView, IDataView<LanguageFrequencyData>
	{
		private class LanguageFrequencyDatasource : SChartDataSource
		{
			private List<SChartDataPoint>  _languageFrequency;

			public LanguageFrequencyDatasource(LanguageFrequencyData data)
			{
				if(data != null) {
					// compute the total of all language 'bins'
					double total = data.Sum(kvp => kvp.Value);

					// take the 5 largest languages, and convert to percentage share
					_languageFrequency = data
						.OrderByDescending(kvp => kvp.Value)
						.Take (5)
						.Select(kvp => CreateDataPoint(kvp.Key, (double)kvp.Value * 100.0 / total))
						.ToList();
				} else {
					_languageFrequency = new List<SChartDataPoint>();
				}
			}

			#region Utility Methods

			private SChartDataPoint CreateDataPoint(string x, double y)
			{
				return new SChartRadialDataPoint () {
					Name = x,
					Value = (NSNumber)y
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
				series.Style.ShowLabels = true;
				series.LabelFormatString = "%.0f%%";
				return series;
			}
			#endregion
		}

		private class LanguageStatsDelegate : SChartDelegate 
		{
			protected override void OnAddingRadialLabel (ShinobiChart chart, UILabel label, SChartRadialDataPoint datapoint, int index, SChartRadialSeries series)
			{
				// hide labels for slice with < 5% language share
				if (datapoint.Value.DoubleValue < 5)
				{
					label.Hidden = true;
				}
			}
		}



		private ShinobiChart _chart;
		private LanguageFrequencyDatasource _dataSource;
		private LanguageStatsDelegate _delegate;

		public LanguageStatsView (IntPtr p) : base(p)
		{
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
			HideIndicator ();
		}

		private void CreateChart()
		{
			_chart = new ShinobiChart (this.Bounds);
			_chart.LicenseKey = ShinobiLicenseKeyProviderJson.Instance.ChartsLicenseKey;
			_chart.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			// disable interaction - in order to allow paging of the container view
			_chart.UserInteractionEnabled = false;

			// add the delegate
			_delegate = new LanguageStatsDelegate ();
			_chart.Delegate = _delegate;

			// Add it as a subview
			this.Add (_chart);
		}
	}
}

