using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBHouseBuildingInfo : IBSBBuildingInfo
	{
		BSBPrice GetSellPriceByLevel(int level);
	}

	[CreateAssetMenu(menuName = "BSB/Building/House info")]
	public class BSBHouseBuildingInfo : BSBBuildingInfo,
		IBSBHouseBuildingInfo
	{ 

		[System.Serializable]
		public struct HouseLevelInfo
		{
			public BSBPrice sellPrice;
		}

		[Header("House")]
		[SerializeField]
		protected List<HouseLevelInfo> _houseLevels = new List<HouseLevelInfo>();

		public BSBPrice GetSellPriceByLevel(int level)
		{
			return _houseLevels[level - 1].sellPrice;
		}

	}

}
