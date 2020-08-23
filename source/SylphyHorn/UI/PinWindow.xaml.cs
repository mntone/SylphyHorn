using System;
using System.Windows;
using MetroRadiance.Interop;
using MetroRadiance.Interop.Win32;
using SylphyHorn.UI.Bindings;

namespace SylphyHorn.UI
{
	partial class PinWindow
	{
		protected override bool ShowInAllVirtualDesktops { get; }

		private IntPtr _target;

		public PinWindow()
		{
			this.InitializeComponent();
		}

		//public PinWindow(IntPtr target, WindowPlacement placement, bool pinned)
		//	: base(placement)
		//{
		//	this.ShowInAllVirtualDesktops = pinned;
		//	this._target = target;
		//	this.InitializeComponent();
		//}

		public void Show(IntPtr target)
		{
			this._target = target;
			this.Visibility = Visibility.Visible;
		}
		public void HideAndClear()
		{
			this.Visibility = Visibility.Collapsed;
			this.DataContext = null;
		}

		protected override bool TryGetTargetRect(out RECT rect, out RECT? capableRect)
		{
			capableRect = null;
			if (User32.IsZoomed(this._target))
			{
				return TryGetWorkAreaFromHwnd(this._target, out rect);
			}
			else
			{
				if (!TryGetWindowRectFromHwnd(this._target, out rect)) return false;
				if (!this.Placement.HasFlag(WindowPlacement.OutsideY)) return true;
				if (!TryGetWorkAreaFromHwnd(this._target, out var temp)) return false;

				capableRect = temp;
				return true;
			}
		}
	}
}
