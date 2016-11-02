using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class VOSSerializableDictionary<TKey, TValue> : 
	IDictionary<TKey, TValue>,
	ISerializationCallbackReceiver
{
	[SerializeField]//, HideInInspector]
	private List<TKey> _keys;
	[SerializeField]//, HideInInspector]
	private List<TValue> _values;

	protected Dictionary<TKey, TValue> _container = new Dictionary<TKey, TValue>();

	
	public ICollection<TKey> Keys
	{
		get
		{
			return _container.Keys;
		}
	}

	public ICollection<TValue> Values
	{
		get
		{
			return _container.Values;
		}
	}

	public int Count
	{
		get
		{
			return _container.Count;
		}
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
	{
		get
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_container).IsReadOnly;
		}
	}

	public TValue this[TKey key]
	{
		get
		{
			return _container[key];
		}

		set
		{
			_container[key] = value;
		}
	}

	// Before the serialization we fill these lists
	public void OnBeforeSerialize()
	{
		_keys = new List<TKey>(this.Count);
		_values = new List<TValue>(this.Count);
		foreach (var kvp in this)
		{
			_keys.Add(kvp.Key);
			_values.Add(kvp.Value);
		}
	}

	// After the serialization we create the dictionary from the two lists
	public void OnAfterDeserialize()
	{
		Clear();
		for (int i = 0; i != Mathf.Min(_keys.Count, _values.Count); i++)
		{
			Add(_keys[i], _values[i]);
		}
	}

	public void Add(TKey key, TValue value)
	{
		_container.Add(key, value);
	}

	public bool ContainsKey(TKey key)
	{
	//	return _keys.Contains(key);
		return _container.ContainsKey(key);
	}

	//
	// < O(n)>
	//
	public bool Remove(TKey key)
	{
		return _container.Remove(key);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		return _container.TryGetValue(key, out value);
	}

	void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)_container).Add(item);
	}

	public void Clear()
	{
		_container.Clear();
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
	{
		return ((ICollection<KeyValuePair<TKey, TValue>>)_container).Contains(item);
	}

	void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)_container).CopyTo(array, arrayIndex);
	}

	bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
	{
		return ((ICollection<KeyValuePair<TKey, TValue>>)_container).Remove(item);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return _container.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)_container).GetEnumerator();
	}
}