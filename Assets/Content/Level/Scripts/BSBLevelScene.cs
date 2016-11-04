using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public class BSBLevelScene : MonoBehaviour 
	{

		[SerializeField]
		protected BSBLevel _level;
		[SerializeField]
		protected BSBLevelBuilder _levelBuilder;

		protected void Start()
		{
			Initialize();
		}

		public void Initialize()
		{
			_level.Initialize();
			_levelBuilder.Build(_level);
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
