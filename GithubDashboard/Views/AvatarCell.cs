using System;
using ShinobiGrids;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using System.Drawing;

namespace GithubDashboard
{
	/// <summary>
	/// A custom avatar cell, renders the avatar as a small thumbnail
	/// </summary>
	[Register("AvatarCell")]
	public class AvatarCell : SDataGridCell
	{
		private UIImageView _imageView;
		private string _avatarUrl;

		public string AvatarUrl
		{
			get
			{
				return _avatarUrl;
			}
			set
			{
				_avatarUrl = value;

				DispatchQueue.DefaultGlobalQueue.DispatchAsync(() => {
					NSUrl url = new NSUrl (value);
					NSData imageData = NSData.FromUrl (url);
					UIImage image = UIImage.LoadFromData (imageData);

					DispatchQueue.MainQueue.DispatchAsync(() => {
						_imageView.Image = image;
					});
				});
			}
		}

		public AvatarCell (IntPtr p) : base(p)
		{
			_imageView = new UIImageView (new RectangleF(3,3,35,35));
			_imageView.Layer.BorderColor = UIColor.LightGray.CGColor;
			_imageView.Layer.BorderWidth = 2.0f;
			this.Add (_imageView);
			this.FitSubviewToView = false;
		}

	}
}

