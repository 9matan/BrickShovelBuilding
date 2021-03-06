﻿using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	namespace Events
	{
		
	}

	public interface IBSBHouseBuilding : IBSBBuilding
	{
		bool isSoldOut { get; }
		int maxHealth { get; }
		int health { get; }
		bool isHealthFull { get; }
	}

	public class BSBHouseBuilding : BSBBuilding,
		IBSBHouseBuilding
	{

		public IBSBTimeManager timeManager
		{
			get { return BSBDirector.timeManager; }
		}

		public bool isSoldOut
		{
			get { return _isSoldOut; }
		}
		public int	maxHealth
		{
			get { return _maxHealth; }
			set { _maxHealth = value; }
		}
		public int	health
		{
			get { return _health; }
			set { _health = value; }
		}
		public bool isHealthFull
		{
			get { return health == maxHealth; }
		}

		[SerializeField]
		protected bool _isSoldOut = false;
		[SerializeField]
		protected int _maxHealth;
		[SerializeField]
		protected int _health;

		public BSBHouseBuilding() : base()
		{
			_type = EBSBBuildingType.HOUSE;
		}



		public override void Initialize(IBSBBuildingInfo __info)
		{
			_isSoldOut = false;
			base.Initialize(__info);
		}



		public void SellHouse()
		{
			timeManager.onMonthEnded += _DecreaseHealth;
			_isSoldOut = true;
		}

		public void Repair()
		{
			_health = _maxHealth;
		}

		protected void _DecreaseHealth()
		{
			--_health;
			if (_health == 0)
				DestroyBuilding();
		}



		public override void Clear()
		{
			if(_isSoldOut)
				timeManager.onMonthEnded -= _DecreaseHealth;

			base.Clear();
		}

	}

}
