using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBMaterialsManager
	{
		BSBPrice materialPrice { get; }
	}

	public class BSBMaterialsManager : MonoBehaviour,
		IBSBMaterialsManager
	{

		public BSBPrice materialPrice
		{
			get { return _materialPrice; }
		}



		[Header("Price")]
		[SerializeField]
		protected BSBPrice _materialPrice;
		[SerializeField]
		protected int _purchasingQuantity;

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
