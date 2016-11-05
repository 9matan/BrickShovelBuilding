using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	namespace Events
	{
		public delegate void OnShopBuildingAction(IBSBShopBuilding shop);
	}

	public interface IBSBShopBuilding : IBSBBuilding
	{

	}

	public class BSBShopBuilding : BSBBuilding,
		IBSBShopBuilding
	{

		public BSBShopBuilding() : base()
		{
			_type = EBSBBuildingType.SHOP;
		}

	}

}
