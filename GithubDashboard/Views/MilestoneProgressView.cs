using System;
using MonoTouch.UIKit;
using GithubAPI;
using ShinobiGauges;
using MonoTouch.Foundation;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace GithubDashboard
{
	[Register("MilestoneProgressView")]
	public class MilestoneProgressView : UIView, IDataView<MilestoneData>
	{
		private SGaugeRadial _gauge;
		private UILabel _milestoneTitle;
		private UILabel _milestoneDescription;

		public MilestoneProgressView (IntPtr p) : base(p)
		{
			this.BackgroundColor = UIColor.Clear;

			_milestoneTitle = new UILabel (new RectangleF(400,10,500,50));
			_milestoneTitle.Font = UIFont.SystemFontOfSize (30.0f);
			this.Add (_milestoneTitle);

			_milestoneDescription = new UILabel (new RectangleF(400,60,500,100));
			_milestoneDescription.Lines = 0;
			_milestoneDescription.Font = UIFont.SystemFontOfSize (22.0f);
			this.Add (_milestoneDescription);
		}

		#region IDataView implementation

		public void RenderData (MilestoneData data)
		{
			if (_gauge != null)
			{
				_gauge.RemoveFromSuperview ();
			}

			MilestoneDataItem milestone = data
				.OrderBy(m => m.created_at)
				.LastOrDefault ();
			if (milestone != null)
			{
				CreateGauge (milestone);
				_milestoneTitle.Text = milestone.title;
				_milestoneDescription.Text = milestone.description;
			}
		}

		#endregion

		void CreateGauge (MilestoneDataItem milestone)
		{
			NSNumber max = milestone.closed_issues + milestone.open_issues;

			// create a gauge
			_gauge = new SGaugeRadial (new RectangleF(0, 0, 350, 350), 0, max);
			this.Add (_gauge);

			// add some qual ranges
			double rangeSpacing = Math.Floor((double)max / 5.0);
			var ranges = new List<SGaugeQualitativeRange>() 
			{
				new SGaugeQualitativeRange(0, (double)max - rangeSpacing*3, UIColor.LightGray),
				new SGaugeQualitativeRange((double)max - rangeSpacing*3, (double)max - rangeSpacing*2, UIColor.Green),
				new SGaugeQualitativeRange((double)max - rangeSpacing*2, (double)max - rangeSpacing, UIColor.Orange),
				new SGaugeQualitativeRange((double)max - rangeSpacing, max, UIColor.Red)
			};
			_gauge.QualitativeRanges = ranges.ToArray ();

			// set ticks
			_gauge.Axis.MajorTickFrequency = 5.0f;
			_gauge.Axis.MinorTickFrequency = 1.0f;

			// qual range styling
			_gauge.Style.QualitativeInnerPosition = 0.3f;
			_gauge.Style.QualitativeOuterPosition = 0.7f;

			// background style
			_gauge.Style.BevelWidth = 0.0f;
			_gauge.Style.InnerBackgroundColor = UIColor.Clear;
			_gauge.Style.OuterBackgroundColor = UIColor.Clear;

			// needle style
			_gauge.Style.NeedleColor = UIColor.Black;

			_gauge.Style.GlassColor = UIColor.Clear;

			// set the current value
			_gauge.Value = milestone.closed_issues;

		}
	}
}

