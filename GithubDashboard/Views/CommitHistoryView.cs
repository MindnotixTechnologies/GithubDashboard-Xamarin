using System;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using ShinobiGrids;
using GithubAPI;
using System.Collections.Generic;

namespace GithubDashboard
{
	[Register("CommitHistoryView")]
	public class CommitHistoryView : UIView
	{
		public class CommitHistoryDataSource : SDataGridDataSource
		{
			private List<CommitData> _commits;

			public CommitHistoryDataSource(List<CommitData> commits)
			{
				_commits = commits;
			}

			protected override uint GetNumberOfRowsInSection (ShinobiDataGrid grid, int sectionIndex)
			{
				return (uint)_commits.Count;
			}

			protected override void PrepareCellForDisplay (ShinobiDataGrid grid, SDataGridCell cell)
			{
				SDataGridTextCell textCell = (SDataGridTextCell)cell;
				SDataGridColumn column = cell.Coordinate.Column;

				CommitData commitData = _commits [cell.Coordinate.Row.RowIndex];

				if (column.Title == "Date")
				{
					textCell.TextField.Text = commitData.commit.author.date.ToString("MM-dd-yy HH:mm");
				}
				else if (column.Title == "Message")
				{
					textCell.TextField.Text = commitData.commit.message;
				}
				else if (column.Title == "Author")
				{
					textCell.TextField.Text = commitData.commit.author.name;
				}
			}

		}

		private ShinobiDataGrid _dataGrid;

		public CommitHistoryView (IntPtr p) : base(p)
		{
		}

		// Use this to specify the repo owner and name
		public void ChangeRepo(string owner, string repo)
		{
			// If we haven't got a grid, then create one
			if(_dataGrid == null)
			{
				createGrid ();
			}

			var commits = GithubDataProvider.CommitsForRepo (owner, repo);
			_dataGrid.DataSource = new CommitHistoryDataSource (commits.ToList ());
		}

		private void createGrid()
		{
			_dataGrid = new ShinobiDataGrid (this.Bounds);
			_dataGrid.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			AddSubview (_dataGrid);

			double width = 522;

			SDataGridColumn dateColumn = new SDataGridColumn ("Date");
			dateColumn.Width = width * 0.2;
			_dataGrid.AddColumn (dateColumn);

			SDataGridColumn messageColumn = new SDataGridColumn ("Message");
			messageColumn.Width = width * 0.6;
			_dataGrid.AddColumn (messageColumn);

			SDataGridColumn authorColumn = new SDataGridColumn ("Author");
			authorColumn.Width = width * 0.2;
			_dataGrid.AddColumn (authorColumn);
		}

	}
}

