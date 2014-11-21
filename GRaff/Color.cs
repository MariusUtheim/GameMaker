﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GRaff
{
	/// <summary>
	/// Represents an ARGB color. This struct is immutable. Colors can be cast from uint structures.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public partial struct Color
	{
		public Color(byte a, byte r, byte g, byte b)
			:this()
		{
			A = a;
			R = r;
			G = g;
			B = b;
		}

		/// <summary>
		/// Gets the value of the red channel of this GRaff.Color.
		/// </summary>
		public byte R { get; private set; }

		/// <summary>
		/// Gets the value of the green channel of this GRaff.Color.
		/// </summary>
		public byte G { get; private set; }

		/// <summary>
		/// Gets the value of the blue channel of this GRaff.Color.
		/// </summary>
		public byte B { get; private set; }

		/// <summary>
		/// Gets the value of the alpha channel of this GRaff.Color.
		/// </summary>
		public byte A { get; private set; }



		/// <summary>
		/// Initializes a new instance of the GRaff.Color class, using the specified ARGB values.
		/// </summary>
		/// <param name="a">The alpha channel.</param>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		public Color(int a, int r, int g, int b) : this((byte)a, (byte)r, (byte)g, (byte)b) { }

		/// <summary>
		/// Initializes a new instance of the GRaff.Color class, using the specified RGB values and an alpha value of 255.
		/// </summary>
		/// <param name="r">The red channel.</param>
		/// <param name="g">The green channel.</param>
		/// <param name="b">The blue channel.</param>
		public Color(byte r, byte g, byte b) : this((byte)255, r, g, b) { }

		/// <summary>
		/// Initializes a new instance of the GRaff.Color class, using the specified ARGB value in a 32-bit format.
		/// Colors can also be implicitly converted from ints.
		/// </summary>
		/// <param name="argb">The ARGB value of the created color.</param>
		public Color(uint argb) : this((byte)(argb >> 24), (byte)(argb >> 16), (byte)(argb >> 8), (byte)argb) { }


		/// <summary>
		/// Gets the value of this color as a 32-bit integer in ARGB format.
		/// </summary>
		public uint Argb
		{
			get
			{
				return (uint)((A << 24) | (R << 16) | (G << 8) | B);
			}
		}

		/// <summary>
		/// Averages the specified GRaff.Color structures, calculating the average of each channel separately.
		/// </summary>
		/// <param name="colors">An array of GRaff.Color structures that will be merged.</param>
		/// <returns>The average of the specified GRaff.Color structures.</returns>
		public static Color Merge(Color[] colors)
		{
			if (colors == null) throw new ArgumentNullException("colors", "Cannot be null");
			if (colors.Length == 0) throw new ArgumentException("Must have at least one element", "colors");
			int a = 0, r = 0, g = 0, b = 0;

			for (int i = 0; i < colors.Length; i++)
			{
				a += colors[i].A;
				r += colors[i].R;
				g += colors[i].G;
				b += colors[i].B;
			}

			return new Color(a / colors.Length, r / colors.Length, g / colors.Length, b / colors.Length);
		}

		/// <summary>
		/// Finds the weighted average of the two GRaff.Color structures, calculating the average of each channel separately.
		/// </summary>
		/// <param name="c1">The first GRaff.Color.</param>
		/// <param name="c2">The second GRaff.Color.</param>
		/// <param name="a">A parameter specifying the weights of the two colors. If this is zero, c1 is returned, and if this is one, c2 is returned.</param>
		/// <returns>The weighted average of the two colors: c1 * (1 - a) + c2 * a</returns>
		public static Color Merge(Color c1, Color c2, double a)
		{
			double b = 1 - a;
			return new Color((int)(c1.A * b + c2.A * a), (int)(c1.R * b + c2.R * a), (int)(c1.G * b + c2.G * a), (int)(c1.B * b + c2.B * a));
		}

		/// <summary>
		/// Creates a new GRaff.Color, with the same color channels as this instance, but with the new specified alpha channel.
		/// </summary>
		/// <param name="alphaChannel">The alpha channel of the new color.</param>
		/// <returns>A new GRaff.Color with the same color as this instance, but with the specified alpha channel.</returns>
		public Color Transparent(int alphaChannel)
		{
			return new Color((byte)alphaChannel, R, G, B);
		}

		/// <summary>
		/// Creates a new GRaff.Color, with the same color channels as this instance, but iwth the new specified opacity.
		/// </summary>
		/// <param name="opacity">The opacity of the new color. 0.0 means it is completely transparent, and 1.0 means it is completely opaque.</param>
		/// <returns>A new GRaff.Color with the same color as this instance, but with an alpha channel corresponding to the specified opacity.</returns>
		public Color Transparent(double opacity)
		{
			return new Color((byte)GMath.Round(255.0 * GMath.Median(0.0, opacity, 1.0)), R, G, B);
		}

		/// <summary>
		/// Converts this GRaff.Color to an OpenTK.Graphics.Color4 object.
		/// </summary>
		/// <returns>The OpenTK.Graphics.Color4 that results from the conversion.</returns>
		internal OpenTK.Graphics.Color4 ToOpenGLColor()
		{
			return new OpenTK.Graphics.Color4(R, G, B, A);
		}

		/// <summary>
		/// Converts this GRaff.Color to a human-readable string, showing the value of each channel.
		/// </summary>
		/// <returns>A string that represents this GRaff.Color</returns>
		public override string ToString() { return String.Format("{0}=0x{1:X}", "Color", Argb); }


		/// <summary>
		/// Specifies whether this GRaff.Color contains the same ARGB value as the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Color and has the same ARGB value as this GRaff.Color.</returns>
		public override bool Equals(object obj) { return (obj is Color) ? (this == (Color)obj) : base.Equals(obj); }

		/// <summary>
		/// Returns a hash code of this GRaff.Color. The hash code is equal to the ARGB value.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Color.</returns>
		public override int GetHashCode() { return (int)Argb; }

		/// <summary>
		/// Compares two GRaff.Color objects. The results specifies whether their ARGB values are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Color to compare.</param>
		/// <param name="right">The second GRaff.Color to compare.</param>
		/// <returns>true if the ARGB values of the two GRaff.Color structures are equal.</returns>
		public static bool operator ==(Color left, Color right) { return (left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B); }
	
		/// <summary>
		/// Compares two GRaff.Color objects. The results specifies whether their ARGB values are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Color to compare.</param>
		/// <param name="right">The second GRaff.Color to compare.</param>
		/// <returns>true if the ARGB values of the two colors are unequal.</returns>
		public static bool operator !=(Color left, Color right) { return (left.A != right.A || left.R != right.R || left.G != right.G || left.B != right.B); }


		/// <summary>
		/// Converts the specified integer in an ARGB format to a GRaff.Color
		/// </summary>
		/// <param name="argb">The System.Uint32 to be converted.</param>
		/// <returns>The GRaff.Color resulting from the conversion.</returns>
		public static implicit operator Color(uint argb) { return new Color(argb); }
	}
}