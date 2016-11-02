using UnityEngine;
using System.Collections;
using System.Collections.Generic;	

public interface IVOSPoolFactory<T>
{
	int freeNumber { get; }
	bool isInit { get; }

	void Initialize();
	T Allocate();
	void CreateItems(int number);
	void Free(T item);
}

public abstract class VOSPoolFactory<T> : MonoBehaviour,
	IVOSPoolFactory<T>
{

	public int freeNumber { get { return _freeItems.Count; } }
	public bool isInit { get; protected set; }

	[SerializeField]
	protected int _startItemsNumber;

	protected Dictionary<int, T> _freeItems = new Dictionary<int, T>();

	public virtual void Initialize()
	{
		_InitializeItems();
		_InitializeStartNumber();

		isInit = true;
	}

	protected void _InitializeItems()
	{
		_freeItems.Clear();
	}

	protected void _InitializeStartNumber()
	{
		CreateItems(_startItemsNumber);
	}

	//
	// < Allocate >
	//

	protected abstract T _Create();

	public virtual T Allocate()
	{
		var item = _AllocateItem();

		_OnAllocated(item);

		return item;
	}

	protected T _AllocateItem()
	{
		if (freeNumber == 0)
			_CreateItem();

		var iter = _freeItems.GetEnumerator();
		iter.MoveNext();

		var hash = iter.Current.Key;
		var item = iter.Current.Value;
		_freeItems.Remove(hash);

		return item;
	}

	public void CreateItems(int number)
	{
		for (int i = 0; i < number; ++i)
			_CreateItem();
	}

	protected void _CreateItem()
	{
		_FreeItem(
			_Create());
	}

	//
	// </ Allocate >
	//

	//
	// < Free >
	//

	public virtual void Free(T item)
	{
		_OnRelease(item);

		_FreeItem(item);

		_OnReleased(item);
	}

	protected virtual void _FreeItem(T item)
	{
		_freeItems.Add(
			_GetHashCode(item), item);
	}

	protected int _GetHashCode(T item)
	{
		return item.GetHashCode();
	}

	//
	// </ Free >
	//

	//
	// < Events >
	//

	protected void _OnRelease(T item)
	{

	}

	protected void _OnReleased(T item)
	{

	}

	protected void _OnAllocated(T item)
	{

	}

	//
	// </ Events >
	//

}
