using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBBarracksBuilding : IBSBBuilding
	{
		int capacity { get; }
	}

	public class BSBBarracksBuilding : BSBBuilding,
		IBSBBarracksBuilding
	{

		public int capacity
		{
			get { return _capacity; }
		}

		[SerializeField]
		protected int _capacity = 0;

		public BSBBarracksBuilding() : base()
		{
			_type = EBSBBuildingType.BARRACKS;
		}

	}

}
