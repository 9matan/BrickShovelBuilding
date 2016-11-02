using UnityEngine;
using System.Collections;

public interface IVOSBuilder
{
	void Build();
}

public static class VOSBuilder
{

	public static T Build<T>(this GameObject parent, string name)
		where T : Component, IVOSBuilder
	{
		GameObject go = null;
		var child = parent.transform.FindChild(name);

		if (child != null)
			go = child.gameObject;
		else
		{
			go = new GameObject(name);
			go.transform.parent = parent.transform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
		}		

		var c = go.GetAddComponent<T>();
		c.Build();
		return c;
	}

	public static T Build<T>(this GameObject parent, T inst, string name)
		where T : Component, IVOSBuilder
	{
		if (inst != null)
		{
			inst.Build();
		}
		else
		{
			inst = Build<T>(parent, name);
		}

		return inst;
	}


	public static T BuildAtObject<T>(this GameObject go)
		where T : Component, IVOSBuilder
	{
		var c = go.GetAddComponent<T>();
		c.Build();
		return c;
	}

	public static T BuildAtObject<T>(this GameObject go, T inst)
		where T : Component, IVOSBuilder
	{
		if (inst != null)
			inst.Build();
		else
			inst = BuildAtObject<T>(go);

		return inst;
	}

}