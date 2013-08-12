using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using GithubAPI;

namespace GithubDashboard
{
	public class RepoSelectorViewControllerSource : UITableViewSource
	{
		private IList<RepoSummaryData> _repoData;

		public string NameForIndex(int index)
		{
			var curRepo = _repoData [index];
			return curRepo.name;
		}

		public RepoSelectorViewControllerSource (IEnumerable<RepoSummaryData> repoData)
		{
			_repoData = new List<RepoSummaryData>(repoData);
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			// TODO: return the actual number of items in the section
			return _repoData.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell ("RepoSelectorTableViewCell");
			if (cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Value1, "RepoSelectorTableViewCell");
			
			// TODO: populate the cell with the appropriate data based on the indexPath
			var curRepo = _repoData [indexPath.Row];
			cell.TextLabel.Text = curRepo.name;
			cell.DetailTextLabel.Text = curRepo.language;
			
			return cell;
		}
	}
}

