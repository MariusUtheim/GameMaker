﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GRaff.Graphics.Text
{
	public sealed class TextRenderer
	{
		private static readonly Regex NewlineRegex = new Regex("\r\n|\n");
		
		public TextRenderer(Font font, FontAlignment alignment = FontAlignment.TopLeft, int width = Int32.MaxValue)
			: this(font, alignment, width, font.Height)
		{
			Contract.Requires<ArgumentNullException>(font != null);
		}

		public TextRenderer(Font font, FontAlignment alignment, int width, double lineSeparation)
		{
			Contract.Requires<ArgumentNullException>(font != null);
			this.Font = font;
			this.Alignment = alignment;
			this.Width = width;
			this.LineSeparation = lineSeparation;
		}

		[ContractInvariantMethod]
		private void objectInvariants()
		{
			Contract.Invariant(Font != null);
		}

		private static bool _isNewline(char c)
		{
			return c == '\r' || c == '\n';
		}

		public Font Font { get; set; }

		public int Width { get; set; }

		public double LineSeparation { get; set; }

		public FontAlignment Alignment { get; set; } = FontAlignment.TopLeft;

		private string _multilineFormat(string text)
		{
			Contract.Assume(text != null);
			var words = text.Split(' ');
			var multilineFormat = new StringBuilder(text.Length);
			var currentLine = new StringBuilder(words[0]);
			var currentLineLength = GetWidth(words[0]);
			var lengthOfSpace = GetWidth(" ");

			var lengths = words.Select(word => GetWidth(word));

			for (var i = 1; i < words.Length; i++)
			{
				var wordLength = GetWidth(words[i]);
				if (currentLineLength + wordLength < Width)
				{
					currentLine.Append(" " + words[i]);
					currentLineLength += lengthOfSpace + wordLength;
				}
				else
				{
					multilineFormat.AppendLine(currentLine.ToString());

					currentLine = new StringBuilder(words[i]);
					currentLineLength = wordLength;
				}
			}

			multilineFormat.AppendLine(currentLine.ToString());

			var result = multilineFormat.ToString().TrimEnd(Environment.NewLine.ToCharArray());
			return result;
		}

		public string MultilineFormat(string text)
		{
			return text == null ? "" : String.Concat(Regex.Split(text, Environment.NewLine).Select(str => _multilineFormat(str)));
		}

		public string[] LineSplit(string text)
		{
			return text == null ? new string[0] : NewlineRegex.Split(text).Select(str => _multilineFormat(str)).SelectMany(str => NewlineRegex.Split(str)).ToArray();
		}

		public string Truncate(string text)
		{
			if (text == null)
				return "";

			var ellipsisWidth = Font.GetWidth("...");
			var lowerBound = Width - ellipsisWidth;
			var offset = 0;

			for (var i = 0; i < text.Length; i++)
			{
				if (_isNewline(text[i]))
					return text.Substring(0, i) + "...";

				var nextWidth = Font.GetWidth(text[i]);
				var advance = Font.GetAdvance(text, i);

				if (offset + nextWidth > Width)
					return text.Substring(0, i) + "...";

				if (offset + advance >= lowerBound)
				{
					offset += advance;
					for (var j = i + 1; j < text.Length; j++)
					{
						nextWidth = Font.GetWidth(text[j]);
						if (offset + nextWidth > Width)
							return text.Substring(0, i) + "...";
						offset += Font.GetAdvance(text, j);
                    }
					break;
				}

				offset += advance;
			}

			return text;
		}

		public int GetWidth(string text)
		{
			return Font.GetWidth(text);
		}



		internal string[] RenderCoords(string text, out GraphicsPoint[] quadCoords)
		{
			var lines = LineSplit(text);
			var length = lines.Sum(line => line.Length);

			var coords = new GraphicsPoint[4 * length];

			var x0 = 0.0;
			var y0 = 0.0;
			
			switch (Alignment & FontAlignment.Vertical)
			{
				case FontAlignment.Top: y0 = 0; break;
				case FontAlignment.Center: y0 = -(LineSeparation * (lines.Length - 1) + Font.Height) / 2; break;
				case FontAlignment.Bottom: y0 = -(LineSeparation * (lines.Length - 1) + Font.Height); break;
			}

			var coordIndex = 0;
			for (var l = 0; l < lines.Length; l++)
			{
				var lineWidth = Font.GetWidth(lines[l]);
				switch (Alignment & FontAlignment.Horizontal)
				{
					case FontAlignment.Left: x0 = 0; break;
					case FontAlignment.Center: x0 = -lineWidth / 2f; break;
					case FontAlignment.Right: x0 = -lineWidth; break;
				}

				var x = x0;
				var y = y0 + l * LineSeparation;
				for (var i = 0; i < lines[l].Length; i++)
				{
					FontCharacter c;
					if (Font.TryGetCharacter(lines[l][i], out c))
					{
						coords[coordIndex] = new GraphicsPoint(x + c.XOffset, y + c.YOffset);
						coords[coordIndex + 1] = new GraphicsPoint(x + c.XOffset + c.Width, y + c.YOffset);
						coords[coordIndex + 2] = new GraphicsPoint(x + c.XOffset + c.Width, y + c.YOffset + c.Height);
						coords[coordIndex + 3] = new GraphicsPoint(x + c.XOffset, y + c.YOffset + c.Height);
						x += c.XAdvance;
						if (i < lines[l].Length - 1)
							x += Font.GetKerning(lines[l][i], lines[l][i + 1]);
					}
					coordIndex += 4;
				}
			}

			quadCoords = coords;
			return lines;
		}

		internal void RenderTexCoords(string str, int offset, ref GraphicsPoint[] texCoords)
		{
			double tXScale = 1.0 / Font.Buffer.Width, tYScale = 1.0 / Font.Buffer.Height;

			for (var i = 0; i < str.Length; i++)
			{
				if (str[i] == '\n')
					continue;

				var index = 4 * (i + offset);

				FontCharacter character;
				if (Font.TryGetCharacter(str[i], out character))
				{
					texCoords[index] = new GraphicsPoint(tXScale * character.X, tYScale * character.Y);
					texCoords[index + 1] = new GraphicsPoint(tXScale * (character.X + character.Width), tYScale * character.Y);
					texCoords[index + 2] = new GraphicsPoint(tXScale * (character.X + character.Width), tYScale * (character.Y + character.Height));
					texCoords[index + 3] = new GraphicsPoint(tXScale * character.X, tYScale * (character.Y + character.Height));
				}
				else
				{
					texCoords[index] = GraphicsPoint.Zero;
					texCoords[index + 1] = GraphicsPoint.Zero;
					texCoords[index + 2] = GraphicsPoint.Zero;
					texCoords[index + 3] = GraphicsPoint.Zero;
				}
			}
		}

	}
}
