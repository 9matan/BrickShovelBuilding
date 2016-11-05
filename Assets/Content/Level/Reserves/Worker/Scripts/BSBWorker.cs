using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBWorker
	{
		int id { get; }

		string workerName { get; }
	}

	public class BSBWorker : MonoBehaviour,
		IBSBWorker
	{

		protected static int _ID = 0;

		public int id
		{
			get { return _id; }
		}
		public string workerName
		{
			get { return _workerName; }
			set { _workerName = value; }
		}

		[SerializeField]
		protected string _workerName = "Ivan";

		protected int _id;

		//
		// < Initialize >
		//

		public void Initialize()
		{
			_id = ++_ID;
		}

		//
		// </ Initialize >
		//


		//
		// < Clear >
		//

		public void Clear()
		{

		}

		//
		// </ Clear >
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
