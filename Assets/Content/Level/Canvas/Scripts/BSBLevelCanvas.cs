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

		public bool isOperationsActive
		{
			get; protected set;
		}

		[SerializeField]
		protected BSBObjectOperetionsView _operations;

		protected Canvas _canvas;


		public void Initialize()
		{

		}



		public void ActivateOperations(IBSBMapPlacement placement)
		{
			isOperationsActive = true;
			_operations.gameObject.Show();
			_operations.SetToItem(placement);
		}
		
		public void DeactivateOperations()
		{
			_operations.Reset();
			_operations.gameObject.Hide();
			isOperationsActive = false;
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
