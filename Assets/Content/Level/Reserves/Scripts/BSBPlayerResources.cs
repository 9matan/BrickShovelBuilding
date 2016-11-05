using UnityEngine;
using System.Collections;
using BSB;
using System.Collections.Generic;

namespace BSB
{

	public interface IBSBPlayerResources
	{
		IBSBReserveStorage fundsStorage { get; }
		IBSBReserveStorage workersStorage { get; }
		IBSBReserveStorage materialsStorage { get; }		

		int funds { get; }
		int workers { get; }
		int materials { get; }

		int workersCapacity { get; }
		int freeWorkersCapacity { get; }

		void Use(IBSBReserves reserves);
		bool Contains(IBSBReserves reserves);
		void Restore(IBSBReserves reserves);
		void Add(IBSBReserves reserves);

		void Use(BSBPrice reserves);
		bool Contains(BSBPrice reserves);
		void Restore(BSBPrice reserves);
		void Add(BSBPrice reserves);
	}

	public class BSBPlayerResources : MonoBehaviour,
		IBSBPlayerResources,
		IVOSInitializable
	{

		public IBSBBarracksBuildingManager	barracksManager
		{
			get { return BSBDirector.barracksManager; }
		}
		public IBSBPriceManager				priceManager
		{
			get { return BSBDirector.priceManager; }
		}
		public IBSBTimeManager				timeManager
		{
			get { return BSBDirector.timeManager; }
		}

		public IBSBReserveStorage fundsStorage
		{
			get
			{
				return _reserves.funds;
			}
		}
		public IBSBReserveStorage workersStorage
		{
			get
			{
				return _reserves.workers;
			}
		}
		public IBSBReserveStorage materialsStorage
		{
			get
			{
				return _reserves.materials;
			}
		}

		public bool isInit
		{
			get; protected set;
		}

		public int funds
		{
			get { return fundsStorage.amount; }
		}
		public int workers
		{
			get { return workersStorage.amount; }
		}
		public int materials
		{
			get { return materialsStorage.amount; }
		}

		public int workersCapacity
		{
			get { return workersStorage.capacity; }
		}
		public int freeWorkersCapacity
		{
			get { return workersStorage.freeCapacity; }
		}

		public BSBPrice income
		{
			get
			{
				return priceManager.Inflation(_income);
			}
			set { _income = value; }
		}

		[SerializeField]
		protected BSBPrice _income;

		protected BSBReservesStorage _reserves = new BSBReservesStorage();


		//
		// < Initialize >
		//
	
		public void Initialize()
		{
			_InitializeReserves();

			_ListenBarracksManager(barracksManager);
			_ListenTimeManager(timeManager);

			isInit = true;
		}

		protected void _InitializeReserves()
		{
		}

		protected void _ListenBarracksManager(IBSBBarracksBuildingManagerEvents events)
		{
			events.onBarracksBuilt += _OnBarracksBuilt;
			events.onBarracksUpgraded += _OnBarracksUpgraded;
		}

		protected void _ListenTimeManager(IBSBTimeManagerEvents events)
		{
			events.onMonthEnded += _UpdateIncome;
		}

		//
		// </ Initialize >
		//

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

		public void Add(IBSBReserves __reserves)
		{
			_reserves.AddMeans(__reserves);
		}

		public void Use(BSBPrice __reserves)
		{
			Use(new BSBReserves(__reserves));
		}

		public void Add(BSBPrice __reserves)
		{
			Add(new BSBReserves(__reserves));
		}

		public bool Contains(BSBPrice __reserves)
		{
			return Contains(new BSBReserves(__reserves));
		}

		public void Restore(BSBPrice __reserves)
		{
			Restore(new BSBReserves(__reserves));
		}

		//
		// < Building events >
		//

		protected void _OnBarracksBuilt(IBSBBarracksBuilding barracks)
		{
			_UpdateWorkersCapacity(barracks.upgradedCapacity);
		}

		protected void _OnBarracksUpgraded(IBSBBarracksBuilding barracks)
		{
			_UpdateWorkersCapacity(barracks.upgradedCapacity);
		}

		//
		// </ Building events >
		//

		protected void _UpdateIncome()
		{
			Add(income);
		}

		protected void _UpdateWorkersCapacity(int delta)
		{
			workersStorage.Extend(delta);
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
