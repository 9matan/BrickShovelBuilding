using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBTrash :
		IBSBMapItem
	{

	}

	public class BSBTrash : MonoBehaviour,
		IBSBTrash
	{

		protected static int _ID = 0;

		public int				id
		{
			get { return _id; }
		}
		public EBSBMapItemType	mapItemType
		{
			get { return EBSBMapItemType.TRASH; }
		}
		public IBSBMapPlacement mapPlacement
		{
			get; set;
		}

		[SerializeField]
		protected int _id;

		protected IBSBTrashInfo _info;

		//
		// < Intialize >
		//

		public void Initialize(IBSBTrashInfo __info)
		{
			_id = _ID++;
			_info = __info;
		}

		//
		// < Intialize >
		//

		public void Clear()
		{

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
