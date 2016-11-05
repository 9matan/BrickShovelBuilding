using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{


	public interface IBSBHouseBuildingManager
	{
		int houseCount { get; }

		BSBPrice			GetHouseSellPrice(IBSBHouseBuilding house);
		void				SellHouse(IBSBHouseBuilding house);
		IBSBHouseBuilding	GetHouseById(int id);

		BSBPrice	RepairPrice(IBSBHouseBuilding house);
		void		RepairHouse(IBSBHouseBuilding ihouse);
		bool		TryRepairHouse(IBSBHouseBuilding house);
	}


	public class BSBHouseBuildingManager : MonoBehaviour,
		IBSBHouseBuildingManager
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

		public int houseCount
		{
			get { return _housesContainer.Count; }
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
	
		public void SellHouse(IBSBHouseBuilding ihouse)
		{
			var house = _GetHouseById(ihouse.id);

			house.maxHealth = house.health = _info.GetHealthByLevel(house.level);
			house.SellHouse();

			playerResources.Add(
				GetHouseSellPrice(house));
		}

		public IBSBHouseBuilding GetHouseById(int id)
		{
			return _GetHouseById(id);
		}



		public BSBPrice RepairPrice(IBSBHouseBuilding house)
		{
			var price = priceManager.GetRepairPrice(
				_info.GetRepairPriceByLevel(house.level));

			int dheath = (house.maxHealth - house.health) / 2;
			price.funds *= dheath;
			price.materials *= dheath;

			return price;
		}

		public void RepairHouse(IBSBHouseBuilding ihouse)
		{
			if (!TryRepairHouse(ihouse))
				return;

			var house = _GetHouseById(ihouse.id);
			house.Repair();

			var price = RepairPrice(house);
			playerResources.Use(price);
			playerResources.Restore(price);
		}

		public bool TryRepairHouse(IBSBHouseBuilding house)
		{
			return playerResources.Contains(
				RepairPrice(house));
		}




		public void AddHouse(BSBHouseBuilding house)
		{
			_ListenHouse(house);
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

		public List<IBSBObjectOperation> GetOperations(IBSBBuilding building)
		{
			var list = new List<IBSBObjectOperation>();
			var house = _GetHouseById(building.id);

			list.Add(
				_GetRepairOperation(house));
			list.Add(
				_GetSellOperation(house));

			return list;
		}

		protected BSBObjectOperation _GetRepairOperation(BSBHouseBuilding house)
		{
			return BSBObjectOperation.Create(
				(IBSBObjectOperation oper) =>
				{
					if (oper.IsValid())
					{
						RepairHouse(house);
					}
				},
				BSBObjectOperationInfo.Create(
					_info.repairOperationSprite,
					() => { return RepairPrice(house); }),
				(IBSBObjectOperation oper) => 
				{
					return house.id >= 0 
					&& house.isSoldOut 
					&& !house.isHealthFull 
					&& TryRepairHouse(house)
					&& house.state == EBSBBuildingState.IDLE;
				}
				);
		}

		protected BSBObjectOperation _GetSellOperation(BSBHouseBuilding house)
		{
			return BSBObjectOperation.Create(
				(IBSBObjectOperation oper) =>
				{
					if (oper.IsValid())
					{
						SellHouse(house);
					}
				},
				BSBObjectOperationInfo.Create(
					_info.sellOperationSprite,
					() => { return GetHouseSellPrice(house); }),
				(IBSBObjectOperation oper) =>
				{
					return house.id >= 0 
					&& !house.isSoldOut
					&& house.state == EBSBBuildingState.IDLE;
				}
				);
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
