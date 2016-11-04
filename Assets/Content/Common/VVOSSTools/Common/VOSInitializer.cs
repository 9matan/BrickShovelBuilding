using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VOSInitializer : MonoBehaviour,
	IVOSInitializable
{

	public bool initAtAwake = false;
	public bool initChilds = true;
	public bool initList = true;

	[SerializeField]
	protected List<GameObject> _initializeList = new List<GameObject>();

	protected void Awake()
	{
		if (initAtAwake)
			Initialize();
	}

	public void Initialize()
	{
		if (initList)
			_InitializeList();

		if (initChilds)
			_InitializeChilds();
	}
	
	protected void _InitializeChilds()
	{
		var initializers = gameObject.GetComponentsInChildren<IVOSInitializable>(true);

		for (int i = 0; i < initializers.Length; ++i)
		{
			if (initializers[i] == this || Equals(initializers[i])) continue;

			initializers[i].Initialize();
		}
	}	

	protected void _InitializeList()
	{
		for(int i = 0; i < _initializeList.Count; ++i)
		{
			if (_initializeList[i] == this.gameObject) continue;

			var initializers = _initializeList[i].GetComponents<IVOSInitializable>();
			for (int j = 0; j < initializers.Length; ++j)
				initializers.Initialize();
		}
	}

}
