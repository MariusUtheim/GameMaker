﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Particles
{
	public class ColorBehavior : IParticleBehavior
	{
		private Color _color;

		public ColorBehavior(Color color)
		{
			this._color = color;
		}

		public void AttachTo(Particle particle)
		{
			particle.Color = _color;
		}
	}
}