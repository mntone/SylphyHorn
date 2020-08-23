using SylphyHorn.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SylphyHorn.Services
{
	public sealed class WindowPoolService : IDisposable
	{
		public static WindowPoolService Instance { get; } = new WindowPoolService();

		private readonly WindowPool<SwitchWindow> _switchWindowPool = new WindowPool<SwitchWindow>();
		private readonly WindowPool<PinWindow> _pinWindowPool = new WindowPool<PinWindow>();

		public SwitchWindow GetSwitchWindow()
			=> this._switchWindowPool.GetOrCreateWindow();

		public PinWindow GetPinWindow()
			=> this._pinWindowPool.GetOrCreateWindow();

		public void Dispose()
		{
			this._switchWindowPool.Dispose();
			this._pinWindowPool.Dispose();
		}

		private sealed class WindowPool<T> : IDisposable where T : NotificationWindow, new()
		{
			private readonly Stack<T> _windows = new Stack<T>();

			public T GetOrCreateWindow()
			{
				try
				{
					var window = this._windows.Pop();
					return window;
				}
				catch (InvalidOperationException)
				{
					var newWindow = new T();
					newWindow.IsVisibleChanged += WindowIsVisibleChangedCallback;
					return newWindow;
				}
			}

			public void Dispose()
			{
				var cont = true;
				do
				{
					try
					{
						var window = this._windows.Pop();
						window.Close();
					}
					catch (InvalidOperationException)
					{
						cont = false;
					}
				} while (cont);
			}

			private void WindowIsVisibleChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
			{
				var newIsVisible = (bool)e.NewValue;
				if (!newIsVisible)
				{
					var window = (T)sender;
					this._windows.Push(window);
				}
			}
		}
	}
}
