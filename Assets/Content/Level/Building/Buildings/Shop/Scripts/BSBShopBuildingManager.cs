using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBShopBuildingManager :
		IBSBShopBuildingManagerEvents
	{
		BSBPrice GetTotalIncome();
	}

	public interface IBSBShopBuildingManagerEvents
	{
		event Events.OnShopBuildingAction onShopBuilt;
		event Events.OnShopBuildingAction onShopUpgraded;
	}

	public class BSBShopBuildingManager : MonoBehaviour,
		IBSBShopBuildingManager
	{

		public IBSBBuildingManager		buildingManager
		{
			get { return BSBDirector.buildingManager; }
		}
		public IBSBHouseBuildingManager	houseManager
		{
			get { return BSBDirector.houseManager; }
		}
		public IBSBPriceManager			priceManager
		{
			get { return BSBDirector.priceManager; }
		}
		public IBSBPlayerResources		playerResources
		{
			get { return BSBDirector.playerResources; }
		}


		[SerializeField]
		protected BSBShopBuildingInfo _info;

		protected Dictionary<int, BSBShopBuilding> _shopsContainer = new Dictionary<int, BSBShopBuilding>();


		//
		// < Initialize >
		//

		public void Initialize()
		{
		}

		//
		// </ Initialize >
		//


			
		public BSBPrice GetTotalIncome()
		{
			BSBPrice totalIncome = new BSBPrice();

			foreach (var kvp in _shopsContainer)
			{
				var income = _info.GetIncomeByLevel(kvp.Value.level);
				totalIncome.funds += (income.funds * houseManager.houseCount);
				totalIncome.materials += income.materials;
			}

			return totalIncome;
		}



		public void AddShop(BSBShopBuilding shop)
		{
			_shopsContainer.Add(shop.id, shop);
		}

		public void RemoveShop(BSBShopBuilding shop)
		{
			_shopsContainer.Remove(shop.id);
		}

		//
		// < Events >
		//

		public event Events.OnShopBuildingAction onShopBuilt = delegate { };
		public event Events.OnShopBuildingAction onShopUpgraded = delegate { };

		public void OnShopBuildingBuilt(BSBShopBuilding shop)
		{
			onShopBuilt(shop);
		}

		public void OnShopBuildingUpgraded(BSBShopBuilding shop)
		{
			onShopUpgraded(shop);
		}

		//
		// </ Events >
		//

		public List<IBSBObjectOperation> GetOperations(IBSBBuilding building)
		{
			var list = new List<IBSBObjectOperation>();
			return list;
		}

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
