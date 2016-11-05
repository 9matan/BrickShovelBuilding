using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBTrashInfo
	{
		Sprite cleaningOperationSprite { get; }
		BSBPrice GetCleaningPrice();
	}

	[CreateAssetMenu(menuName = "BSB/Trash/Info")]
	public class BSBTrashInfo : ScriptableObject,
		 IBSBTrashInfo
	{

		public Sprite cleaningOperationSprite
		{
			get { return _cleaningOperationSprite; }
		}

		[SerializeField]
		protected Sprite _cleaningOperationSprite;
		[SerializeField]
		protected BSBPrice _cleaningPrice;
	
		public BSBPrice GetCleaningPrice()
		{
			return _cleaningPrice;
		}

		
	}

}
