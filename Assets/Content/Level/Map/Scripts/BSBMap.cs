using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBMap
	{
		IBSBMapPlacement	GetRandomEmptyPlacement();
		IBSBMapPlacement	GetPlacementByWorldPosition(Vector3 worldPosition);
		void				SetMapItem(IBSBMapPlacement iplacement, IBSBMapItem item);
		bool SetMapItemToRandomEmptyPlacement(IBSBMapItem item);
		void				ClearPlacement(IBSBMapPlacement iplacement);
	}

	public class BSBMap : MonoBehaviour,
		IBSBMap,
		IVOSInitializable
	{

		[SerializeField]
		protected BSBMapEmptyItemFactory _emptyFactory;

		protected Dictionary<int, BSBMapPlacement> _placementsContainer = new Dictionary<int, BSBMapPlacement>();


		//
		// < Initialize >
		//

		public void Initialize()
		{
			_emptyFactory.Initialize();
			_InitializePlacements();
		}

		protected void _InitializePlacements()
		{
			var _placements = gameObject.GetComponentsInChildren<BSBMapPlacement>(true);

			for (int i = 0; i < _placements.Length; ++i)
			{
				_placements[i].Initialize();
				_AddPlacement(_placements[i]);
				_ClearPlacement(_placements[i]);
			}
		}



		protected void _AddPlacement(BSBMapPlacement placement)
		{
			_placementsContainer.Add(placement.id, placement);
		}

		//
		// </ Initialize >
		//

		public IBSBMapPlacement GetRandomEmptyPlacement()
		{
			List<IBSBMapPlacement> empty = new List<IBSBMapPlacement>();

			foreach (var kvp in _placementsContainer)
				if (kvp.Value.isEmpty)
					empty.Add(kvp.Value);

			if (empty.Count == 0)
				return null;
			return empty[Random.Range(0, empty.Count - 1)];
		}

		public IBSBMapPlacement GetPlacementByWorldPosition(Vector3 worldPosition)
		{
			IBSBMapPlacement placement = null;

			foreach (var kvp in _placementsContainer)
			{
				if (kvp.Value.IsHere(worldPosition))
				{
					placement = kvp.Value;
					break;
				}
			}

			return placement;
		}

		public void SetMapItem(IBSBMapPlacement iplacement, IBSBMapItem item)
		{
			var placement = _GetPlacementById(iplacement.id);
			if (placement.mapItem.mapItemType == EBSBMapItemType.EMPTY)
				_FreeEmptyItem((BSBMapEmptyItem)placement.mapItem);
			placement.SetMapItem(item);
		}

		public bool SetMapItemToRandomEmptyPlacement(IBSBMapItem item)
		{
			var placement = GetRandomEmptyPlacement();
			if (placement == null)
				return false;

			SetMapItem(placement, item);

			return true;
		}

		public void ClearPlacement(IBSBMapPlacement iplacement)
		{
			_ClearPlacement(
				_GetPlacementById(iplacement.id));	
		}



		protected void _ClearPlacement(BSBMapPlacement placement)
		{
			if (placement.mapItem == null || placement.mapItem.mapItemType != EBSBMapItemType.EMPTY)
				placement.SetMapItem(_CreateEmptyItem());
		}

		protected BSBMapPlacement _GetPlacementById(int id)
		{
			return _placementsContainer[id];
		}




		protected BSBMapEmptyItem _CreateEmptyItem()
		{
			return _emptyFactory.Allocate();
		}

		protected void _FreeEmptyItem(BSBMapEmptyItem item)
		{
			_emptyFactory.Free(item);
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


	public enum EBSBMapItemType
	{
		EMPTY,
		BUILDING,
		TRASH
	}

	public interface IBSBMapItem :
		IVOSTransformable
	{
		int id { get; }
		EBSBMapItemType mapItemType { get; }
	}

}
