﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GRaff.Synchronization;

namespace GRaff
{
	#if !PUBLISH
	#warning Missing documentation
	public sealed class AssetBatch : IAsset
	{
		private List<IAsset> _resources;

		public AssetBatch()
		{
			_resources = new List<IAsset>();
		}

		public AssetBatch(params IAsset[] resources)
		{
			_resources = new List<IAsset>(resources);
		}

		public void Load()
		{
			foreach (var resource in _resources)
				resource.Load();
		}

		public IAsyncOperation LoadAsync()
		{
			throw new NotImplementedException();
		}

		public void Unload()
		{
			foreach (var resource in _resources)
				resource.Unload();
		}

		/// <summary>
		/// Gets whether every GRaff.IAsset objects in this GRaff.AssetBatch is loaded. 
		/// </summary>
		public bool IsLoaded
		{
			get
			{
				return _resources.All(resource => resource.IsLoaded);
			}
		}
	}
	#endif
}
