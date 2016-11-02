using UnityEngine;
using System.Collections;

public interface IVOSComponentCreator
{
	T Create<T>();
}

public class VOSComponentCreator : MonoBehaviour,
	IVOSComponentCreator,
	IVOSBuilder
{

	public Vector3 instPos = Vector3.zero;
	public Vector3 instRot = Vector3.zero;
	public GameObject prefab = null;

	public T Create<T>()
	{
		return (Instantiate(prefab, instPos, Quaternion.Euler(instRot)) as GameObject).GetComponent<T>();
	}

#if UNITY_EDITOR

	[ContextMenu("Build")]
	public void Build()
	{
		if(prefab == null)
			Debug.Log("Set prefab to creator!");
	}

#else

	public void Build(){}

#endif

}
