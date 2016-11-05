using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBBarracksBuildingManager :
		IBSBBarracksBuildingManagerEvents
	{
		int GetBarracksCapacity(IBSBBarracksBuilding barracks);
		int GetTotalBarracksCapacity();
		bool AddWorker(IBSBWorker worker);
	}

	public interface IBSBBarracksBuildingManagerEvents
	{
		event Events.OnBarracksBuildingAction onBuildingBuild;
		event Events.OnBarracksBuildingAction onBuildingBuilt;
		event Events.OnBarracksBuildingAction onBuildingUpgrade;
		event Events.OnBarracksBuildingAction onBuildingUpgraded;
	}

	public class BSBBarracksBuildingManager : MonoBehaviour,
		IBSBBarracksBuildingManager
	{

		public IBSBBuildingManager buildingManager
		{
			get { return BSBDirector.buildingManager; }
		}

		[SerializeField]
		protected BSBBarracksBuildingInfo _barracksInfo;

		protected Dictionary<int, BSBBarracksBuilding> _barracksContainer = new Dictionary<int, BSBBarracksBuilding>();

		//
		// < Initialize >
		//

		public void Initialize()
		{
		}

		//
		// </ Initialize >
		//

		public int GetBarracksCapacity(IBSBBarracksBuilding barracks)
		{
			return _barracksInfo.GetCapacityByLevel(barracks.level);
		}

		public int GetTotalBarracksCapacity()
		{
			int total = 0;

			foreach(var kvp in _barracksContainer)
			{
				total += GetBarracksCapacity(kvp.Value);
			}

			return total;
		}

		public bool AddWorker(IBSBWorker worker)
		{
			bool res = false;

			foreach(var kvp in _barracksContainer)
			{
				if(res = kvp.Value.AddWorker(worker))
				{
					break;
				}
			}

			return res;
		}


		public void AddBarracks(BSBBarracksBuilding barracks)
		{
			_barracksContainer.Add(barracks.id, barracks);
		}

		public void RemoveBarracks(BSBBarracksBuilding barracks)
		{
			_barracksContainer.Remove(barracks.id);
		}

		//
		// < Events >
		//

		public event Events.OnBarracksBuildingAction onBuildingBuild = delegate { };
		public event Events.OnBarracksBuildingAction onBuildingBuilt = delegate { };
		public event Events.OnBarracksBuildingAction onBuildingUpgrade = delegate { };
		public event Events.OnBarracksBuildingAction onBuildingUpgraded = delegate { };

		public void OnBuildingBuild(BSBBarracksBuilding barracks)
		{
			onBuildingBuild(barracks);
		}

		public void OnBuildingBuilt(BSBBarracksBuilding barracks)
		{
			barracks.capacity = barracks.upgradedCapacity = GetBarracksCapacity(barracks);

			onBuildingBuilt(barracks);
		}

		public void OnBuildingUpgrade(BSBBarracksBuilding barracks)
		{
			onBuildingUpgrade(barracks);
		}

		public void OnBuildingUpgraded(BSBBarracksBuilding barracks)
		{
			var capacity = GetBarracksCapacity(barracks);
			barracks.upgradedCapacity = capacity - barracks.capacity;
			barracks.capacity = capacity;

			onBuildingUpgraded(barracks);
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
