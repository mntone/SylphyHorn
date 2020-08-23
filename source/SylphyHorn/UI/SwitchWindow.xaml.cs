using System;
using System.Windows;
using MetroRadiance.Interop;
using MetroRadiance.Interop.Win32;
using SylphyHorn.UI.Bindings;

namespace SylphyHorn.UI
{
	partial class SwitchWindow
	{
		private IntPtr _target;

		public SwitchWindow()
		{
			this.InitializeComponent();
		}

		//public SwitchWindow(IntPtr target, WindowPlacement placement)
		//	: base(placement)
		//{
		//	this._target = target;
		//	this.InitializeComponent();
		//}

		public void Show(IntPtr target)
		{
			this._target = target;
			this.Show();

			//var storyboard = (System.Windows.Media.Animation.Storyboard)this.FindResource("FadeIn");
			//storyboard.Begin(this);
		}

		public void HideAndClear(bool animating)
		{
			//if (animating)
			//{
			//	var storyboard = (System.Windows.Media.Animation.Storyboard)this.FindResource("FadeOut");
			//	storyboard.Completed += FadeOutCompletedCallback;
			//	storyboard.Begin(this);
			//}
			//else
			//{
				//this.NativeOpacity = 0;
				this.Hide();
			//}
			this.DataContext = null;
			this._target = IntPtr.Zero;
		}

		private void FadeOutCompletedCallback(object sender, EventArgs e)
		{
			this.Hide();
		}

		protected override bool TryGetTargetRect(out RECT rect, out RECT? capableRect)
		{
			capableRect = null;
			return TryGetWorkAreaFromHmonitor(this._target, out rect);
		}
	}
}
