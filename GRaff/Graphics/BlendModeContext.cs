﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRaff.Graphics
{
	public sealed class BlendModeContext : IDisposable
	{
		private BlendMode _previous;
		private bool _isDisposed = false;

		public BlendModeContext(BlendMode mode) /*C#6.0*/
		{
			_previous = ColorMap.BlendMode;
			ColorMap.BlendMode = mode;
		}

		void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				ColorMap.BlendMode = _previous;
				_previous = null;
			}
		}

		~BlendModeContext() {
		  Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
