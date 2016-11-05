using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBShopBuildingManager :
		IBSBShopBuildingManagerEvents
	{ 

	}

	public interface IBSBShopBuildingManagerEvents
	{
		event Events.OnShopBuildingAction onShopBuilt;
		event Events.OnShopBuildingAction onShopUpgraded;
	}

	public class BSBShopBuildingManager : MonoBehaviour,
		IBSBShopBuildingManager
	{

		public IBSBBuildingManager	buildingManager
		{
			get { return BSBDirector.buildingManager; }
		}
		public IBSBPriceManager		priceManager
		{
			get { return BSBDirector.priceManager; }
		}
		public IBSBPlayerResources	playerResources
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

	//	public void OnShopBuildingBuilt

		//
		// </ Events >
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
