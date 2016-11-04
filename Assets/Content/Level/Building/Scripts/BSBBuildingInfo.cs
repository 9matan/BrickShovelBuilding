using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBBuildingInfo
	{
		int levelCount { get; }

		BSBPrice	GetPriceByLevel(int level);
		float		GetComplexityByLevel(int level);
	}

	[CreateAssetMenu(menuName = "BSB/Building/Info")]
	public class BSBBuildingInfo : ScriptableObject,
		IBSBBuildingInfo
	{

		[System.Serializable]
		public struct LevelInfo
		{
			public BSBPrice price;
			public float complexity;
		}

		public int levelCount
		{
			get { return _levels.Count; }
		}

		[SerializeField]
		protected List<LevelInfo> _levels = new List<LevelInfo>();

		public BSBPrice GetPriceByLevel(int level)
		{
			return _levels[level].price;
		}

		public float GetComplexityByLevel(int level)
		{
			return _levels[level].complexity;
		}
		
	}

}
