using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBWorkerFactory : VOSComponentPoolFactory<BSBWorker>
	{

		[SerializeField]
		protected string[] _workersName;

		public override BSBWorker Allocate()
		{
			var worker = base.Allocate();
			worker.workerName = _GetRandomName();
			return worker;
		}

		protected string _GetRandomName()
		{
			return _workersName[Random.Range(0, _workersName.Length)];
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
