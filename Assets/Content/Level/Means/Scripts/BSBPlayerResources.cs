using UnityEngine;
using System.Collections;
using BSB;
using System;
using System.Collections.Generic;

namespace BSB
{

	public interface IBSBPlayerResources
	{
		IBSBFundsMeans funds { get; }
		IBSBWorkersMeans workers { get; }
		IBSBMaterialsMeans materials { get; }

		void Use(IBSBReserves __reserves);
		bool Contains(IBSBReserves __reserves);
		void Restore(IBSBReserves __reserves);
	}

	public class BSBPlayerResources : MonoBehaviour,
		IBSBPlayerResources
	{

		public IBSBFundsMeans funds
		{
			get
			{
				return _reserves.funds;
			}
		}
		public IBSBWorkersMeans workers
		{
			get
			{
				return _reserves.workers;
			}
		}
		public IBSBMaterialsMeans materials
		{
			get
			{
				return _reserves.materials;
			}
		}
		
		[SerializeField]
		protected BSBReserves _reserves = new BSBReserves();

		public void Use(IBSBReserves __reserves)
		{
			_reserves.UseMeans(__reserves);
		}

		public bool Contains(IBSBReserves __reserves)
		{
			return _reserves.ContainsMeans(__reserves);
		}

		public void Restore(IBSBReserves __reserves)
		{
			_reserves.RestoreMeans(__reserves);
		}

		//
		// < Log >
		//

		public bool debug = false;

		public void Log(object msg)
		{
			if (debug)
				Debug.Log(msg);
		}

		//
		// </ Log >
		//

	}

}
