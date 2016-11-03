using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBWorkerManager
	{
		BSBPrice workerPrice { get; }
	}

	public class BSBWorkerManager : MonoBehaviour,
		IBSBWorkerManager
	{

		public BSBPrice workerPrice
		{
			get { return _workerPrice; }
		}

		[Header("Price")]
		[SerializeField]
		protected BSBPrice _workerPrice;
		[SerializeField]
		protected int _purchasingQuantity = 1;

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
