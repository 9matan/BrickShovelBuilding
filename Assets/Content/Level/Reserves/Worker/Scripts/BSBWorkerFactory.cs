using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBWorkerFactory : VOSComponentPoolFactory<BSBWorker>
	{	
	


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
