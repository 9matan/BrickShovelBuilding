using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBCameraController
	{
		Camera camera { get; }
	}

	public class BSBCameraController : MonoBehaviour,
		IBSBCameraController
	{

		public new Camera camera
		{
			get { return _camera; }
		}

		[SerializeField]
		protected Camera _camera;
		[SerializeField]
		protected Vector2 _cameraArea = new Vector2(15.0f, 5.0f);
	
	
	
	
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

#if UNITY_EDITOR

		protected void OnDrawGizmos()
		{
			GizmosDrawArea();
		}

		public void GizmosDrawArea()
		{
			Gizmos.DrawWireCube(transform.position, _cameraArea);
		}

#endif

	}

}
