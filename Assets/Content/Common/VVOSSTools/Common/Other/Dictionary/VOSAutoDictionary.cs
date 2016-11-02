using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IVOSAutoDictionary<TKey, TValue>
{
	TValue this[TKey key] { get; }
	bool ContainsKey(TKey key);
}

public interface IVOSAutoDictionaryValue<TKey>
{
	TKey keyValue { get; }
}

[System.Serializable]
public class VOSAutoDictionary<TKey, TValue> : IVOSAutoDictionary<TKey, TValue>
	where TValue : IVOSAutoDictionaryValue<TKey>
{

	public TValue this[TKey key]
	{
		get
		{
			CheckSynchronize();
			return _dictionary[key];
		}
		set
		{
			CheckSynchronize();
			_AddToDictionary(key, value);
		}
	}

	public bool isEmpty { get { return _dictionary.Count == 0; } }
	public bool synchronize = false;

	[SerializeField]
	protected TValue[] _values;

	protected Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();



	public void CheckSynchronize()
	{
		if (synchronize || isEmpty)
		{
			Synchronize();
		}
	}

	public virtual void Synchronize()
	{
		SynchronizeDictionary();
	}

	public void SynchronizeDictionary()
	{
		if (_values == null) return;

		for(int i = 0; i < _values.Length; ++i)
		{
			_AddToDictionary(_values[i].keyValue, _values[i]);
		}
	}



	protected void _AddToDictionary(TKey key, TValue val)
	{
		if (!_dictionary.ContainsKey(key))
			_dictionary.Add(key, val);
		else
			_dictionary[key] = val;
	}

	public bool ContainsKey(TKey key)
	{
		CheckSynchronize();
		return _dictionary.ContainsKey(key);
	}

	

	public void Clear()
	{
		_dictionary.Clear();
	}

}
