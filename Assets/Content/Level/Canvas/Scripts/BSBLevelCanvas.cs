using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBLevelCanvas
	{
		Canvas canvas { get; }
		void ShowGameOver();
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
		[SerializeField]
		protected GameObject _gameOver;

		protected Canvas _canvas;


		public void Initialize()
		{
			
		}



		public void ActivateOperations(IBSBMapPlacement placement)
		{
			isOperationsActive = true;
			_operations.gameObject.Show();
			if (!_operations.SetToItem(placement))
				DeactivateOperations();
		}
		
		public void DeactivateOperations()
		{
			if (!isOperationsActive) return;

			_operations.Reset();
			_operations.gameObject.Hide();
			isOperationsActive = false;
		}


		public void ShowGameOver()
		{
			_gameOver.Show();
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
