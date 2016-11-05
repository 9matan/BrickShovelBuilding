using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBBarracksBuildingInfo : IBSBBuildingInfo
	{

	}

	[CreateAssetMenu(menuName = "BSB/Building/Barracks info")]
	public class BSBBarracksBuildingInfo : BSBBuildingInfo,
		IBSBBarracksBuildingInfo
	{
				
		[System.Serializable]
		public struct BarracksLevelInfo
		{
			public int capacity;
		}


		[Header("Barracks")]
		[SerializeField]
		protected List<BarracksLevelInfo> _brracksLevels = new List<BarracksLevelInfo>();

		public int GetCapacityByLevel(int level)
		{
			level = Mathf.Min(level - 1, _brracksLevels.Count - 1);

			return _brracksLevels[level].capacity;
		}



	}

}
