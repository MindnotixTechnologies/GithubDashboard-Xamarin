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
		private IList<RepoSummaryDataItem> _repoData;
		private Action<string> _repoSelectionHandler;

		public string NameForIndex(int index)
		{
			var curRepo = _repoData [index];
			return curRepo.name;
		}

		public RepoSelectorViewControllerSource (IEnumerable<RepoSummaryDataItem> repoData, Action<string> repoSelectionHandler)
		{
			_repoData = new List<RepoSummaryDataItem>(repoData);
			_repoSelectionHandler = repoSelectionHandler;
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

			var curRepo = _repoData [indexPath.Row];
			cell.TextLabel.Text = curRepo.name;
			cell.DetailTextLabel.Text = curRepo.language;
			
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			var curRepo = _repoData [indexPath.Row];
			_repoSelectionHandler (curRepo.name);
		}
	}
}

