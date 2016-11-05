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

		Sprite		GetSpriteByLevel(int level);
		Sprite		GetUpgradingSpriteByLevel(int level, int index);
		int			GetLevelUpgradingSpritesCount(int level);
	}

	[CreateAssetMenu(menuName = "BSB/Building/Info")]
	public class BSBBuildingInfo : ScriptableObject,
		IBSBBuildingInfo
	{

		[System.Serializable]
		public struct LevelInfo
		{
			public BSBPrice price;
			public float	complexity;
			public Sprite[] upgradingSprites;
			public Sprite	sprite;
		}

		public int levelCount
		{
			get { return _levels.Count; }
		}

		[SerializeField]
		protected List<LevelInfo> _levels = new List<LevelInfo>();

		public BSBPrice GetPriceByLevel(int level)
		{
			return _levels[level - 1].price;
		}

		public float GetComplexityByLevel(int level)
		{
			return _levels[level - 1].complexity;
		}

		public Sprite GetUpgradingSpriteByLevel(int level, int index)
		{
			if (GetLevelUpgradingSpritesCount(level) == 0)
				return GetSpriteByLevel(level);

			return _levels[level].upgradingSprites[index];
		}
		
		public int GetLevelUpgradingSpritesCount(int level)
		{
			return Mathf.Max(_levels[level].upgradingSprites.Length, 1);
		}

		public Sprite GetSpriteByLevel(int level)
		{
			return _levels[level - 1].sprite;
		}

	}

}
