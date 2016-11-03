using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBBuildingManager
	{

	}

	public class BSBBuildingManager : MonoBehaviour,
		IBSBBuildingManager
	{

		public IBSBPlayerResources playerResources
		{
			get { return BSBDirector.playerResources; }
		}


		//
		// < Upgrade >
		//

		public IBSBReserves UpgradePrice(IBSBBuilding build)
		{
			return null;
		}

		public void UpgradeBuilding(IBSBBuilding build)
		{

		}

		//
		// </ Upgrade >
		//

		//
		// < Build >
		//

		public IBSBReserves BuildPrice(EBSBBuildingType type)
		{
			return null;
		}

		public IBSBBuilding BuildBuilding(EBSBBuildingType type)
		{
			return null;
		}

		public bool TryBuild(EBSBBuildingType type)
		{
			return playerResources.Contains(
				BuildPrice(type));
		}

		//
		// </ Build >
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
