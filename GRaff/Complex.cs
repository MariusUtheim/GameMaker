﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
	/// <summary>
	/// Represents a complex number.
	/// </summary>
	public struct Complex
	{
		public Complex(double real, double imaginary) : this()
		{
			Real = real;
			Imaginary = imaginary;
		}

		/// <summary>
		/// Gets or sets the real part of this GRaff.Complex instance.
		/// </summary>
		public double Real { get; private set; }

		/// <summary>
		/// Gets or sets the imaginary part of this GRaff.Complex instance.
		/// </summary>
		public double Imaginary { get; private set; } 

		/// <summary>
		/// Converts this GRaff.Complex to a human-readable string, showing the value in Cartesian form x + yi.
		/// </summary>
		/// <returns>A string that represents this GRaff.Complex</returns>
		public override string ToString()
		{
			if (Imaginary == 0)
				return Real.ToString();
			else if (Real == 0)
				return Imaginary.ToString() + "i";
			else if (Imaginary > 0)
				return String.Format("{0} + {1}i", Real, Imaginary);
			else
				return String.Format("{0} - {1}i", Real, -Imaginary);
		}

		/// <summary>
		/// Specifies whether this GRaff.Complex is equal to the specified System.Object.
		/// </summary>
		/// <param name="obj">The System.Object to compare to.</param>
		/// <returns>true if obj is a GRaff.Complex and the two complex numbers are equal.</returns>
		public override bool Equals(object obj) { return (obj is Complex) ? (this == (Complex)obj) : base.Equals(obj); }

		/// <summary>
		/// Returns a hash code for this GRaff.Complex.
		/// </summary>
		/// <returns>An integer value that specified a hash value for this GRaff.Complex.</returns>
		public override int GetHashCode() {
			var i = Imaginary.GetHashCode();
			return Real.GetHashCode() ^ (i << 16 | i >> 16); }

		/// <summary>
		/// Compares two GRaff.Complex objects. The results specifies whether the two complex numbers are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Complex to compare.</param>
		/// <param name="right">The second GRaff.Complex to compare.</param>
		/// <returns>true if the two complex numbers are equal.</returns>
		public static bool operator ==(Complex left, Complex right) { return (left.Real == right.Real && left.Imaginary == right.Imaginary); }

		/// <summary>
		/// Compares two GRaff.Complex objects. The result specifies whether the two complex numbers are unequal. 
		/// </summary>
		/// <param name="left">The first GRaff.Complex to compare.</param>
		/// <param name="right">The second GRaff.Complex to compare.</param>
		/// <returns>true if the two complex numbers are unequal.</returns>
		public static bool operator !=(Complex left, Complex right) { return (left.Real != right.Real || left.Imaginary != right.Imaginary); }


		/// <summary>
		/// Performs complex addition of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex sum of the two numbers.</returns>
		public static Complex operator +(Complex left, Complex right) { return new Complex(left.Real + right.Real, left.Imaginary + right.Imaginary); }


		/// <summary>
		/// Performs complex subtraction of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex difference of the two numbers.</returns>
		public static Complex operator -(Complex left, Complex right) { return new Complex(left.Real - right.Real, left.Imaginary - right.Imaginary); }


		/// <summary>
		/// Performs complex multiplication of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex product of the two numbers.</returns>
		public static Complex operator *(Complex left, Complex right) { return new Complex(left.Real * right.Real - left.Imaginary * right.Imaginary, left.Real * right.Imaginary + left.Imaginary * right.Real); }
		

		/// <summary>
		/// Performs complex division of the two numbers.
		/// </summary>
		/// <param name="left">The first number.</param>
		/// <param name="right">The second number.</param>
		/// <returns>The complex ratio of the two numbers.</returns>
		public static Complex operator /(Complex left, Complex right)
		{
			if (right == 0)	throw new DivideByZeroException();
			double m = 1 / (right.Real * right.Real + right.Imaginary * right.Imaginary);
			return new Complex(m * (left.Real * right.Real + left.Imaginary * right.Imaginary), m * (left.Imaginary * right.Real - left.Real * right.Imaginary));
		}

		/// <summary>
		/// Converts the specified double to a purely real complex number.
		/// </summary>
		/// <param name="d">A real number.</param>
		/// <returns>The GRaff.Complex that results from the conversion.</returns>
		public static implicit operator Complex(double d) { return new Complex(d, 0); }
	}
}