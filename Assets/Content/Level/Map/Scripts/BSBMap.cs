using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	namespace Events
	{
		public delegate void OnMapAction(IBSBMap map);
	}
	

	public interface IBSBMap :
		IBSBMapEvents
	{
		bool selectionOn { get; set; }

		List<IBSBObjectOperation> GetEmptyOperations(IBSBMapEmptyItem empty);

		IBSBMapPlacement	activePlacement { get; }

		IBSBMapPlacement	GetRandomEmptyPlacement();
		IBSBMapPlacement	GetPlacementByWorldPosition(Vector3 worldPosition);
		void				SetMapItem(IBSBMapPlacement iplacement, IBSBMapItem item);
		bool				SetMapItemToRandomEmptyPlacement(IBSBMapItem item);
		void				ClearPlacement(IBSBMapPlacement iplacement);
	}

	public interface IBSBMapEvents
	{
		event Events.OnMapAction onPlacementSelected;
		event Events.OnMapAction onPlacementDeselected;
	}

	public class BSBMap : MonoBehaviour,
		IBSBMap,
		IVOSInitializable
	{

		public IBSBBuildingManager			buildingManager
		{
			get { return BSBDirector.buildingManager; }
		}
		public IBSBHouseBuildingManager		houseManager
		{
			get { return BSBDirector.houseManager; }
		}
		public IBSBShopBuildingManager		shopManager
		{
			get { return BSBDirector.shopManager; }
		}
		public IBSBBarracksBuildingManager	barracksManager
		{
			get { return BSBDirector.barracksManager; }
		}

		public IBSBMapPlacement				activePlacement
		{
			get
			{
				return _activePlacement;
			}
		}

		public new Camera	camera
		{
			get { return BSBDirector.camera; }
		}
		public bool			selectionOn
		{
			get; set;
		}

		[SerializeField]
		protected BSBMapEmptyItemFactory _emptyFactory;
		[SerializeField]
		protected VOSManipulator _manipulator;

		protected IBSBMapPlacement _activePlacement = null;

		protected Dictionary<int, BSBMapPlacement> _placementsContainer = new Dictionary<int, BSBMapPlacement>();


		//
		// < Initialize >
		//

		public void Initialize()
		{
			selectionOn = true;

			_emptyFactory.Initialize();
			_InitializePlacements();
			_ListenManipulator(_manipulator);
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



		protected void _ListenManipulator(IVOSManipulator manipulator)
		{
			manipulator.onPressed += _OnManipulatorReleased;
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
		// < Manipulator >
		//

		protected void _OnManipulatorReleased(IVOSManipulator control)
		{
			if (!selectionOn)
				return;
		
			var position = control.ToWorldPosition(camera);
			var placement = GetPlacementByWorldPosition(position);

			if (placement == null)
			{
				if (_activePlacement != null)
					_OnPlacementDeselected();
				_activePlacement = placement;
			}
			else
			{
				_activePlacement = placement;
				_OnPlacementSelected();
			}
		}

		//
		// </ Manipulator >
		//

		//
		// < Events >
		//

		public event Events.OnMapAction onPlacementSelected = delegate { };
		public event Events.OnMapAction onPlacementDeselected = delegate { };

		protected void _OnPlacementSelected()
		{
			onPlacementSelected(this);
		}

		protected void _OnPlacementDeselected()
		{
			onPlacementDeselected(this);
		}

		//
		// < Events >
		//

		public List<IBSBObjectOperation> GetEmptyOperations(IBSBMapEmptyItem empty)
		{
			var list = new List<IBSBObjectOperation>();

			list.Add(
				_GetBuildOperation(empty, EBSBBuildingType.HOUSE));
			list.Add(
				_GetBuildOperation(empty, EBSBBuildingType.BARRACKS));
			list.Add(
				_GetBuildOperation(empty, EBSBBuildingType.SHOP));

			return list;
		}

		protected BSBObjectOperation _GetBuildOperation(IBSBMapEmptyItem empty, EBSBBuildingType type)
		{
			return BSBObjectOperation.Create(
				(IBSBObjectOperation oper) =>
				{
					if (oper.IsValid())
					{
						var building = buildingManager.BuildBuilding(type);
						var placement = empty.mapPlacement;
						SetMapItem(placement, building);
					}
				},
				buildingManager.GetBuildOperationInfo(type),
				(IBSBObjectOperation oper) =>
				{
					return buildingManager.TryBuild(type);
				}
				);
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
		IBSBMapPlacement mapPlacement { get; set; }
	}

}
