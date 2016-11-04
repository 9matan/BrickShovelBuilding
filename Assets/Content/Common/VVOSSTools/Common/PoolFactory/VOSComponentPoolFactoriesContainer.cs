using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IVOSFactoryTypable<TType>
{
	TType factoryType { get; }
}

public class VOSComponentIPoolFactoriesContainer<TType, TFactory, TComponent> : MonoBehaviour
	where TComponent : Component
	where TFactory : IVOSComponentPoolFactory<TComponent>, IVOSFactoryTypable<TType>
{

	public TFactory this[TType type]
	{
		get { return _factories[type]; }
	}

	protected Dictionary<TType, TFactory> _factories = new Dictionary<TType, TFactory>();
	


	public void Initialize()
	{
		_InitializeFactories();
	}

	protected void _InitializeFactories()
	{
		var list = gameObject.GetComponentsInChildren<TFactory>(true);
		for(int i = 0; i < list.Length; ++i)
		{
			list[i].Initialize();
			_factories.Add(list[i].factoryType, list[i]);
		}
	}




	public bool Contains(TType type)
	{
		return _factories.ContainsKey(type);
	}

	public TComponent Allocate(TType type)
	{
		return _factories[type].Allocate();
	}

	public void Free(TType type, TComponent component)
	{
		_factories[type].Free(component);
	}
	
}


public class VOSComponentPoolFactoriesContainer<TType, TFactory, TComponent> :
	VOSComponentIPoolFactoriesContainer<TType, TFactory, TComponent>
	where TComponent : Component
	where TFactory : VOSComponentPoolFactory<TComponent>, IVOSFactoryTypable<TType>
{
	
#if UNITY_EDITOR

	[ContextMenu("Add factory")]
	public void AddFactory()
	{
		var gobj = new GameObject("Factory");
		var factory = gobj.AddComponent<TFactory>();

		gobj.transform.SetParent(transform);
		gobj.transform.localPosition = Vector3.zero;
		gobj.transform.localRotation = Quaternion.identity;

		factory.Build();
	}

#endif

}
