using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{


	public interface IBSBHouseBuildingManager
	{
		BSBPrice			GetHouseSellPrice(IBSBHouseBuilding house);
		void				SellHouse(IBSBHouseBuilding house);
		IBSBHouseBuilding	GetHouseById(int id);
	}


	public class BSBHouseBuildingManager : MonoBehaviour 
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
		protected BSBHouseBuildingInfo _info;

		[SerializeField]
		protected Dictionary<int, BSBHouseBuilding> _housesContainer = new Dictionary<int, BSBHouseBuilding>();

		//
		// < Initialize >
		//

		public void Initialize()
		{
		}

		//
		// </ Initialize >
		//

		public BSBPrice GetHouseSellPrice(IBSBHouseBuilding house)
		{
			return priceManager.GetHouseSellPrice(
				_info.GetSellPriceByLevel(house.level));
		}
	
		public void SellHouse(IBSBHouseBuilding house)
		{
			playerResources.Add(
				GetHouseSellPrice(house));
		}

		public IBSBHouseBuilding GetHouseById(int id)
		{
			return _GetHouseById(id);
		}


		public void AddHouse(BSBHouseBuilding house)
		{
			_housesContainer.Add(house.id, house);
		}

		public void RemoveHouse(BSBHouseBuilding house)
		{
			_housesContainer.Remove(house.id);
		}




		protected BSBHouseBuilding _GetHouseById(int id)
		{
			return _housesContainer[id];
		}

		protected void _ListenHouse(IBSBBuilding house)
		{
			house.onBuildingDestruction += _OnHouseDestruction;
		}

		//
		// < Events >
		//

		protected void _OnHouseDestruction(IBSBBuilding house)
		{
			buildingManager.RemoveBuilding(house);
		}

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
