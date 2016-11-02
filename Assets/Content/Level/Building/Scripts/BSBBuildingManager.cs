using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBBuildingManager
	{

	}

	public class BSBBuildingManager : MonoBehaviour,
		IBSBBuildingManager
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
