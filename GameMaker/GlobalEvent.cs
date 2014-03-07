﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMaker
{
	public static class GlobalEvent
	{
		public static event Action DrawBackground;
		public static event Action DrawForeground;
		public static event Action<Key> Key;
		public static event Action<Key> KeyPressed;
		public static event Action<Key> KeyReleased;

		internal static void OnKey(Key key)
		{
			if (Key != null)
				Key.Invoke(key);
		}

		internal static void OnKeyPressed(Key key)
		{
			if (KeyPressed != null)
				KeyPressed.Invoke(key);
		}

		internal static void OnKeyReleased(Key key)
		{
			if (KeyReleased != null)
				KeyReleased.Invoke(key);
		}

		internal static void OnDrawBackground()
		{
			if (DrawBackground != null)
				DrawBackground();
		}

		internal static void OnDrawForeground()
		{
			if (DrawForeground != null)
				DrawForeground();
		}
	}
}
