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

		public static IBSBBarracksBuildingManager barracksManager
		{
			get
			{
				if (buildingManager == null)
					return null;
				return buildingManager.barracksManager;
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



		public static IBSBMap map
		{
			get
			{
				if (_map == null || _map.Equals(null))
					_map = GameObject.FindObjectOfType<BSBMap>();
				return _map;
			}
		}

		private static BSBMap _map;



		public static IBSBCameraController cameraController
		{
			get
			{
				if (_cameraController == null || _map.Equals(null))
					_cameraController = GameObject.FindObjectOfType<BSBCameraController>();
				return _cameraController;
			}
		}

		public static Camera camera
		{
			get
			{
				if (cameraController != null)
					return cameraController.camera;
				return null;
			}
		}

		private static BSBCameraController _cameraController;



		public static IBSBTimeManager timeManager
		{
			get
			{
				if (_timeManager == null || _map.Equals(null))
					_timeManager = GameObject.FindObjectOfType<BSBTimeManager>();
				return _timeManager;
			}
		}

		private static BSBTimeManager _timeManager;

	}

}
