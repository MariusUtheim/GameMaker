﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	public class Room
	{
		public event EventHandler Enter;
		public event EventHandler Leave;

		protected internal Room(int width, int height)
		{
			this.Width = width;
			this.Height = height;
//			Window.Size = this.Size;
			this.Background = new Background();
		}

		public static Room Current { get; private set; }

		public static void Goto(Room room)
		{
			Contract.Requires(room != null);
			Current?._Leave();
			Current = room;
			room._Enter();
		}

		public static void Goto<TRoom>() where TRoom : Room
		{
			Goto(Activator.CreateInstance<TRoom>());
		}

		public int Width { get; private set; }
		public int Height { get; private set; }
		public IntVector Size => new IntVector(Width, Height);
		public Point Center => new Point(Width / 2.0, Height / 2.0);

		public Background Background { get; private set; }

		public virtual void OnKeyPressed(Key key) { }

		public virtual void OnEnter() { }

		public virtual void OnLeave() { }

		internal void _Leave()
		{
			OnLeave();
			Leave?.Invoke(this, new EventArgs());

			foreach (var instance in Instance.All)
				Instance.Remove(instance);
		}

		internal void _Enter()
		{
			Current = this;
			View.FocusRegion = new IntRectangle(0, 0, Width, Height);

			if (Background != null)
				Instance.Create(Background);
			OnEnter();
			Enter?.Invoke(this, new EventArgs());
		}

	}
}
