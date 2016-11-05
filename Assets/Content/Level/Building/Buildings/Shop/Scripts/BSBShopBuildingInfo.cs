using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	

	public interface IBSBShopBuildingInfo : IBSBBuildingInfo
	{
		BSBPrice GetIncomeByLevel(int level);
	}

	[CreateAssetMenu(menuName = "BSB/Building/Shop info")]
	public class BSBShopBuildingInfo : BSBBuildingInfo,
		IBSBShopBuildingInfo
	{

		[System.Serializable]
		public class ShopLevelInfo
		{
			public BSBPrice income;
		}

		[SerializeField]
		protected List<ShopLevelInfo> _shopLevels = new List<ShopLevelInfo>();
		
		public BSBPrice GetIncomeByLevel(int level)
		{
			return _shopLevels[level - 1].income;
		}

	}

}
