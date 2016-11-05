using UnityEngine;
using System.Collections;
using BSB;
using System;

namespace BSB
{

	public interface IBSBMapEmptyItem :
		IBSBMapItem
	{

	}

	public class BSBMapEmptyItem : MonoBehaviour,
		IBSBMapEmptyItem
	{

		public int id
		{
			get { return 0; }
		}

		public IBSBMapPlacement mapPlacement
		{
			get; set;
		}
		public EBSBMapItemType	mapItemType
		{
			get { return EBSBMapItemType.EMPTY; }
		}
		
	}

}
