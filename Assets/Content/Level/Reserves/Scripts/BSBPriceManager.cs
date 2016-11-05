using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBPriceManager
	{
		float inflation { get; }
		float materialsInflation { get; }
		float buildingsInflation { get; }

		BSBPrice GetBuildingPrice(BSBPrice price);
		BSBPrice GetMaterialPrice(BSBPrice price);
		BSBPrice GetWorkerPrice(BSBPrice price);
		BSBPrice GetHouseSellPrice(BSBPrice price);
		BSBPrice GetRepairPrice(BSBPrice price);


		BSBPrice MaterialsIncomeInflation(BSBPrice price);
		BSBPrice Inflation(BSBPrice price);
	}

	public class BSBPriceManager : MonoBehaviour,
		IBSBPriceManager,
		IVOSInitializable
	{


		public void Initialize()
		{
			BSBDirector.timeManager.onMonthEnded += _UpdateInflation;
		}




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

		public BSBPrice GetRepairPrice(BSBPrice price)
		{
			return _Inflation(price);
		}

		public BSBPrice Inflation(BSBPrice price)
		{
			return _Inflation(price);
		}

		public BSBPrice MaterialsIncomeInflation(BSBPrice price)
		{
			price.materials = ((int)((float)price.materials / _materialsInflation));
			return price;
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


		public float inflation
		{
			get { return _inflation; }
		}
		public float materialsInflation
		{
			get { return _materialsInflation; }
		}
		public float buildingsInflation
		{
			get { return _buildingsInflation; }
		}

		[SerializeField]
		protected float _inflation = 1.0f;
		[SerializeField]
		protected float _materialsInflation = 1.0f;
		[SerializeField]
		protected float _buildingsInflation = 1.0f;


		protected void _UpdateInflation()
		{
			_inflation = Random.Range(0.7f, 1.5f);
			_materialsInflation = Random.Range(0.8f, 1.65f);
			_buildingsInflation = Random.Range(0.55f, 1.25f);
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

	[System.Serializable]
	public struct BSBPrice
	{
		public int funds;
		public int workers;
		public int materials;

		public static BSBPrice operator +(BSBPrice p1, BSBPrice p2)
		{
			p1.funds += p2.funds;
			p1.materials += p2.materials;
			p1.workers += p2.workers;

			return p1;
		}
	}

}
