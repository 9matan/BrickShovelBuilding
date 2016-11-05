using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBCameraController
	{
		Camera camera { get; }

		float worldTopBound { get; }
		float worldBottomBound { get; }
		float worldLeftBound { get; }
		float worldRightBound { get; }
	}

	public class BSBCameraController : MonoBehaviour,
		IBSBCameraController
	{

		public new Camera camera
		{
			get { return _camera; }
		}

		public float worldTopBound
		{
			get
			{
				return transform.position.y + (realCameraArea.y - _cameraArea.y) * 0.5f;
			}
		}
		public float worldBottomBound
		{
			get
			{
				return transform.position.y - (realCameraArea.y + _cameraArea.y) * 0.5f;
			}
		}
		public float worldLeftBound
		{
			get
			{
				return transform.position.x - (realCameraArea.x + _cameraArea.x) * 0.5f;
			}
		}
		public float worldRightBound
		{
			get
			{
				return transform.position.x + (realCameraArea.x - _cameraArea.x) * 0.5f;
			}
		}

		public Vector2 realCameraArea
		{
			get { return _cameraArea * _size * resolutionCeof; }
		}

		public float size
		{
			get { return _size; }
			set { _size = value; }
		}

		public float resolutionCeof
		{
			get { return _resolutionCeof; }
		}

		[SerializeField]
		protected Camera _camera;
		[SerializeField]
		protected float _resolutionCeof;
		[SerializeField]
		protected Vector2 _cameraArea = new Vector2(15.0f, 5.0f);
		[SerializeField]
		protected float _size = 1.0f;	
	
	
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
			Gizmos.DrawWireCube(transform.position, realCameraArea);
		}

#endif

	}

}
