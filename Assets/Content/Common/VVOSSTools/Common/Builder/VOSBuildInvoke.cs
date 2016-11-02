using UnityEngine;
using System.Collections;

public class VOSBuildInvoke : MonoBehaviour
{

#if UNITY_EDITOR

	[ContextMenu("Build")]
	public void Build()
	{
		_InvokeBuild(gameObject);
		Debug.Log("Build invoke: done!");
	}

	protected void _InvokeBuild(GameObject go)
	{
		var builders = go.GetComponents<IVOSBuilder>();

		for (int i = 0; i < builders.Length; ++i)
			builders[i].Build();

		var cnum = go.transform.childCount;
		for (int i = 0; i < cnum; ++i)
			_InvokeBuild(go.transform.GetChild(i).gameObject);
	}

#endif

}
