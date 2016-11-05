using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBLevelUIGameOver : MonoBehaviour 
	{

		[SerializeField]
		protected Text _scoreui;



		protected void OnEnable()
		{
			_scoreui.text = "Your score: ";

			var pr = BSBDirector.playerResources;
			int score = pr.funds + pr.workers * 50 + pr.materials * 4;

			_scoreui.text += score.ToString(); 
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
