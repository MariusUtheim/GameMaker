﻿using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GRaff.Graphics;

namespace GRaff.Particles
{
	public class ParticleSystem : GameElement, IEnumerable<Particle>
	{
		protected LinkedList<Particle> particles;

		// Hiding this from subclasses 
		private TexturedRenderSystem _renderSystem;

		public int Count { get { return particles.Count; } }

		public ParticleSystem(ParticleType type)
		{
			particles = new LinkedList<Particle>();
			_renderSystem = new TexturedRenderSystem();
			this.ParticleType = type;
		}
		
		public ParticleType ParticleType { get; private set; }

		public IEnumerator<Particle> GetEnumerator()
		{
			return particles.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

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
			Parallel.ForEach(this, particle =>
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
		}

		public override void OnDraw()
		{
			ParticleType.Render(particles);
		}
	}
}