using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBBuildingFace : MonoBehaviour 
	{

		public IBSBBuilding building
		{
			get; protected set;
		}

		public Sprite sprite
		{
			get { return _spriter.sprite; }
			protected set { _spriter.sprite = value; }
		}

		[SerializeField]
		protected SpriteRenderer _spriter;

		//
		// < Initialize >
		//

		public void Initialize(IBSBBuilding __building)
		{
			building = __building;
			_lastSprite = -1;
			_lastBuildingState = EBSBBuildingState.NONE;
		}

		//
		// < Initialize >
		//


		protected void Update()
		{
			_UpdateSprite();
		}

		protected EBSBBuildingState _lastBuildingState;
		protected int				_lastSprite = -1;

		protected void _UpdateSprite()
		{
			if (building == null) return;

			if (building.state == EBSBBuildingState.IDLE)
			{
				if (_lastBuildingState != building.state)
				{
					sprite = building.info.GetSpriteByLevel(building.level);
				}
			}
			else
			{
				var index = _GetBuildingSpriteIndex();

				if (_lastBuildingState != building.state || _lastSprite != index)
				{
					sprite = building.info.GetUpgradingSpriteByLevel(building.level, index);					
				}

				_lastSprite = index;
			}
			
			_lastBuildingState = building.state;
		}

		protected int _GetBuildingSpriteIndex()
		{
			float step = (1.0f / (float)building.info.GetLevelUpgradingSpritesCount(building.level)) + 0.001f;
			float time = (building.currentActionTime / building.actionTime);
			return (int)(time / step);
		}

		//
		// < Clear >
		//

		public void Clear()
		{
			building = null;
			
		}

		//
		// </ Clear >
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
