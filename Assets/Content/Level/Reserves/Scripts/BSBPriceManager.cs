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
		BSBPrice GetHouseSellPrice(BSBPrice price);
	}

	public class BSBPriceManager : MonoBehaviour,
		IBSBPriceManager
	{

		public BSBPrice GetBuildingPrice(BSBPrice price)
		{
			return _BuildingsInflation(
				_Inflation(price));
		}
	
		public BSBPrice GetMaterialPrice(BSBPrice price)
		{
			return _MaterialsInflation(
				_Inflation(price));
		}

		public BSBPrice GetWorkerPrice(BSBPrice price)
		{
			return _Inflation(price);
		}

		public BSBPrice GetHouseSellPrice(BSBPrice price)
		{
			return GetBuildingPrice(price);
		}

		protected BSBPrice _Inflation(BSBPrice price)
		{
			price.funds = (int)(_inflation * (float)price.funds);
			return price;
		}

		protected BSBPrice _MaterialsInflation(BSBPrice price)
		{
			price.funds = (int)(_materialsInflation * (float)price.funds);
			return price;
		}

		protected BSBPrice _BuildingsInflation(BSBPrice price)
		{
			price.funds = (int)(_buildingsInflation * (float)price.funds);
			return price;
		}

		[SerializeField]
		protected float _inflation = 1.0f;
		[SerializeField]
		protected float _materialsInflation = 1.0f;
		[SerializeField]
		protected float _buildingsInflation = 1.0f;

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

	[System.Serializable]
	public struct BSBPrice
	{
		public int funds;
		public int workers;
		public int materials;
	}

}
