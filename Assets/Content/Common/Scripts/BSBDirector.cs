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

	}

}
