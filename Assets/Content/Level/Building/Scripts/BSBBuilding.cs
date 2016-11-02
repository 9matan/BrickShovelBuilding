using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBBuilding
	{

	}

	public enum EBSBBuildingType
	{
		NONE,
		BARRACK,
		HOUSE,
		SHOP
	}

	public enum EBSBBuildingState
	{
		CONSTRUCTION,
		UPGRADE,
		IDLE
	}

	public class BSBBuilding : MonoBehaviour 
	{

		public EBSBBuildingType		type
		{
			get { return _type; }
		}
		public int					level
		{
			get { return _level; }
		}
		public EBSBBuildingState	state
		{
			get { return _state; }
		}

		[SerializeField]
		protected EBSBBuildingType _type;
		[SerializeField]
		protected int _level;
		[SerializeField]
		protected EBSBBuildingState _state;






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
