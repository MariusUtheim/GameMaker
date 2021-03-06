﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
#endif


namespace GRaff.Graphics
{
	public static class GLExtensions
	{

        public static GraphicsPoint[] TriangleStripCoordinates(this Rectangle rect)
        {
            return new[]
            {
                new GraphicsPoint(rect.Left, rect.Top),
                new GraphicsPoint(rect.Right, rect.Top),
                new GraphicsPoint(rect.Left, rect.Bottom),
                new GraphicsPoint(rect.Right, rect.Bottom)
            };
        }

        public static GraphicsPoint[] Tesselate(this Polygon polygon)
		{
			var result = new GraphicsPoint[polygon.Length];
			for (int i = 0, sign = 1; i < polygon.Length; i++, sign = -sign)
				result[i] = (GraphicsPoint)polygon.Vertex(i * sign);
			return result;
		}

        public static GraphicsPoint[] Outline(this Polygon polygon)
            => polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray();

		public static void Bind(this Texture texture)
		{
            Contract.Requires<ObjectDisposedException>(!texture.IsDisposed);
			GL.BindTexture(TextureTarget.Texture2D, texture.Id);
		}

	}
}
