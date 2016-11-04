using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBBarracksBuildingManager
	{
	}

	public class BSBBarracksBuildingManager : MonoBehaviour,
		IBSBBarracksBuildingManager,
		IVOSInitializable
	{

		protected Dictionary<int, BSBBarracksBuilding> _barracksContainer = new Dictionary<int, BSBBarracksBuilding>();	
	
		public void Initialize()
		{

		}

		
		public void AddBarracks(BSBBarracksBuilding barracks)
		{
			_ListenBarracks(barracks);
			_barracksContainer.Add(barracks.id, barracks);
		}

		protected void _ListenBarracks(IBSBBarracksBuilding barracks)
		{
			barracks.onBuildingBuilt += _OnBuildingBuilt;
			barracks.onBuildingUpgraded += _OnBuildingUpgraded;
		}

		//
		// < Events >
		//

		public event Events.OnBuildingAction onBuildingBuild = delegate { };
		public event Events.OnBuildingAction onBuildingBuilt = delegate { };
		public event Events.OnBuildingAction onBuildingUpgrade = delegate { };
		public event Events.OnBuildingAction onBuildingUpgraded = delegate { };

		protected void _OnBuildingBuild(IBSBBuilding building)
		{
			onBuildingBuild(building);
		}

		protected void _OnBuildingBuilt(IBSBBuilding building)
		{
			onBuildingBuilt(building);
		}

		protected void _OnBuildingUpgrade(IBSBBuilding building)
		{
			onBuildingUpgrade(building);
		}

		protected void _OnBuildingUpgraded(IBSBBuilding building)
		{
			onBuildingUpgraded(building);
		}

		//
		// </ Events >
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
