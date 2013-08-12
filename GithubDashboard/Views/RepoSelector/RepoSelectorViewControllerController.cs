using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using GithubAPI;
using System.Collections.Generic;

namespace GithubDashboard
{
	public class RepoSelectorViewDelegate: UITableViewDelegate
	{
		private Action<NSIndexPath> _repoSelectionHandler;
		public RepoSelectorViewDelegate(Action<NSIndexPath> repoSelectionHandler)
		{
			_repoSelectionHandler = repoSelectionHandler;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			_repoSelectionHandler (indexPath);
		}
	}

	public class RepoSelectorViewControllerController : UITableViewController
	{
		private String _owner;
		private RepoSelectorViewControllerSource _tableViewDataSource;
		private RepoSelectorViewDelegate _tableViewDelegate;
		private UIActivityIndicatorView _actIndicator;

		public RepoSelectorViewControllerController (String owner, Action<string> repoSelectionHandler) : base (UITableViewStyle.Plain)
		{
			_owner = owner;
			_actIndicator = new UIActivityIndicatorView ();
			_actIndicator.Center = new PointF (View.Bounds.Width / 2, View.Bounds.Height / 2);
			_actIndicator.StartAnimating ();
			View.Add (_actIndicator);


			// Save off the repo selection handler
			_tableViewDelegate = new RepoSelectorViewDelegate (idxPath => {
				if (_tableViewDataSource != null) {
					var repoName = _tableViewDataSource.NameForIndex (idxPath.Row);
					repoSelectionHandler (repoName);
				}
			});
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Set the delegate
			TableView.Delegate = _tableViewDelegate;

			// Get the data
			GithubDataProvider.RepoList (_owner, data => {
				_tableViewDataSource = new RepoSelectorViewControllerSource (data);
				InvokeOnMainThread (delegate {
					TableView.Source = _tableViewDataSource;
					TableView.ReloadData ();
					_actIndicator.RemoveFromSuperview ();
					_actIndicator.StopAnimating ();
				});
			});
		}

	}
}

