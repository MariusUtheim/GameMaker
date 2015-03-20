﻿

namespace GRaff.Particles
{
	public class AccelerationProperty : IParticleBehavior
	{
		public AccelerationProperty(Vector gravity)
			:this(gravity, 0) { }

		public AccelerationProperty(double friction)
			: this(Vector.Zero, friction) { }

		public AccelerationProperty(Vector gravity, double friction)
		{
			this.Gravity = gravity; 
			this.Friction = friction;
		}

		public double Friction { get; set; }

		public Vector Gravity { get; set; }

		public void AttachTo(Particle particle)
		{
			if (Gravity != Vector.Zero)
				particle.AttachBehavior(new GravityProperty(Gravity));
			if (Friction != 0)
				particle.AttachBehavior(new FrictionProperty(Friction));
		}
	}
}
