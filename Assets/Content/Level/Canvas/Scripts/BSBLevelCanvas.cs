using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBLevelCanvas
	{
		Canvas canvas { get; }
	}

	[RequireComponent(typeof(Canvas))]
	public class BSBLevelCanvas : MonoBehaviour,
		IBSBLevelCanvas,
		IVOSInitializable
	{

		public Canvas canvas
		{
			get
			{
				if (_canvas == null)
					_canvas = GetComponent<Canvas>();
				return _canvas;
			}
		}

		protected Canvas _canvas;


		public void Initialize()
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
