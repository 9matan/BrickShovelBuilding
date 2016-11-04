using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBMaterialsManager
	{
		BSBPrice materialPrice { get; }

		void BuyMaterials();
		bool TryBuyMaterials();
	}

	public class BSBMaterialsManager : MonoBehaviour,
		IBSBMaterialsManager,
		IVOSInitializable
	{

		public IBSBPlayerResources	playerResources
		{
			get { return BSBDirector.playerResources; }
		}
		public IBSBPriceManager		priceManager
		{
			get { return BSBDirector.priceManager; }
		}
		
		public BSBPrice materialPrice
		{
			get { return priceManager.GetMaterialPrice(_materialPrice); }
		}

		[Header("Price")]
		[SerializeField]
		protected BSBPrice _materialPrice;
		[SerializeField]
		protected int _purchasingQuantity;

		//
		// < Initialize >
		//

		public void Initialize()
		{

		}

		//
		// </ Initialize >
		//

		public void BuyMaterials()
		{
			if (!TryBuyMaterials())
				return;

			playerResources.Use(materialPrice);
			var reserves = new BSBPrice();
			reserves.materials = _purchasingQuantity;
			playerResources.Add(reserves);
		}

		public bool TryBuyMaterials()
		{
			return playerResources.Contains(materialPrice);
		}


		//
		// < Clear >
		//

		public void Clear()
		{

		}

		//
		// </ Clear >
		//


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
