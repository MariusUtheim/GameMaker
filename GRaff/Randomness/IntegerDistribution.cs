﻿using System;
using System.Diagnostics.Contracts;

namespace GRaff.Randomness
{
    /// <summary>
    /// Generator for integers according to a uniform distribution.
    /// </summary>
	public sealed class IntegerDistribution : IDistribution<int>
	{
		private readonly Random _rnd;
        private readonly int _lowerInclusive, _upperExclusive;

        public IntegerDistribution(int lowerInclusive, int upperExclusive)
			: this(GRandom.Source, lowerInclusive, upperExclusive) { }

        public IntegerDistribution(Random rnd, int lowerInclusive, int upperExclusive)
		{
			_rnd = rnd;
			_lowerInclusive = lowerInclusive;
			_upperExclusive = upperExclusive;
		}

		public int Generate() => _rnd.Integer(_lowerInclusive, _upperExclusive);
	}
}
