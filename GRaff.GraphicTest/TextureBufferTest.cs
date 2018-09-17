﻿using GRaff.Graphics;
using GRaff.Effects;

namespace GRaff.GraphicTest
{
    [Test]
	class TextureBufferTest : GameElement
	{
		private Texture _texture = Textures.Giraffe;
		private Texture _chessboard, _mono, _linear, _sinusoidal;

        public TextureBufferTest()
        {
            _chessboard = TextureGenerator.Generate(TextureGenerator.Chessboard(Colors.LightSlateGray, Colors.Black, (4, 4)), (80, 110));
            _mono = TextureGenerator.Generate(TextureGenerator.Monocolored(Colors.LightPink), (80, 110));
            _linear = TextureGenerator.Generate(TextureGenerator.Linear(Colors.MediumPurple, Colors.Blue, Colors.Red, Colors.ForestGreen), (80, 110));
            _sinusoidal = TextureGenerator.Generate(TextureGenerator.Sinusoidal(Colors.Purple, Colors.Blue, Colors.Red, Colors.ForestGreen), (80, 110));
        }

		
		public override void OnDraw()
		{
			Draw.Clear(Colors.LightGray);
			Draw.Texture(_texture, (0, 0));
            Draw.SubTexture(new SubTexture(_texture), new Rectangle(_texture.Width, 0, 400, 400), Colors.MediumPurple, Colors.Blue, Colors.Red, Colors.ForestGreen);
			Draw.Texture(_chessboard, (10, _texture.Height + 10));
            Draw.Texture(_mono, (110, _texture.Height + 10));
            Draw.Texture(_linear, (210, _texture.Height + 10));
            Draw.Texture(_sinusoidal, (310, _texture.Height + 10));
		}
	}
}
