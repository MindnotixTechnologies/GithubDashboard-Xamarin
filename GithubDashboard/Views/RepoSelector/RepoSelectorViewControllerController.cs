using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using GithubAPI;
using System.Collections.Generic;

namespace GithubDashboard
{
	public class RepoSelectorViewControllerController : UITableViewController
	{
		private String _owner;
		private RepoSelectorViewControllerSource _tableViewDataSource;
		private UIActivityIndicatorView _actIndicator;
		Action<String> _repoSelectionHandler;

		public RepoSelectorViewControllerController (String owner, Action<string> repoSelectionHandler) : base (UITableViewStyle.Plain)
		{
			_owner = owner;
			_actIndicator = new UIActivityIndicatorView ();
			_actIndicator.Center = new PointF (View.Bounds.Width / 2, View.Bounds.Height / 2);
			_actIndicator.StartAnimating ();
			View.Add (_actIndicator);

			// Save the selection handler
			_repoSelectionHandler = repoSelectionHandler;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Get the data
			GithubDataProvider.Instance.RepoList (_owner, data => {
				_tableViewDataSource = new RepoSelectorViewControllerSource (data, _repoSelectionHandler);
				InvokeOnMainThread (() => {
					TableView.Source = _tableViewDataSource;
					TableView.ReloadData ();
					_actIndicator.RemoveFromSuperview ();
					_actIndicator.StopAnimating ();
				});
			});
		}
	}
}

