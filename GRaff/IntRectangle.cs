﻿using System;


namespace GRaff
{
	/// <summary>
	/// Represents a rectangle with integer coordinates.
	/// </summary>
	public struct IntRectangle
	{/*C#6.0*/
		public IntRectangle(int x, int y, int width, int height)
			: this()
		{
			Left = x;
			Top = y;
			Width = width;
			Height = height;
		}

		public static readonly IntRectangle Zero;

		/// <summary>
		/// Initializes a new instance of the GRaff.IntRectangle class at the specified location and with the specified size.
		/// </summary>
		/// <param name="location">The location of the top-left corner.</param>
		/// <param name="size">The size of the rectangle.</param>
		public IntRectangle(IntVector location, IntVector size)
			: this(location.X, location.Y, size.X, size.Y) { }

		/// <summary>
		/// Gets the left coordinate of this GRaff.IntRectangle.
		/// </summary>
		public int Left { get; private set; }

		/// <summary>
		/// Gets the top coordinate of this GRaff.IntRectangle.
		/// </summary>
		public int Top { get; private set;  }

		/// <summary>
		/// Gets the width of this GRaff.IntRectangle.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// Gets the height of this GRaff.IntRectangle.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Gets the right coordinate of this GRaff.IntRectangle.
		/// </summary>
		public int Right { get { return Left + Width; } }


		/// <summary>
		/// Gets the bottom coordinate of this GRaff.IntRectangle.
		/// </summary>
		public int Bottom { get { return Top + Height; } }


		/// <summary>
		/// Gets the location of the top-left corner of this GRaff.IntRectangle.
		/// </summary>
		public IntVector TopLeft { get { return new IntVector(Left, Top); } }

		/// <summary>
		/// Gets the location of the top-right corner of this GRaff.IntRectangle.
		/// </summary>
		public IntVector TopRight { get { return new IntVector(Right, Top); } }

		/// <summary>
		/// Gets the location of the bottom-left corner of this GRaff.IntRectangle.
		/// </summary>
		public IntVector BottomLeft { get { return new IntVector(Left, Bottom); } }

		/// <summary>
		/// Gets the location of the bottom-right corner of this GRaff.IntRectangle.
		/// </summary>
		public IntVector BottomRight { get { return new IntVector(Right, Bottom); } }

		/// <summary>
		/// Gets the area of this GRaff.IntRectangle.
		/// </summary>
		public int Area { get { return Width * Height; } }

		/// <summary>
		/// Gets or sets the size of this GRaff.IntRectangle.
		/// </summary>
		public IntVector Size { get { return new IntVector(Width, Height); } }

		/// <summary>
		/// Tests whether two rectangles intersect.
		/// </summary>
		/// <param name="other">The GRaff.IntRectangle to test intersection with.</param>
		/// <returns>true if the two rectangles intersect.</returns>
		public bool Intersects(IntRectangle other)
		{
			return !(Left > other.Right || Top > other.Bottom || Right < other.Left || Bottom < other.Top);
		}

		public IntRectangle? Intersection(IntRectangle other)
		{
			var left = GMath.Max(Left, other.Left);
			var top = GMath.Max(Top, other.Top);
			var right = GMath.Min(Right, other.Right);
			var bottom = GMath.Min(Bottom, other.Bottom);

			if (left > right || top > bottom)
				return null;
			else
				return new IntRectangle(left, top, right - left, bottom - top);
		}


		/// <summary>
		/// Converts this GRaff.IntRectangle to a human-readable string.
		/// </summary>
		/// <returns>A string that represents this GRaff.IntRectangle</returns>
		public override string ToString() { return String.Format("IntRectangle: [({0},{1}), ({2},{3})]", Left, Top, Width, Height); }

		/// <summary>
		/// Specifies whether this GRaff.IntRectangle contains the same location and size as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.IntRectangle and has the same location and size as this GRaff.IntRectangle.</returns>
		public override bool Equals(object obj) { return (obj is IntRectangle) ? (this == (IntRectangle)obj) : base.Equals(obj); }

		/// <summary>
		/// Returns a hash code for this GRaff.IntRectangle.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.IntRectangle.</returns>
		public override int GetHashCode() { return Left ^ Top ^ Width ^ Height; }

		/// <summary>
		/// Compares two GRaff.IntRectangle objects. The result specifies whether their locations and sizes are equal.
		/// </summary>
		/// <param name="left">The first GRaff.IntRectangle to compare.</param>
		/// <param name="right">The second GRaff.IntRectangle to compare.</param>
		/// <returns>true if the locations and sizes of the two GRaff.IntRectangle structures are equal.</returns>
		public static bool operator ==(IntRectangle left, IntRectangle right) { return (left.Left == right.Left && left.Top == right.Top && left.Width == right.Width && left.Height == right.Height); }

		/// <summary>
		/// Compares two GRaff.IntRectangle objects. The result specifies whether their locations and sizes are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.IntRectangle to compare.</param>
		/// <param name="right">The second GRaff.IntRectangle to compare.</param>
		/// <returns>true if the locations and sizes of the two GRaff.IntRectangle structures are unequal.</returns>
		public static bool operator !=(IntRectangle left, IntRectangle right) { return (left.Left != right.Left || left.Top != right.Top || left.Width == right.Width || left.Height == right.Height); }


		/// <summary>
		/// Translates a GRaff.IntRectangle by a given GRaff.IntVector.
		/// </summary>
		/// <param name="r">The GRaff.IntRectangle to translate.</param>
		/// <param name="v">The GRaff.IntVector to translate by.</param>
		/// <returns>The translated GRaff.IntRectangle.</returns>
		public static IntRectangle operator +(IntRectangle r, IntVector v) { return new IntRectangle(r.TopLeft + v, r.Size); }
		

		/// <summary>
		/// Translates a GRaff.IntRectangle by subtracting a given GRaff.IntVector.
		/// </summary>
		/// <param name="r">The GRaff.IntRectangle to translate.</param>
		/// <param name="v">The negative GRaff.IntVector to translate by.</param>
		/// <returns>The translated GRaff.IntRectangle.</returns>
		public static IntRectangle operator -(IntRectangle r, IntVector v) { return new IntRectangle(r.TopLeft - v, r.Size); }


		/// <summary>
		/// Converts the specified GRaff.IntRectangle to a GRaff.Rectangle.
		/// </summary>
		/// <param name="r">The GRaff.IntRectangle to be converted</param>
		/// <returns>The GRaff.Rectangle that results from the conversion.</returns>
		public static implicit operator Rectangle(IntRectangle r) { return new Rectangle(r.Left, r.Top, r.Width, r.Height); }

		/// <summary>
		/// Converts the specified GRaff.Rectangle to a GRaff.IntRectangle, truncating the top-left coordinates and the dimensions.
		/// </summary>
		/// <param name="r">The GRaff.Rectangle to be converted</param>
		/// <returns>The GRaff.IntRectangle that results from the conversion.</returns>
		public static explicit operator IntRectangle(Rectangle r) { return new IntRectangle((int)r.Left, (int)r.Top, (int)r.Width, (int)r.Height); }

	}
}
