using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public class BSBObjectOperetionsView : MonoBehaviour 
	{

		[SerializeField]
		protected List<BSBObjectOperetionView> _viewsList = new List<BSBObjectOperetionView>();

		public bool SetToItem(IBSBMapPlacement placement)
		{
			var operList = _GetOperations(placement.mapItem);
			if (operList == null)
				return false;

			var pos = placement.transform.position;
			pos.y += 1.0f;
			transform.position = pos;

			
			for(int i = 0; i < operList.Count; ++i)
			{
				_viewsList[i].gameObject.Show();
				_viewsList[i].SetOperation(operList[i]);
			}

			return true;
		}

		//
		// < SetCurrentItem >
		//

		protected List<IBSBObjectOperation> _GetOperations(IBSBMapItem item)
		{
			switch (item.mapItemType)
			{
				case EBSBMapItemType.BUILDING:
					return _GetBuildingOperations((IBSBBuilding)item);
				case EBSBMapItemType.TRASH:
					return _GetTrashOperations((IBSBTrash)item);
				case EBSBMapItemType.EMPTY:
					return _GetEmptyOperations((IBSBMapEmptyItem)item);
			}

			return null;
		}

		protected List<IBSBObjectOperation> _GetBuildingOperations(IBSBBuilding building)
		{
			if (building.state != EBSBBuildingState.IDLE) return null;

			return BSBDirector.buildingManager.GetOperations(building);
		}

		protected List<IBSBObjectOperation> _GetTrashOperations(IBSBTrash trash)
		{
			return BSBDirector.trashManager.GetOperations(trash);
		}

		protected List<IBSBObjectOperation> _GetEmptyOperations(IBSBMapEmptyItem empty)
		{
			return BSBDirector.map.GetEmptyOperations(empty);
		}

		//
		// </ SetCurrentItem >
		//

		public void Reset()
		{
			for(int i = 0; i < _viewsList.Count; ++i)
			{
				_viewsList[i].Reset();
				_viewsList[i].gameObject.Hide();
			}
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
