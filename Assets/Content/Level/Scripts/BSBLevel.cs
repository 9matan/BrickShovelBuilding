using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBLevel : MonoBehaviour 
	{

		[SerializeField]
		protected int _yearsTime;

		public void Initialize()
		{
			BSBDirector.timeManager.onYearEnded += _OnYearEnded;
		}



		protected void _OnYearEnded()
		{
			if (_yearsTime == BSBDirector.timeManager.year)
				BSBDirector.levelCanvas.ShowGameOver();
		}

		public void RestartLevel()
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("level");
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
