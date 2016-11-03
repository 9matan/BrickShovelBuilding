using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBBuildingInfo
	{
		IBSBReserves GetPriceByLevel(int level);
	}

	[CreateAssetMenu(menuName = "BSB/Building/Info")]
	public class BSBBuildingInfo : ScriptableObject,
		IBSBBuildingInfo
	{

		[SerializeField]
		protected List<BSBPrice> _prices = new List<BSBPrice>();

		public IBSBReserves GetPriceByLevel(int level)
		{
			return new BSBReserves(_prices[level]);
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
