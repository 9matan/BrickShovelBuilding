using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace BSB
{

	[CustomPropertyDrawer(typeof(BSBBuildingInfoContainer))]
	public class BSBPricesContainerDrawer : VOSDictionaryDrawer<EBSBBuildingType, BSBBuildingInfo>
	{
	}

}