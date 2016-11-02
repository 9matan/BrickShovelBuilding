using UnityEngine;
using System.Collections;

public interface IVOSCreator<T>
{
	T Create();
}

public interface IVOSGOCreator :
	IVOSCreator<GameObject>
{

}

public class VOSGOCreator : MonoBehaviour,
	IVOSGOCreator
{

	public Vector3 instPos = Vector3.zero;
	public Vector3 instRot = Vector3.zero;
	public GameObject prefab = null;

	public GameObject Create()
	{
		return (Instantiate(prefab, instPos, Quaternion.Euler(instRot)) as GameObject);
	}

}
