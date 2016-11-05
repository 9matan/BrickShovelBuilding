using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBMapPlacement :
		IVOSTransformable
	{
		bool		isEmpty { get; }
		int			id { get; }
		IBSBMapItem mapItem { get; }

		bool IsHere(Vector3 worldPosition);
	}

	public class BSBMapPlacement : MonoBehaviour,
		IBSBMapPlacement
	{

		protected static int _ID = 0;

		public bool			isEmpty
		{
			get
			{
				if (_mapItem == null)
					return true;
				else
					return _mapItem.mapItemType == EBSBMapItemType.EMPTY;
			}
		}
		public int			id
		{
			get { return _id; }
		}
		public IBSBMapItem	mapItem
		{
			get { return _mapItem; }
		}

		[SerializeField]
		protected Vector2 _triggerArea = new Vector2(1.0f, 1.0f);
//		[SerializeField]
//		protected SpriteRenderer _spriter;
		[SerializeField]
		protected int _id;

		protected IBSBMapItem	_mapItem;
		

		public void Initialize()
		{
			_id = _ID++;
		}

		public bool IsHere(Vector3 worldPosition)
		{
			return worldPosition.x >= transform.position.x - _triggerArea.x
				&& worldPosition.x <= transform.position.x + _triggerArea.x
				&& worldPosition.y >= transform.position.y - _triggerArea.y
				&& worldPosition.y <= transform.position.y + _triggerArea.y;
		}

		public void SetMapItem(IBSBMapItem __mapItem)
		{
			_mapItem = __mapItem;

			_mapItem.transform.SetParent(transform);
			_mapItem.transform.localPosition = Vector3.zero;
			_mapItem.mapPlacement = this;
		}

		//
		// < Log >
		//

		public bool debug = false;

		public void Log(object msg)
		{
			if (debug)
				Debug.Log(msg);
		}



		//
		// </ Log >
		//

#if UNITY_EDITOR

		protected void OnDrawGizmos()
		{
			GizmosDrawTrigger();
		}

		public void GizmosDrawTrigger()
		{
			Gizmos.DrawWireCube(transform.position, _triggerArea);
		}

#endif

	}


	

}
