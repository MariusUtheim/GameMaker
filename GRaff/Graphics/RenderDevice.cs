﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using GRaff.Graphics.Text;
#if OpenGL4
using OpenTK.Graphics.OpenGL4;
#else
using OpenTK.Graphics.ES30;
using GLPrimitiveType = OpenTK.Graphics.ES30.PrimitiveType;
#endif

namespace GRaff.Graphics
{
    internal class RenderDevice : IRenderDevice
	{
		private readonly SerialRenderSystem _renderSystem = new SerialRenderSystem();
        private readonly InterleavedRenderSystem _interleaved = new InterleavedRenderSystem();


		public RenderDevice()
		{
            _renderSystem.QuadTexCoords(UsageHint.StaticDraw, 1);
		}

        //TODO// Copy to framebuffer

        
        public void Clear(Color color)
		{
			GL.ClearColor(color.ToOpenGLColor());
            GL.Clear(ClearBufferMask.ColorBufferBit);
		}

		public void Draw(PrimitiveType type, GraphicsPoint[] vertices, Color[] colors)
		{
			Contract.Requires<ArgumentException>(vertices.Length == colors.Length);
            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColors(colors);
            _renderSystem.Render(type);
		}

        public void Draw(PrimitiveType type, GraphicsPoint[] vertices, Color color)
        {
            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColor(color);
            _renderSystem.Render(type);
        }

        public void Draw(PrimitiveType type, GraphicsVertex[] primitive)
        {
            _interleaved.SetPrimitive(primitive);
            _interleaved.Render(type);
        }
        

        public void FillEllipse(Color color, GraphicsPoint location, double hRadius, double vRadius)
        {
            _renderSystem.SetVertices(Polygon.Ellipse(location, hRadius, vRadius).Tesselate());
            _renderSystem.SetColor(color);
            _renderSystem.Render(PrimitiveType.LineLoop);
        }
        
        public void FillEllipse(Color innerColor, Color outerColor, GraphicsPoint center, double hRadius, double vRadius)
        {
            if (hRadius == 0 && vRadius == 0)
            {
                Draw(PrimitiveType.Points, new[] { center }, innerColor);
                return;
            }
            var ellipse = Polygon.Ellipse(center, hRadius, vRadius);
            var vertices = new GraphicsPoint[ellipse.Length + 2];
            int i = 0;
            vertices[i++] = center;
            foreach (var p in ellipse.Vertices)
                vertices[i++] = (GraphicsPoint)p;
            vertices[vertices.Length - 1] = new GraphicsPoint(center.X + hRadius, center.Y);

            var colors = new Color[ellipse.Length + 2];
            colors[0] = innerColor;
            for (int j = 1; j < colors.Length; j++)
                colors[j] = outerColor;

            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColors(colors);
            _renderSystem.Render(PrimitiveType.TriangleFan);
        }


		public void DrawPolygon(Color color, Polygon polygon)
		{
            _renderSystem.SetVertices(polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray());
            _renderSystem.SetColor(color);

			if (polygon.Length == 1)
				_renderSystem.Render(PrimitiveType.Points);
			else if (polygon.Length == 2)
				_renderSystem.Render(PrimitiveType.Lines);
			else
				_renderSystem.Render(PrimitiveType.LineLoop);
		}

        public void FillPolygon(Color color, Polygon polygon)
        {
            _renderSystem.SetVertices(polygon.Vertices.Select(v => (GraphicsPoint)v).ToArray());
            _renderSystem.SetColor(color);

            if (polygon.Length == 1)
                _renderSystem.Render(PrimitiveType.Points);
            else if (polygon.Length == 2)
                _renderSystem.Render(PrimitiveType.Lines);
            else
                _renderSystem.Render(PrimitiveType.TriangleFan);
        }


        public void DrawTexture(SubTexture texture, double xOrigin, double yOrigin, Matrix transform, Color blend)
        {
            Contract.Requires<ObjectDisposedException>(!texture.Texture.IsDisposed);
            
            _renderSystem.SetVertices(new[] {
                transform * new GraphicsPoint(-xOrigin, -yOrigin),
                transform * new GraphicsPoint(texture.Width - xOrigin, -yOrigin),
                transform * new GraphicsPoint(-xOrigin, texture.Height - yOrigin),
				transform * new GraphicsPoint(texture.Width - xOrigin, texture.Height - yOrigin),
            });
            _renderSystem.SetColor(blend);
            
            _renderSystem.SetTexCoords(texture.TriangleStripCoords);
            _renderSystem.Render(texture.Texture, PrimitiveType.TriangleStrip);
        }

        public void DrawTexture(Texture buffer, PrimitiveType type, GraphicsPoint[] vertices, Color blend, GraphicsPoint[] texCoords)
        {
            Contract.Requires<ObjectDisposedException>(!buffer.IsDisposed);

            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColor(blend);
            _renderSystem.SetTexCoords(texCoords);
            _renderSystem.Render(buffer, type);
        }

        public void DrawTexture(Texture buffer, PrimitiveType type, GraphicsPoint[] vertices, Color[] colors, GraphicsPoint[] texCoords)
        {
            Contract.Requires<ObjectDisposedException>(!buffer.IsDisposed);

            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColors(colors);
            _renderSystem.SetTexCoords(texCoords);
            _renderSystem.Render(buffer, type);
        }



        public void DrawText(TextRenderer renderer, Color color, string text, Matrix transform)
		{
            Contract.Requires<ObjectDisposedException>(!renderer.Font.IsDisposed);

			if (text == null)
				return;

            (var vertices, var texCoords) = renderer.RenderVertices(text, transform);

            _renderSystem.SetVertices(vertices);
            _renderSystem.SetColor(color);
            _renderSystem.SetTexCoords(texCoords);
            _renderSystem.Render(renderer.Font.Texture, PrimitiveType.Triangles);
		}


        public void Redraw()
        {
            _Graphics.ErrorCheck();
            var texture = Texture.FromScreen();

            using (View.FullWindow().Use())
            {
                _renderSystem.SetVertices(new GraphicsPoint(0, 0), new GraphicsPoint(Window.Width, 0), new GraphicsPoint(0, Window.Height), new GraphicsPoint(Window.Width, Window.Height));
                _renderSystem.SetColor(Colors.White);
                _renderSystem.SetTexCoords(UsageHint.StreamDraw, new[] { 0.0, 0.0, 1.0, 0.0, 0.0, 1.0, 1.0, 1.0 });
                _renderSystem.Render(texture, PrimitiveType.TriangleStrip);
            }

			texture.Dispose();
            _Graphics.ErrorCheck();
        }
    }
}
