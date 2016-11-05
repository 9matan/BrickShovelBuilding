using UnityEngine;
using System.Collections.Generic;

using BSB;

namespace BSB
{

	namespace Events
	{
		public delegate void OnBarracksBuildingAction(IBSBBarracksBuilding barracks);
	}

	public interface IBSBBarracksBuilding : IBSBBuilding
	{
		int capacity { get; }
		int freeCapacity { get; }
		int upgradedCapacity { get; }
	}

	public class BSBBarracksBuilding : BSBBuilding,
		IBSBBarracksBuilding
	{

		public int upgradedCapacity
		{
			get; set;
		}

		public int capacity
		{
			get { return _capacity; }
			set { _capacity = value; }
		}

		public int freeCapacity
		{
			get { return _capacity - _workers.Count; }
		}

		[SerializeField]
		protected int _capacity = 0;

		protected List<IBSBWorker> _workers = new List<IBSBWorker>();

		public BSBBarracksBuilding() : base()
		{
			_type = EBSBBuildingType.BARRACKS;
		}

		public bool AddWorker(IBSBWorker worker)
		{
			if (freeCapacity == 0)
				return false;

			_workers.Add(worker);
			return true;
		}

	}

}
