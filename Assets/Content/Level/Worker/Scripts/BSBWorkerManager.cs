using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBWorkerManager
	{
		BSBPrice workerPrice { get; }
	}

	public class BSBWorkerManager : MonoBehaviour,
		IBSBWorkerManager
	{

		public IBSBPlayerResources	playerResources
		{
			get { return BSBDirector.playerResources; }
		}
		public IBSBPriceManager		priceManager
		{
			get { return BSBDirector.priceManager; }
		}

		public BSBPrice workerPrice
		{
			get { return priceManager.GetWorkerPrice(_workerPrice); }
		}

		[Header("Price")]
		[SerializeField]
		protected BSBPrice _workerPrice;
		[SerializeField]
		protected int _purchasingQuantity = 1;

		//
		// < Initialize >
		//

		public void Initialize()
		{

		}

		//
		// </ Initialize >
		//

		public void HireWorker()
		{
			if (!TryHireWorker())
				return;

			var worker = _CreateWorker();
			_HireWorker(worker);
		}		

		public bool TryHireWorker()
		{
			if (playerResources.workers.freeCapacity == 0)
				return false;

			return playerResources.Contains(workerPrice);
		}

		protected BSBWorker _CreateWorker()
		{
			return null;
		}

		protected void _HireWorker(IBSBWorker worker)
		{
			playerResources.Use(workerPrice);
			var reserves = new BSBPrice();
			reserves.workers = _purchasingQuantity;
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
