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
		event Events.OnBarracksBuildingAction onBarracksBuild;
		event Events.OnBarracksBuildingAction onBarracksBuilt;
		event Events.OnBarracksBuildingAction onBarracksUpgrade;
		event Events.OnBarracksBuildingAction onBarracksUpgraded;
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

		public event Events.OnBarracksBuildingAction onBarracksBuild = delegate { };
		public event Events.OnBarracksBuildingAction onBarracksBuilt = delegate { };
		public event Events.OnBarracksBuildingAction onBarracksUpgrade = delegate { };
		public event Events.OnBarracksBuildingAction onBarracksUpgraded = delegate { };

		public void OnBarracksBuild(BSBBarracksBuilding barracks)
		{
			onBarracksBuild(barracks);
		}

		public void OnBarracksBuilt(BSBBarracksBuilding barracks)
		{
			barracks.capacity = barracks.upgradedCapacity = GetBarracksCapacity(barracks);

			onBarracksBuilt(barracks);
		}

		public void OnBarracksUpgrade(BSBBarracksBuilding barracks)
		{
			onBarracksUpgrade(barracks);
		}

		public void OnBarracksUpgraded(BSBBarracksBuilding barracks)
		{
			var capacity = GetBarracksCapacity(barracks);
			barracks.upgradedCapacity = capacity - barracks.capacity;
			barracks.capacity = capacity;

			onBarracksUpgraded(barracks);
		}

		//
		// </ Events >
		//

		public List<IBSBObjectOperation> GetOperations(IBSBBuilding building)
		{
			var list = new List<IBSBObjectOperation>();	
			return list;
		}

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
