using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBTrashInfo
	{
		BSBPrice GetCleaningPrice();
	}

	[CreateAssetMenu(menuName = "BSB/Trash/Info")]
	public class BSBTrashInfo : ScriptableObject,
		 IBSBTrashInfo
	{

		[SerializeField]
		protected BSBPrice _cleaningPrice;
	
		public BSBPrice GetCleaningPrice()
		{
			return _cleaningPrice;
		}

		
	}

}
