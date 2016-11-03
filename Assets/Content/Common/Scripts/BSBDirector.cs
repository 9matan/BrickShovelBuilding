using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public static class BSBDirector
	{

		public static IBSBPlayerResources playerResources
		{
			get
			{
				if (_playerResources == null || _playerResources.Equals(null))
					_playerResources = GameObject.FindObjectOfType<BSBPlayerResources>();
				return _playerResources;
			}
		}

		private static BSBPlayerResources _playerResources;	


	
		public static IBSBBuildingManager buildingManager
		{			
			get
			{
				if (_buildingManager == null || _buildingManager.Equals(null))
					_buildingManager = GameObject.FindObjectOfType<BSBBuildingManager>();
				return _buildingManager;
			}			
		}

		private static BSBBuildingManager _buildingManager;



		public static IBSBWorkerManager workerManager
		{
			get
			{
				if (_workerManager == null || _workerManager.Equals(null))
					_workerManager = GameObject.FindObjectOfType<BSBWorkerManager>();
				return _workerManager;
			}
		}

		private static BSBWorkerManager _workerManager;



		public static IBSBMaterialsManager materialManager
		{
			get
			{
				if (_materialManager == null || _materialManager.Equals(null))
					_materialManager = GameObject.FindObjectOfType<BSBMaterialsManager>();
				return _materialManager;
			}
		}

		private static BSBMaterialsManager _materialManager;



		public static IBSBPriceManager priceManager
		{
			get
			{
				if (_priceManager == null || _priceManager.Equals(null))
					_priceManager = GameObject.FindObjectOfType<BSBPriceManager>();
				return _priceManager;
			}
		}

		private static BSBPriceManager _priceManager;
	}

}
