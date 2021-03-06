﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GRaff
{
	/// <summary>
	/// Represents the matrix of an affine transformation.
	/// </summary>
	public sealed class Matrix : ICloneable, IEquatable<Matrix>
	{

		/// <summary>
		/// Initializes a new instance of the GRaff.Matrix class as an identity matrix.
		/// </summary>
		public Matrix()
			: this(1, 0, 0, 0, 1, 0)
		{ }

        /// <summary>
        /// Initializes a new instance of the GRaff.Matrix class with the specified matrix elements and with the affine components being zero.
        /// </summary>
        /// <param name="m00">The first element of the first row.</param>
        /// <param name="m01">The second element of the first row.</param>
        /// <param name="m10">The first element of the second row.</param>
        /// <param name="m11">The second element of the second row.</param>
        public Matrix(double m00, double m01, double m10, double m11)
        {
            this.M00 = m00;
            this.M01 = m01;
            this.M02 = 0;
            this.M10 = m10;
            this.M11 = m11;
            this.M12 = 0;
        }

		/// <summary>
		/// Initializes a new instance of the GRaff.Matrix class with the specified matrix elements.
		/// </summary>
		/// <param name="m00">The first element of the first row.</param>
		/// <param name="m01">The second element of the first row.</param>
		/// <param name="m02">The third element of the first row.</param>
		/// <param name="m10">The first element of the second row.</param>
		/// <param name="m11">The second element of the second row.</param>
		/// <param name="m12">The third element of the second row.</param>
		public Matrix(double m00, double m01, double m02, double m10, double m11, double m12)
		{
			this.M00 = m00;
			this.M01 = m01;
			this.M02 = m02;
			this.M10 = m10;
			this.M11 = m11;
			this.M12 = m12;
		}

		public static Matrix Identity { get; } = new Matrix();

        public static Matrix Zero { get; } = new Matrix(0, 0, 0, 0, 0, 0);

		/// <summary>
		/// Creates a GRaff.Matrix representing a scaling transformation.
		/// </summary>
		/// <param name="sX">The horizontal scale factor.</param>
		/// <param name="sY">The vertical scale factor</param>
		/// <returns>A new GRaff.Matrix representing the transformation.</returns>
		public static Matrix Scaling(double sX, double sY)
		    => new Matrix(sX, 0, 0, 0, sY, 0);

		/// <summary>
		/// Creates a GRaff.Matrix representing a shear transformation.
		/// </summary>
		/// <param name="hX">The horizontal shear factor.</param>
		/// <param name="hY">The vertical shear factor.</param>
		/// <returns>A new GRaff.Matrix representing the transformation.</returns>
		public static Matrix Shearing(double hX, double hY)
	        => new Matrix(1, hX, 0, hY, 1, 0);

		/// <summary>
		/// Creates a GRaff.Matrix representing a rotation transform around the origin.
		/// </summary>
		/// <param name="a">The angle to rotate by.</param>
		/// <returns>A new GRaff.Matrix representing the transformation.</returns>
		public static Matrix Rotation(Angle a)
		    => new Matrix(GMath.Cos(a), -GMath.Sin(a), 0, GMath.Sin(a), GMath.Cos(a), 0);

		/// <summary>
		/// Creates a GRaff.Matrix representing a translation trnasformation.
		/// </summary>
		/// <param name="tX">The amount to translate by in the horizontal direction.</param>
		/// <param name="tY">The amount to translate by in the vertical direction.</param>
		/// <returns>A new GRaff.Matrix representing the transformation.</returns>
		public static Matrix Translation(double tX, double tY)
		    => new Matrix(1, 0, tX, 0, 1, tY);

        public static Matrix Translation(Point p) => Translation((Vector)p);

        public static Matrix Translation(Vector v)
            => new Matrix(1, 0, v.X, 0, 1, v.Y);

		public static Matrix Mapping(Triangle src, Triangle dst)
		{
			double c12 = src.X1 * src.Y2 - src.X2 * src.Y1;
			double c23 = src.X2 * src.Y3 - src.X3 * src.Y2;
			double c31 = src.X3 * src.Y1 - src.X1 * src.Y3;
			double determinant = c12 + c23 + c31;

			if (determinant == 0)
				throw new MatrixException("The components of the source triangle are not linearly independent.");

			return new Matrix(
				(src.Y2 - src.Y3) * dst.X1 + (src.Y3 - src.Y1) * dst.X2 + (src.Y1 - src.Y2) * dst.X3,
				(src.X3 - src.X2) * dst.X1 + (src.X1 - src.X3) * dst.X2 + (src.X2 - src.X1) * dst.X3,
				c23 * dst.X1 + c31 * dst.X2 + c12 * dst.X3,
				(src.Y2 - src.Y3) * dst.Y1 + (src.Y3 - src.Y1) * dst.Y2 + (src.Y1 - src.Y2) * dst.Y3,
				(src.X3 - src.X2) * dst.Y1 + (src.X1 - src.X3) * dst.Y2 + (src.X2 - src.X1) * dst.Y3,
				c23 * dst.Y1 + c31 * dst.Y2 + c12 * dst.Y3
			) / determinant;
		}

		/// <summary>
		/// Gets the first element of the first row of this GRaff.Matrix.
		/// </summary>
		public double M00 { get; private set; }

		/// <summary>
		/// Gets the second element of the first row of this GRaff.Matrix.
		/// </summary>
		public double M01 { get; private set; }

		/// <summary>
		/// Gets the third element of the first row of this GRaff.Matrix.
		/// </summary>
		public double M02 { get; private set; }

		/// <summary>
		/// Gets the first element of the second row of this GRaff.Matrix.
		/// </summary>
		public double M10 { get; private set; }

		/// <summary>
		/// Gets the second element of the second row of this GRaff.Matrix.
		/// </summary>
		public double M11 { get; private set; }

		/// <summary>
		/// Gets the third element of the second row of this GRaff.Matrix.
		/// </summary>
		public double M12 { get; private set; }

		/// <summary>
		/// Gets the determinant of this GRaff.Matrix.
		/// </summary>
		public double Determinant => M00 * M11 - M01 * M10;

		/// <summary>
		/// Gets the inverse of this GRaff.Matrix.
		/// </summary>
		/// <exception cref="GRaff.MatrixException">If the determinant is zero.</exception>
		public Matrix Inverse
		{
			get
			{
				double det = Determinant;
				if (det == 0)
					throw new MatrixException();
				return new Matrix(M11 / det, -M01 / det, (-M02 * M11 + M01 * M12) / det, -M10 / det, M00 / det, (M02 * M10 - M00 * M12) / det);
			}
		}


		/// <summary>
		/// Applies a scaling transformation to this GRaff.Matrix and returns the result.
        /// This is equivalent to Matrix.Scaling(sX, sY) * this.
		/// </summary>
		/// <param name="sX">The horizontal scale factor.</param>
		/// <param name="sY">The vertical scale factor.</param>
		/// <returns>This GRaff.Matrix, after the transformation.</returns>
		public Matrix Scale(double sX, double sY)
			=> new Matrix(M00 * sX, M01 * sX, M02 * sX, M10 * sY, M11 * sY, M12 * sY);


		/// <summary>
		/// Applies a rotation transformation to this GRaff.Matrix.
        /// This is equivalent to Matrix.Rotation(a) * this.
		/// </summary>
		/// <param name="a">The angle to rotate by.</param>
		/// <returns>This GRaff.Matrix, after the transformation.</returns>
		public Matrix Rotate(Angle a)
		{
			double c = GMath.Cos(a), s = GMath.Sin(a);
			return new Matrix(M00 * c - M10 * s, M01 * c - M11 * s, M02 * c - M12 * s,
                              M00 * s + M10 * c, M01 * s + M11 * c, M02 * s + M12 * c);
		}

		/// <summary>
		/// Applies a translation transformation to this GRaff.Matrix.
        /// This is equivalent to Matrix.Translation(tX, tY) * this.
		/// </summary>
		/// <param name="tX">The horizontal translation.</param>
		/// <param name="tY">The vertical translation.</param>
		/// <returns>This GRaff.Matrix, after the transformation.</returns>
		public Matrix Translate(double tX, double tY)
			=> new Matrix(M00, M01, M02 + tX, M10, M11, M12 + tY);

		/// <summary>
		/// Applies a shear transformation to this GRaff.Matrix.
        /// This is equivalent to Matrix.Shearing(hX, hY) * this.
		/// </summary>
		/// <param name="hX">The horizontal shear factor.</param>
		/// <param name="hY">The vertical shear factor.</param>
		/// <returns>This GRaff.Matrix, after the transformation.</returns>
		public Matrix Shear(double hX, double hY)
			=> new Matrix(M00 + hX * M10, M01 + hX * M11, M02 + hX * M12, M10 + hY * M00, M11 + hY * M01, M12 + hY * M02);

		public bool Equals(Matrix? other)
			=> other is object && (this - other)._magnitude <= GMath.MachineEpsilon;

        /// <summary>
        /// Specifies whether this GRaff.Matrix contains the same elements as the specified System.Object.
        /// </summary>
        /// <param name="obj">The System.Object to compare to.</param>
        /// <returns>true if obj is a GRaff.Matrix and has the same elements as this GRaff.Matrix.</returns>
        public override bool Equals(object? obj)
            => (obj is Matrix m) ? Equals(m) : false;

		/// <summary>
		/// Returns a hash code for this GRaff.Matrix.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this GRaff.Matrix.</returns>
		public override int GetHashCode()
			=> GMath.HashCombine(M00.GetHashCode(), M01.GetHashCode(), M02.GetHashCode(), M10.GetHashCode(), M11.GetHashCode(), M12.GetHashCode());

		/// <summary>
		/// Converts this GRaff.Matrix to a human-readable string, displaying the values of the elements.
		/// </summary>
		/// <returns>A string that represents this GRaff.Matrix.</returns>
		public override string ToString() 
            => $"[[{M00}, {M01}, {M02}], [{M10}, {M11}, {M12}]]";

		/// <summary>
		/// Creates a deep clone of this GRaff.Matrix.
		/// </summary>
		/// <returns>A deep clone of this GRaff.Matrix.</returns>
		public Matrix Clone() => new Matrix(M00, M01, M02, M10, M11, M12);

		/// <summary>
		/// Creates a deep clone of this GRaff.Matrix.
		/// </summary>
		/// <returns>A deep clone of this GRaff.Matrix.</returns>
		object ICloneable.Clone() => Clone();

		private double _magnitude => M00 * M00 + M01 * M01 + M02 * M02 + M10 * M10 + M11 * M11 + M12 * M12;

		/// <summary>
		/// Compares two GRaff.Matrix objects. The result specifies whether all their elements are equal.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix to compare.</param>
		/// <param name="right">The second GRaff.Matrix to compare.</param>
		/// <returns>true if all elements of the two GRaff.Matrix objects are equal.</returns>
		public static bool operator ==(Matrix? left, Matrix? right)
			=> left?.Equals(right) ?? false;

		/// <summary>
		/// Compares two GRaff.Matrix objects. The result specifies whether all their elements are unequal.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix to compare.</param>
		/// <param name="right">The second GRaff.Matrix to compare.</param>
		/// <returns>true if all elements of the two GRaff.Matrix objects are unequal.</returns>
		public static bool operator !=(Matrix? left, Matrix? right)
			=> !(left == right);

		/// <summary>
		/// Computes the element-wise sum of the two GRaff.Matrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix.</param>
		/// <param name="right">The second GRaff.Matrix.</param>
		/// <returns>The sum of the elements of each GRaff.Matrix.</returns>
		public static Matrix operator +(Matrix left, Matrix right)
		    => new Matrix(left.M00 + right.M00, left.M01 + right.M01, left.M02 + right.M02,
                          left.M10 + right.M10, left.M11 + right.M11, left.M12 + right.M12);


		/// <summary>
		/// Computes the element-wise difference of the two GRaff.Matrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix.</param>
		/// <param name="right">The second GRaff.Matrix.</param>
		/// <returns>The difference of the elements of each GRaff.Matrix.</returns>
		public static Matrix operator -(Matrix left, Matrix right)
		    => new Matrix(left.M00 - right.M00, left.M01 - right.M01, left.M02 - right.M02,
                          left.M10 - right.M10, left.M11 - right.M11, left.M12 - right.M12);


		/// <summary>
		/// Computes the matrix product of the two GRaff.Matrix objects.
		/// </summary>
		/// <param name="left">The first GRaff.Matrix.</param>
		/// <param name="right">The second GRaff.Matrix.</param>
		/// <returns>The matrix product of the two GRaff.Matrix.</returns>
		public static Matrix operator *(Matrix left, Matrix right)
		    => new Matrix(
					left.M00 * right.M00 + left.M01 * right.M10, left.M00 * right.M01 + left.M01 * right.M11, left.M00 * right.M02 + left.M01 * right.M12 + left.M02,
					left.M10 * right.M00 + left.M11 * right.M10, left.M10 * right.M01 + left.M11 * right.M11, left.M10 * right.M02 + left.M11 * right.M12 + left.M12
				);


		public static Matrix operator *(Matrix left, double right)
		    => new Matrix(left.M00 * right, left.M01 * right, left.M02 * right,
                          left.M10 * right, left.M11 * right, left.M12 * right);


        public static Matrix operator *(double left, Matrix right)
            => new Matrix(left * right.M00, left * right.M01, left * right.M02,
                          left * right.M10, left * right.M11, left * right.M12);


		public static Matrix operator /(Matrix left, double right)
		    => new Matrix(left.M00 / right, left.M01 / right, left.M02 / right,
                          left.M10 / right, left.M11 / right, left.M12 / right);


		/// <summary>
		/// Computes the matrix product of the GRaff.Matrix and the GRaff.Point.
		/// This constitutes performing the affine transformation on that GRaff.Point.
		/// </summary>
		/// <param name="m">A GRaff.Matrix representing the affine transformation.</param>
		/// <param name="p">A GRaff.Point to be transformed by the affine transformation.</param>
		/// <returns>The transformed GRaff.Point.</returns>
		public static Point operator *(Matrix m, Point p)
		    => new Point(m.M00 * p.X + m.M01 * p.Y + m.M02, m.M10 * p.X + m.M11 * p.Y + m.M12);


		/// <summary>
		/// Computes the matrix product of the GRaff.Matrix and the GRaff.Vector.
		/// This constitutes performing the affine transformation on that GRaff.Vector.
		/// </summary>
		/// <param name="m">A GRaff.Matrix representing the affine transformation.</param>
		/// <param name="v">A GRaff.Vector to be transformed by the affine transformation.</param>
		/// <returns>The transformed GRaff.Vector.</returns>
		public static Vector operator *(Matrix m, Vector v)
		    => new Vector(m.M00 * v.X + m.M01 * v.Y, m.M10 * v.X + m.M11 * v.Y);


        public static Line operator *(Matrix m, Line l)
            => new Line(m * l.Origin, m * l.Destination);


		public static Triangle operator *(Matrix left, Triangle right)
		    => new Triangle(left * right.V1, left * right.V2, left * right.V3);


        public static Quadrilateral operator *(Matrix left, Quadrilateral right)
            => new Quadrilateral(left * right.V1, left * right.V2, left * right.V3, left * right.V4);


        public static Quadrilateral operator *(Matrix m, Rectangle r)
            => new Quadrilateral(m * r.TopLeft, m * r.TopRight, m * r.BottomRight, m * r.BottomLeft);


        public static Polygon operator *(Matrix left, Polygon right)
            => new Polygon(right.Vertices.Select(p => left * p).ToArray());

	}
}
