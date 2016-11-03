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

		[SerializeField]
		protected BSBBuildingInfoContainer _infoContainer;

		//
		// < Upgrade >
		//

		public IBSBReserves UpgradePrice(IBSBBuilding building)
		{
			return null;
		}

		public void UpgradeBuilding(IBSBBuilding building)
		{

		}

		public bool TryBuild(IBSBBuilding building)
		{
			return playerResources.Contains(
				UpgradePrice(building));
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

	
	[System.Serializable]
	public class BSBBuildingInfoContainer : VOSSerializableDictionary<EBSBBuildingType, BSBBuildingInfo> { }


}
