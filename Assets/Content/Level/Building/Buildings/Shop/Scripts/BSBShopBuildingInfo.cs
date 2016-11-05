using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBShopBuildingInfo : IBSBBuildingInfo
	{

	}

	[CreateAssetMenu(menuName = "BSB/Building/Shop info")]
	public class BSBShopBuildingInfo : BSBBuildingInfo,
		IBSBShopBuildingInfo
	{

		
	}

}
