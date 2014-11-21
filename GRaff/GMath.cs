﻿

using System;

namespace GRaff
{
	public static partial class GMath
	{
		public const double Tau = 6.283185307179586476925286766559;
		/// <summary>Conversion factor for degrees to radians. Is equal to τ / 360.</summary>
		public const double DegToRad = 0.01745329251994329576923690768489;
		/// <summary>Conversion factor for radians to degrees. Is equal to 360 / τ.</summary>
		public const double RadToDeg = 57.2957795130823208767981548141050;
		public static readonly Complex I = new Complex(0, 1);
		public const double GoldenRatio = 1.6180339887498948482045868343656;



		public static int BitBlockShift(int x)
		{
			return (x << 16) | (x >> 16);
		}
		public static uint BitBlockShift(uint x)
		{
			return (x << 16) | (x >> 16);
		}

		public static byte Median(byte x1, byte x2, byte x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static sbyte Median(sbyte x1, sbyte x2, sbyte x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static short Median(short x1, short x2, short x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static ushort Median(ushort x1, ushort x2, ushort x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static int Median(int x1, int x2, int x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static uint Median(uint x1, uint x2, uint x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static long Median(long x1, long x2, long x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static ulong Median(ulong x1, ulong x2, ulong x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static float Median(float x1, float x2, float x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static double Median(double x1, double x2, double x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}
		public static decimal Median(decimal x1, decimal x2, decimal x3)
		{
			if ((x1 <= x2 && x2 <= x3) || (x3 <= x2 && x2 <= x1))
				return x2;
			else if ((x2 <= x3 && x3 <= x1) || (x1 <= x3 && x3 <= x2))
				return x3;
			else
				return x1;
		}

		public static int RoundInt(double x)
		{
			return (int)Round(x);
		}
		public static int RoundInt(float x)
		{
			return (int)Round(x);
		}
		public static int RoundInt(decimal x)
		{
			return (int)Round(x);
		}
		public static uint RoundUInt(double x)
		{
			return (uint)Round(x);
		}
		public static uint RoundUInt(float x)
		{
			return (uint)Round(x);
		}
		public static uint RoundUInt(decimal x)
		{
			return (uint)Round(x);
		}
		public static long RoundLong(double x)
		{
			return (long)Round(x);
		}
		public static long RoundLong(decimal x)
		{
			return (long)Round(x);
		}
		public static ulong RoundULong(double x)
		{
			return (ulong)Round(x);
		}
		public static ulong RoundULong(decimal x)
		{
			return (ulong)Round(x);
		}


		public static int Sqr(byte x) { return x * x; }
		public static int Sqr(sbyte x) { return x * x; }
		public static int Sqr(short x) { return x * x; }
		public static int Sqr(ushort x) { return x * x; }
		public static int Sqr(int x) { return x * x; }
		public static uint Sqr(uint x) { return x * x; }
		public static long Sqr(long x) { return x * x; }
		public static ulong Sqr(ulong x) { return x * x; }
		public static float Sqr(float x) { return x * x; }
		public static double Sqr(double x) { return x * x; }
		public static decimal Sqr(decimal x) { return x * x; }
	}
}

