using UnityEngine;
using System.Collections;

public static class VOSGameObjectExtention
{

	public static bool ExistChild<T>(this GameObject go, string name)
		where T : Component
	{
		var child = go.transform.FindChild(name);
		return (child != null) && (child.GetComponent<T>() != null);
	}

	public static GameObject FindOrCreateChild(this GameObject go, string name)
	{
		var child = go.transform.FindChild(name);
		if (child == null)
			return go.CreateChild(name);
		return child.gameObject;
	}

	public static GameObject CreateChild(this GameObject go, string name)
	{
		var child = new GameObject(name);
		child.transform.parent = go.transform;
		child.transform.localPosition = Vector3.zero;
		child.transform.localRotation = Quaternion.identity;
		return child;
	}

	public static T GetAddComponent<T>(this GameObject go) where T : Component
	{
		var comp = go.GetComponent<T>();
		if (comp == null)
			comp = go.AddComponent<T>();

		return comp;
	}

	public static void Show(this GameObject go)
	{
		go.SetActive(true);
	}

	public static void Hide(this GameObject go)
	{
		go.SetActive(false);
	}

	public static string ParentName(this GameObject go, int depth, bool useThisName)
	{
		var name = ParentName(go, depth);

		if (useThisName)
			name = string.Concat(name, go.name);

		return name;
	}

	public static string ParentName(this GameObject go, int depth)
	{
		if (depth <= 0 || go.transform.parent == null) return string.Empty;

		return string.Concat(
			ParentName(go.transform.parent.gameObject, depth - 1), go.transform.parent.name);
	}

}
