using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{
	
	[RequireComponent(typeof(BSBPlayerResources))]
	public class BSBPlayerResourcesInspectorView : MonoBehaviour 
	{

#if UNITY_EDITOR

		[SerializeField]
		protected int _funds;
		[SerializeField]
		protected int _materials;

		[SerializeField]
		protected int _workers;
		[SerializeField]
		protected int _freeWorkers;
		[SerializeField]
		protected int _workersCapacity;


		protected BSBPlayerResources _resources;

		protected void Awake()
		{
			_resources = GetComponent<BSBPlayerResources>();
		}


		protected void Update()
		{
			_UpdateReservesView();
		}

		protected void _UpdateReservesView()
		{
			_funds = _resources.funds;
			_materials = _resources.materials;

			_freeWorkers = _resources.workers;
			_workersCapacity = _resources.workersCapacity;
			_workers = _resources.workersStorage.maxAmount;
		}

#endif

	}

}
