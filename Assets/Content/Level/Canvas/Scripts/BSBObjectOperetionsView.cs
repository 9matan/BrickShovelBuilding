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
		[SerializeField]
		protected Image _hpui;

		protected IBSBMapPlacement _current;

		public bool SetToItem(IBSBMapPlacement placement)
		{
			Reset();
			return _SetToItem(placement);
		}

		protected bool _SetToItem(IBSBMapPlacement placement)
		{
			var operList = _GetOperations(placement.mapItem);
			if (operList == null)
				return false;

			_current = placement;
			transform.position = placement.transform.position;

			for (int i = 0; i < operList.Count; ++i)
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

			if (building.type == EBSBBuildingType.HOUSE)
			{
				var house = BSBDirector.houseManager.GetHouseById(building.id);
				if (house.isSoldOut)
				{
					_hpui.transform.parent.parent.gameObject.Show();
					_hpui.fillAmount = (float)house.health / (float)house.maxHealth;
				}
			}

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

		protected void Update()
		{
			if (_current == null) return;

			_SetToItem(_current);
		}

		public void Reset()
		{
			_current = null;
			_hpui.transform.parent.parent.gameObject.Hide();
			for (int i = 0; i < _viewsList.Count; ++i)
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
