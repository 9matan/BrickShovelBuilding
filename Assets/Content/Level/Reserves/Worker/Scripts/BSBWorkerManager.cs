using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBWorkerManager
	{
		BSBPrice workerPrice { get; }
    bool TryHireWorker();
    void HireWorker();
	}

	public class BSBWorkerManager : MonoBehaviour,
		IBSBWorkerManager,
		IVOSInitializable
	{

		public IBSBPlayerResources			playerResources
		{
			get { return BSBDirector.playerResources; }
		}
		public IBSBPriceManager				priceManager
		{
			get { return BSBDirector.priceManager; }
		}
		public IBSBBarracksBuildingManager	barracksManager
		{
			get { return BSBDirector.barracksManager; }
		}

		public BSBPrice workerPrice
		{
			get { return priceManager.GetWorkerPrice(_workerPrice); }
		}

		[SerializeField]
		protected BSBWorkerFactory _factory;

		[Header("Price")]
		[SerializeField]
		protected BSBPrice	_workerPrice;
		[SerializeField]
		protected int		_purchasingQuantity = 1;


		protected Dictionary<int, BSBWorker> _workers = new Dictionary<int, BSBWorker>();

		//
		// < Initialize >
		//

		public void Initialize()
		{
			_factory.Initialize();
		}

		//
		// </ Initialize >
		//

		public void HireWorker()
		{
			if (!TryHireWorker())
				return;

			var worker = _CreateWorker();
			worker.Initialize();
			_AddWorker(worker);
			_HireWorker(worker);
		}		

		public void HireOneWorkerFree()
		{
			var worker = _CreateWorker();
			worker.Initialize();
			_AddWorker(worker);
			_HireOneWorkerFree(worker);
		}

		public bool TryHireWorker()
		{
			if (playerResources.freeWorkersCapacity == 0)
				return false;

			return playerResources.Contains(workerPrice);
		}




		protected BSBWorker _CreateWorker()
		{
			return _factory.Allocate();
		}

		protected void _AddWorker(BSBWorker worker)
		{
			if (!barracksManager.AddWorker(worker))
				Debug.LogError("Not enough space for worker!");
			_workers.Add(worker.id, worker);
		}

		protected void _HireWorker(IBSBWorker worker)
		{
			playerResources.Use(workerPrice);
			var reserves = new BSBPrice();
			reserves.workers = _purchasingQuantity;
			playerResources.Add(reserves);
		}

		protected void _HireOneWorkerFree(IBSBWorker worker)
		{
			var reserves = new BSBPrice();
			reserves.workers = 1;
			playerResources.Add(reserves);
		}

		//
		// < Clear >
		//

		public void Clear()
		{

		}

		//
		// </ Clear >
		//


		//
		// < Log >
		//

		public bool debug = false;
				
		public void Log(object msg)
		{
			if(debug)
				Debug.Log(msg);
		}		

		//
		// </ Log >
		//
		
	}

}
