using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBPriceManager
	{
		BSBPrice GetBuildingPrice(BSBPrice price);
		BSBPrice GetMaterialPrice(BSBPrice price);
		BSBPrice GetWorkerPrice(BSBPrice price);
	}

	public class BSBPriceManager : MonoBehaviour,
		IBSBPriceManager
	{

		public BSBPrice GetBuildingPrice(BSBPrice price)
		{
			return price;
		}
	
		public BSBPrice GetMaterialPrice(BSBPrice price)
		{
			return price;
		}

		public BSBPrice GetWorkerPrice(BSBPrice price)
		{
			return price;
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
