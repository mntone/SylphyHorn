using Livet;

namespace SylphyHorn.UI.Bindings
{
	public class NotificationWindowViewModel : NotificationObject
	{
		#region Title 変更通知プロパティ

		private string _Title;

		public string Title
		{
			get { return this._Title; }
			set
			{
				if (this._Title != value)
				{
					this._Title = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Header 変更通知プロパティ

		private string _Header;

		public string Header
		{
			get { return this._Header; }
			set
			{
				if (this._Header != value)
				{
					this._Header = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Body 変更通知プロパティ

		private string _Body;

		public string Body
		{
			get { return this._Body; }
			set
			{
				if (this._Body != value)
				{
					this._Body = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
	}
}
