﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.Graphics.Particles
{
	public class ParticleSystem : GameElement
	{
		protected readonly LinkedList<Particle> particles = new LinkedList<Particle>();

		// Hiding this from subclasses 
		private readonly SerialRenderSystem _renderSystem;

        public int Count => particles.Count;

		public ParticleSystem(ParticleType type)
		{
			_renderSystem = new SerialRenderSystem();
			this.ParticleType = type;
		}
		
		public bool DestroyAutomatically { get; set; }

		public ParticleType ParticleType { get; private set; }

		protected void Remove(Particle particle)
		{
			particles.Remove(particle);
		}

		public void Create(double x, double y, int count)
		{
			for (int i = 0; i < count; i++)
				particles.AddFirst(ParticleType.Generate(x, y));
		}

		public void Create(Point location, int count)
		{
			Create(location.X, location.Y, count);
		}

		public void Create(IEnumerable<Point> pts)
		{
			foreach (var p in pts)
				Create(p.X, p.Y, 1);
		}

		public override void OnStep()
		{
			ConcurrentBag<Particle> removeBag = new ConcurrentBag<Particle>();
			Parallel.ForEach(particles, particle =>
			{
				if (!particle.Update())
					removeBag.Add(particle);
			});

			var toRemove = new HashSet<Particle>(removeBag.ToArray());

			for (var next = particles.First; next != null; )
			{
				var current = next;
				next = next.Next;
				if (toRemove.Contains(current.Value))
					particles.Remove(current);
			}

			if (DestroyAutomatically && particles.Count == 0)
				this.Destroy();
		}

		public override void OnDraw()
		{
			ParticleType.Render(particles);
		}
	}
}
