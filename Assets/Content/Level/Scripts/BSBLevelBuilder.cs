using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBLevelBuilder : MonoBehaviour 
	{

		public const int INFINITY_CAPACITY = 10000000;

		public IBSBTrashManager		trashManager
		{
			get { return BSBDirector.trashManager; }
		}
		public BSBPlayerResources	playerResources
		{
			get { return _playerResources; }
		}
		public IBSBMap				map
		{
			get { return BSBDirector.map; }
		}

		[SerializeField]
		protected BSBBuildingManager _buildingManager;
		[SerializeField]
		protected BSBWorkerManager _workerManager;
		[SerializeField]
		protected BSBPlayerResources _playerResources;
		[SerializeField]
		protected BSBPrice _startReserves;
		[SerializeField]
		protected BSBPrice _startIncome;

		protected BSBLevel _level;	

		public void Build(BSBLevel __level)
		{
			_level = __level;

			_BuildBuildings();
			_BuildTrashes();
			_BuildReserves();			

			_Test();
		}
	
		protected void _BuildReserves()
		{
			playerResources.fundsStorage.Extend(INFINITY_CAPACITY);
			playerResources.materialsStorage.Extend(INFINITY_CAPACITY);

			for (int i = 0; i < _startReserves.workers; ++i)
				_workerManager.HireOneWorkerFree();

			_startReserves.workers = 0;

			playerResources.Add(_startReserves);

			playerResources.income += _startIncome;
		}

		protected void _BuildBuildings()
		{
			var barrack = _buildingManager.BuildBuildingImmediatelyFree(EBSBBuildingType.BARRACKS);
			map.SetMapItemToRandomEmptyPlacement(barrack);
		}

		[SerializeField]
		protected int _trashNumber = 2;

		protected void _BuildTrashes()
		{
			for (int i = 0; i < _trashNumber; ++i)
			{
				map.SetMapItemToRandomEmptyPlacement(
					trashManager.CreateTrash());
			}
		}
	
		protected void _Test()
		{
	//		_buildingManager.BuildBuilding(EBSBBuildingType.HOUSE);
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
