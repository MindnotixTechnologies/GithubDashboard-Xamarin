using System;
using GithubAPI;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Drawing;
using ShinobiGrids;
using System.Linq;
using GithubDashboard.Utilities;
using MonoTouch.ObjCRuntime;

namespace GithubDashboard
{
	[Register("IssuesDataGridView")]
	public class IssuesDataGridView : LoadingIndicatorView, IDataView<IssuesData>
	{
		private class IssuesDataGridHelperDelegate : SDataGridDataSourceHelperDelegate
		{
			protected override bool PopulateCell (SDataGridDataSourceHelper helper, SDataGridCell cell, NSObject value, string propertyKey, NSObject source)
			{
				if (propertyKey == "reported")
				{
					AvatarCell avatarCell = (AvatarCell)cell;
					avatarCell.AvatarUrl = (NSString)value;
					return true;
				}

				return false;
			}
			protected override NSObject GroupValueForProperty (SDataGridDataSourceHelper helper, string propertyKey, NSObject source)
			{
				IssueDataItem issue = (IssueDataItem)source;
				NSObject value = GetProperty(issue, propertyKey);

				// for the date column, we specialise the formatting
				if (propertyKey == "created_at")
				{
					DateTime created = issue.created_at;
					value = (NSString)created.ToString ("MMM yyyy");
				}

				return value;
			}

			protected override NSObject DisplayValueForProperty (SDataGridDataSourceHelper helper, string propertyKey, NSObject source)
			{
				IssueDataItem issue = (IssueDataItem)source;
				NSObject value = GetProperty(issue, propertyKey);

				// for the date column, we specialise the formatting
				if (propertyKey == "created_at")
				{
					DateTime created = issue.created_at;
					value = (NSString)created.ToString ("dd-MM-yyyy");
				}

				return value;
			}

			protected override NSObject SortValueForProperty (SDataGridDataSourceHelper helper, string propertyKey, NSObject source)
			{
				return GetProperty((IssueDataItem)source, propertyKey);
			}

			/// <summary>
			/// Gets the property with the given name, casting to the required type
			/// </summary>
			private NSObject GetProperty(IssueDataItem issue, string property)
			{
				NSObject val;
				switch(property)
				{
				case "title":
					val = (NSString)issue.title;
					break;

				case "created_at":
					val = (NSDate)issue.created_at;
					break;

				case "reported":
					val = (NSString)issue.user.avatar_url;
					break;

				default:
					return null;
				}

				return val;
			}
			 
		}

		private class IssuesDataGridDataSource : SDataGridDataSource
		{
			private IssuesData _issuesData;

			public IssuesDataGridDataSource (IssuesData issuesData)
			{
				_issuesData = issuesData;
			}

			#region implemented abstract members of SDataGridDataSource
			protected override void PrepareCellForDisplay (ShinobiDataGrid grid, SDataGridCell cell)
			{
				// cast to the know type of cell
				SDataGridTextCell textCell = (SDataGridTextCell)cell;

				// find the issue
				IssueDataItem issue = _issuesData [cell.Coordinate.Row.RowIndex];

				// update the text
				textCell.TextField.Text = issue.title;
			}
			protected override uint GetNumberOfRowsInSection (ShinobiDataGrid grid, int sectionIndex)
			{
				return (uint)_issuesData.Count;
			}
			#endregion
		}

		private ShinobiDataGrid _dataGrid;
		private SDataGridDataSourceHelper _dataSourceHelper;

		public IssuesDataGridView (IntPtr p) : base(p)
		{
		}

		#region IDataView implementation

		public void RenderData (IssuesData data)
		{
			if (_dataGrid == null)
			{
				this.CreateDataGrid ();
			}

			this.HideIndicator ();

			_dataSourceHelper.Data = data.Cast<NSObject>().ToArray();
		}

		#endregion

		void CreateDataGrid ()
		{
			// create a datagrid
			_dataGrid = new ShinobiDataGrid (this.Bounds);
			_dataGrid.LicenseKey = ShinobiLicenseKeyProviderJson.Instance.GridsLicenseKey;
			_dataGrid.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

			// styling
			_dataGrid.DefaultCellStyleForAlternateRows = new SDataGridCellStyle(new UIColor(0.9f,0.9f,0.9f,1.0f), null, null);
			_dataGrid.DefaultGridLineStyle = new SDataGridLineStyle (0.0f, UIColor.Clear);

			// add the columns
			SDataGridColumn createdDateColumn = new SDataGridColumn ("created", "created_at");
			createdDateColumn.SortMode = SDataGridColumnSortMode.TriState;
			createdDateColumn.Width = new NSNumber (150);
			_dataGrid.AddColumn (createdDateColumn);

			SDataGridColumn titleColumn = new SDataGridColumn ("title", "title");
			titleColumn.SortMode = SDataGridColumnSortMode.TriState;
			titleColumn.Width = new NSNumber (795);
			_dataGrid.AddColumn (titleColumn);

			SDataGridColumn reportedColumn = new SDataGridColumn (" ", "reported", new Class("AvatarCell"), new Class("SDataGridHeaderCell"));
			reportedColumn.Width = new NSNumber (50);
			_dataGrid.AddColumn (reportedColumn);

			// provide a datasource helper. This acts as both the datasource and the delegate
			_dataSourceHelper = new SDataGridDataSourceHelper (_dataGrid);
			_dataSourceHelper.GroupedPropertyKey = "created_at";
			_dataSourceHelper.GroupedPropertySortOrder = SDataGridColumnSortOrder.Descending;
			_dataSourceHelper.Delegate = new IssuesDataGridHelperDelegate ();

			this.Add (_dataGrid);
		}
	}
}

