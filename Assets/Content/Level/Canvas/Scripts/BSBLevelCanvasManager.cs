using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBLevelCanvasManager : MonoBehaviour,
		IVOSInitializable
	{

		public IBSBMap map
		{
			get { return BSBDirector.map; }
		}

		[SerializeField]
		protected BSBLevelCanvas _canvas;
		


		public void Initialize()
		{
			_ListenMap(map);
		}

		protected void _ListenMap(IBSBMapEvents events)
		{
			events.onPlacementSelected += _OnMapElementSelected;
			events.onPlacementDeselected += _OnMapElementDeselected;
		}


		
		protected void Update()
		{
			map.selectionOn = !_canvas.isOperationsActive;
		}

		//
		// < Map events >
		//

		protected void _OnMapElementSelected(IBSBMap map)
		{
			_canvas.ActivateOperations(map.activePlacement);
		}

		protected void _OnMapElementDeselected(IBSBMap map)
		{
			_canvas.DeactivateOperations();
		}

		//
		// </ Map events >
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
