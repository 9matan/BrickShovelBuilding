using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBLevelBuilder : MonoBehaviour 
	{

		public IBSBPlayerResources playerResources
		{
			get { return BSBDirector.playerResources; }
		}
		public IBSBBuildingManager buildingManager
		{
			get { return BSBDirector.buildingManager; }
		}

		protected BSBLevel _level;

		[SerializeField]
		protected BSBPrice _startReserves;

		public void Build(BSBLevel __level)
		{
			_level = __level;

			_BuildReserves();

			_Test();
		}
	
		protected void _BuildReserves()
		{
			playerResources.fundsStorage.Extend(100000000);
			playerResources.materialsStorage.Extend(100000000);
			playerResources.workersStorage.Extend(_startReserves.workers);

			playerResources.Add(_startReserves);
		}
	
	
		protected void _Test()
		{
			buildingManager.BuildBuilding(EBSBBuildingType.HOUSE);
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
