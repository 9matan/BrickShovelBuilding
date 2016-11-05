using UnityEngine;
using System.Collections;
using BSB;
using System;

namespace BSB
{

	public class BSBMapEmptyItem : MonoBehaviour,
		IBSBMapItem
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
