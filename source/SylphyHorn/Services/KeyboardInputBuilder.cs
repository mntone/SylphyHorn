using SylphyHorn.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SylphyHorn.Services
{
	public sealed class KeyboardInputBuilder
	{
		private static HashSet<VirtualKeys> _extendedKey = new HashSet<VirtualKeys>(38)
		{
			VirtualKeys.VK_CONTROL,
			VirtualKeys.VK_RCONTROL,
			VirtualKeys.VK_MENU,
			VirtualKeys.VK_RMENU,
			VirtualKeys.VK_LWIN,
			VirtualKeys.VK_RWIN,

			VirtualKeys.VK_SNAPSHOT,
			VirtualKeys.VK_INSERT,
			VirtualKeys.VK_DELETE,

			VirtualKeys.VK_PRIOR,
			VirtualKeys.VK_NEXT,
			VirtualKeys.VK_END,
			VirtualKeys.VK_HOME,

			VirtualKeys.VK_LEFT,
			VirtualKeys.VK_UP,
			VirtualKeys.VK_RIGHT,
			VirtualKeys.VK_DOWN,

			VirtualKeys.VK_CANCEL,
			VirtualKeys.VK_DIVIDE,
			VirtualKeys.VK_NUMLOCK,

			VirtualKeys.VK_BROWSER_BACK,
			VirtualKeys.VK_BROWSER_FORWARD,
			VirtualKeys.VK_BROWSER_REFRESH,
			VirtualKeys.VK_BROWSER_STOP,
			VirtualKeys.VK_BROWSER_SEARCH,
			VirtualKeys.VK_BROWSER_FAVORITES,
			VirtualKeys.VK_BROWSER_HOME,

			VirtualKeys.VK_VOLUME_MUTE,
			VirtualKeys.VK_VOLUME_DOWN,
			VirtualKeys.VK_VOLUME_UP,

			VirtualKeys.VK_MEDIA_NEXT_TRACK,
			VirtualKeys.VK_MEDIA_PREV_TRACK,
			VirtualKeys.VK_MEDIA_STOP,
			VirtualKeys.VK_MEDIA_PLAY_PAUSE,

			VirtualKeys.VK_LAUNCH_MAIL,
			VirtualKeys.VK_LAUNCH_MEDIA_SELECT,
			VirtualKeys.VK_LAUNCH_APP1,
			VirtualKeys.VK_LAUNCH_APP2,
		};
		private static bool IsExtendedKey(VirtualKeys key)
			=> _extendedKey.Contains(key);

		public Collection<KEYBDINPUT> Inputs { get; } = new Collection<KEYBDINPUT>();

		private KeyboardInputBuilder AddKey(VirtualKeys key, bool keyUp)
		{
			var scanCode = (ushort)(NativeMethods.MapVirtualKey((uint)key, MapVirtualKey.MAPVK_VK_TO_VSC) & 0xFF);
			var extendedKey = IsExtendedKey(key);
			this.Inputs.Add(new KEYBDINPUT
			{
				wVk = key,
				wScan = scanCode,
				dwFlags = (keyUp ? KeyEventFlags.KEYEVENTF_KEYUP : 0) | (extendedKey ? KeyEventFlags.KEYEVENTF_EXTENDEDKEY : 0),
				time = 0,
				dwExtraInfo = UIntPtr.Zero,
			});
			return this;
		}

		public KeyboardInputBuilder AddKeyDown(VirtualKeys key)
			=> this.AddKey(key, false);

		public KeyboardInputBuilder AddKeyUp(VirtualKeys key)
			=> this.AddKey(key, true);

		public INPUT[] ToArray()
		{
			var inputsData = this.Inputs.Select(inputs => new INPUT()
			{
				type = InputType.INPUT_KEYBOARD,
				ui = new UNIONINPUT()
				{
					ki = inputs,
				}
			}).ToArray();
			return inputsData;
		}
	}

	public static class KeyboardInputBuilderExtensions
	{
		public static KeyboardInputBuilder AddKeysDown(this KeyboardInputBuilder builder, IEnumerable<VirtualKeys> keys)
		{
			foreach (var key in keys)
			{
				builder.AddKeyDown(key);
			}
			return builder;
		}

		public static KeyboardInputBuilder AddKeysUp(this KeyboardInputBuilder builder, IEnumerable<VirtualKeys> keys)
		{
			foreach (var key in keys)
			{
				builder.AddKeyUp(key);
			}
			return builder;
		}

		public static KeyboardInputBuilder AddReversedKeysUp(this KeyboardInputBuilder builder, IEnumerable<VirtualKeys> keys)
			=> builder.AddKeysUp(keys.Reverse());

		public static KeyboardInputBuilder AddKeyPress(this KeyboardInputBuilder builder, VirtualKeys key)
			=> builder.AddKeyDown(key).AddKeyUp(key);

		public static KeyboardInputBuilder AddKeysPress(this KeyboardInputBuilder builder, IEnumerable<VirtualKeys> keys)
			=> builder.AddKeysDown(keys).AddReversedKeysUp(keys);
	}
}
