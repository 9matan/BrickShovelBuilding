using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBHouseBuildingInfo : IBSBBuildingInfo
	{

	}

	[CreateAssetMenu(menuName = "BSB/Building/House info")]
	public class BSBHouseBuildingInfo : BSBBuildingInfo,
		IBSBHouseBuildingInfo
	{

		

	}

}
