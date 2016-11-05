using UnityEngine;
using System.Collections;

public interface IVOSManipulator
{
	Vector2 position { get; }
	bool isPressed { get; }

	event VOSManipulator.OnReleased onReleased;
	event VOSManipulator.OnPressed onPressed;

	Vector3 ToWorldPosition(Camera camera);

	void Clear();
}

public class VOSManipulator : MonoBehaviour,
	IVOSManipulator,
	IVOSBuilder
{

	public delegate void OnReleased(IVOSManipulator control);
	public delegate void OnPressed(IVOSManipulator control);

	public event OnReleased onReleased = delegate { };
	public event OnPressed onPressed = delegate { };

	public Vector2 position { get; protected set; }
	public bool isPressed { get; protected set; }

	public Vector3 ToWorldPosition(Camera camera)
	{
		var pos = position;
//		pos.y -= Screen.height;
		return camera.ScreenToWorldPoint(pos);
	}

	protected void Update()
	{
		var currentPress = _IsPressed();

		if (currentPress)
		{
			position = _GetPosition();
		}

		if (isPressed && !currentPress)
		{
			_OnReleased();
		}
		else if(!isPressed && currentPress)
		{
			_OnPressed();
		}		
		
		isPressed = currentPress;
	}

	protected void _OnReleased()
	{
		onReleased(this);
	}

	protected void _OnPressed()
	{
		onPressed(this);
	}

	protected Vector2 _GetPosition()
	{
#if UNITY_EDITOR || UNITY_STANDALONE		
		return Input.mousePosition;
#elif UNITY_IOS || UNITY_ANDROID
		return Input.GetTouch(0).position;
#endif
	}

	protected bool _IsPressed()
	{
#if UNITY_EDITOR || UNITY_STANDALONE		
		return Input.GetMouseButton(0);
#elif UNITY_IOS || UNITY_ANDROID
		return Input.touchCount > 0;
#endif
	}


	public void Clear()
	{
		onPressed = delegate { };
		onReleased = delegate { };
	}


#if UNITY_EDITOR

	[ContextMenu("Build")]
	public void Build()
	{

	}

#else

	public void Build(){}

#endif

}
