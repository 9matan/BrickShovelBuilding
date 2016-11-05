﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{	

	public interface IBSBBuildingManager :
		IBSBBuildingInfoManager,
		IBSBBuildingManagerEvents
	{
		IBSBBarracksBuildingManager barracksManager { get; }

		IBSBBuilding	BuildBuilding(EBSBBuildingType type);
		bool			TryBuild(EBSBBuildingType type);

		void UpgradeBuilding(IBSBBuilding building);
		bool TryUpgrade(IBSBBuilding building);
	}

	public interface IBSBBuildingManagerEvents
	{
		event Events.OnBuildingAction onBuildingBuild;
		event Events.OnBuildingAction onBuildingBuilt;
		event Events.OnBuildingAction onBuildingUpgrade;
		event Events.OnBuildingAction onBuildingUpgraded;
	}

	public interface IBSBBuildingInfoManager
	{
		int			MaxBuildingLevel(IBSBBuilding building);
		BSBPrice	UpgradePrice(IBSBBuilding building);
		float		UpgradeTime(IBSBBuilding building);
		BSBPrice	BuildPrice(EBSBBuildingType type);
		float		BuildTime(EBSBBuildingType type);
	}

	public class BSBBuildingManager : MonoBehaviour,
		IBSBBuildingManager,
		IVOSInitializable
	{

		public IBSBPlayerResources	playerResources
		{
			get { return BSBDirector.playerResources; }
		}
		public IBSBPriceManager		priceManager
		{
			get { return BSBDirector.priceManager; }
		}

		public IBSBBarracksBuildingManager barracksManager
		{
			get { return _barracksManager; }
		}

		[SerializeField]
		protected BSBBuildingInfoContainer	_infoContainer;
		[SerializeField]
		protected BSBBuildingFactories		_factories;

		[Header("Managers")]
		[SerializeField]
		protected BSBBarracksBuildingManager _barracksManager;

		protected Dictionary<int, BSBBuilding>	_buildings = new Dictionary<int, BSBBuilding>();

		//
		// < Initialize >
		//

		public void Initialize()
		{
			_factories.Initialize();

			_InitializeManagers();
		}

		protected void _InitializeManagers()
		{
			_barracksManager.Initialize();
		}

		//
		// </ Initialize >
		//

		public int MaxBuildingLevel(IBSBBuilding building)
		{
			return _infoContainer[building.type].levelCount;
		}

		//
		// < Upgrade >
		//

		public BSBPrice UpgradePrice(IBSBBuilding building)
		{
			return priceManager.GetBuildingPrice(
				_infoContainer[building.type].
				GetPriceByLevel(building.level + 1));
		}

		public float UpgradeTime(IBSBBuilding building)
		{
			return _infoContainer[building.type].
				GetComplexityByLevel(building.level + 1);
		}

		public void UpgradeBuilding(IBSBBuilding building)
		{
			if (!TryUpgrade(building))
				return;

			_UpgradeByilding(
				_GetBuildingById(building.id));
			_OnBuildingUpgrade(
				_GetBuildingById(building.id));
		}

		public bool TryUpgrade(IBSBBuilding building)
		{
			if (MaxBuildingLevel(building) == building.level)
				return false;

			return playerResources.Contains(
				UpgradePrice(building));
		}



		protected void _UpgradeByilding(BSBBuilding building)
		{
			var price = UpgradePrice(building);
			playerResources.Use(price);
			BuildingActionListener<BSBPrice>.StartTrigger(
				building, price, _RestoreReserves);
			building.Upgrade(
				UpgradeTime(building));
		}

		//
		// </ Upgrade >
		//

		//
		// < Build >
		//

		public BSBPrice BuildPrice(EBSBBuildingType type)
		{
			return priceManager.GetBuildingPrice(
				_infoContainer[type].GetPriceByLevel(1));
		}

		public float BuildTime(EBSBBuildingType type)
		{
			return _infoContainer[type].GetComplexityByLevel(1);
		}

		public IBSBBuilding BuildBuilding(EBSBBuildingType type)
		{
			if (!TryBuild(type))
				return null;

			var building = _CreateBuilding(type);
			building.Initialize();
			_AddBuilding(building);
			_OnBuildingBuild(building);
			_BuildBuilding(building);			
			
			return building;
		}

		public bool TryBuild(EBSBBuildingType type)
		{
			return playerResources.Contains(
				BuildPrice(type));
		}
		


		public IBSBBuilding BuildBuildingImmediatelyFree(EBSBBuildingType type)
		{
			var building = _CreateBuilding(type);
			building.Initialize();
			_AddBuilding(building);
			_OnBuildingBuild(building);
			_BuildBuildingImmediatelyFree(building);

			return building;
		}



		protected void _BuildBuilding(BSBBuilding building)
		{
			var price = BuildPrice(building.type);
			playerResources.Use(price);
			BuildingActionListener<BSBPrice>.StartTrigger(
				building, price, _RestoreReserves);

			building.Build(
				BuildTime(building.type));
		}

		protected void _BuildBuildingImmediatelyFree(BSBBuilding building)
		{			
			building.BuildImmediately();
		}



		protected void _AddBuilding(BSBBuilding building)
		{
			_ListenBuilding(building);
			_buildings.Add(building.id, building);

			switch(building.type)
			{
				case EBSBBuildingType.BARRACKS:
					_barracksManager.AddBarracks((BSBBarracksBuilding)building);
					break;
			}
		}

		protected BSBBuilding _GetBuildingById(int id)
		{
			if (!_buildings.ContainsKey(id))
				return null;
			return _buildings[id];
		}
		
		protected BSBBuilding _CreateBuilding(EBSBBuildingType type)
		{
			var building = _factories.Allocate(type);
			return building;
		}



		protected void _ListenBuilding(IBSBBuilding building)
		{
			building.onBuildingBuilt += _OnIBuildingBuilt;
			building.onBuildingUpgraded += _OnIBuildingUpgraded;
		}

		//
		// </ Build >
		//

		protected void _RestoreReserves(IBuildingActionListener<BSBPrice> listener)
		{
			playerResources.Restore(listener.data);
		}

		//
		// < Events >
		//

		public event Events.OnBuildingAction onBuildingBuild = delegate { };
		public event Events.OnBuildingAction onBuildingBuilt = delegate { };
		public event Events.OnBuildingAction onBuildingUpgrade = delegate { };
		public event Events.OnBuildingAction onBuildingUpgraded = delegate { };

		protected void _OnIBuildingBuilt(IBSBBuilding building)
		{
			_OnBuildingBuilt(_GetBuildingById(building.id));
		}

		protected void _OnIBuildingUpgraded(IBSBBuilding building)
		{
			_OnBuildingUpgraded(_GetBuildingById(building.id));
		}



		protected void _OnBuildingBuild(BSBBuilding building)
		{
			onBuildingBuild(building);
		}

		protected void _OnBuildingBuilt(BSBBuilding building)
		{
			switch (building.type)
			{
				case EBSBBuildingType.BARRACKS:
					_barracksManager.OnBuildingBuilt((BSBBarracksBuilding)building);
					break;
			}

			onBuildingBuilt(building);
		}

		protected void _OnBuildingUpgrade(BSBBuilding building)
		{
			onBuildingUpgrade(building);
		}

		protected void _OnBuildingUpgraded(BSBBuilding building)
		{
			switch (building.type)
			{
				case EBSBBuildingType.BARRACKS:
					_barracksManager.OnBuildingUpgraded((BSBBarracksBuilding)building);
					break;
			}

			onBuildingUpgraded(building);
		}

		//
		// </ Events >
		//

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

		public delegate void OnBuildingActionListener<TData>(IBuildingActionListener<TData> listener);

		public interface IBuildingActionListener<TData>
		{
			IBSBBuilding	building { get; }
			TData			data { get; }
		}

		public class BuildingActionListener<TData> :
			IBuildingActionListener<TData>
		{

			public static BuildingActionListener<TData> StartTrigger(IBSBBuilding building, TData data, OnBuildingActionListener<TData> callBack)
			{
				var listener = new BuildingActionListener<TData>();
				listener.building = building;
				listener.data = data;
				listener.callBack = callBack;
				listener.trigger = true;
				listener.Start();
				return listener;
			}

			public IBSBBuilding building
			{
				get; protected set;
			}
			public TData		data
			{
				get; protected set;
			}
			public bool			trigger
			{
				get; set;
			}

			public OnBuildingActionListener<TData> callBack;

			public void Start()
			{
				building.onBuildingBuilt += _OnActionDone;
				building.onBuildingUpgraded += _OnActionDone;
			}

			protected void _OnActionDone(IBSBBuilding building)
			{
				if(trigger)
					Stop();

				callBack(this);
			}

			public void Stop()
			{ 
				building.onBuildingUpgraded -= _OnActionDone;
				building.onBuildingBuilt -= _OnActionDone;				
			}

		}

	}
	
	[System.Serializable]
	public class BSBBuildingInfoContainer : VOSSerializableDictionary<EBSBBuildingType, BSBBuildingInfo> { }

}