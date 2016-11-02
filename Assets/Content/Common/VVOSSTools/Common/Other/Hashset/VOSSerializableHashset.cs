using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class VOSSerializableHashset<TValue> :
	ICollection<TValue>,
	IEnumerable<TValue>,
	ISerializationCallbackReceiver
{

	[SerializeField]
	protected HashSet<TValue> _hash = new HashSet<TValue>();

	[SerializeField]
	protected List<TValue> _values = new List<TValue>();

	public int Count
	{
		get
		{
			return _hash.Count;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<TValue>)_hash).IsReadOnly;
		}
	}

	public void Add(TValue item)
	{
		_hash.Add(item);
	}

	public void Clear()
	{
		_hash.Clear();
	}

	public bool Contains(TValue item)
	{
		return _hash.Contains(item);
	}

	public void CopyTo(TValue[] array, int arrayIndex)
	{
		_hash.CopyTo(array, arrayIndex);
	}

	public IEnumerator<TValue> GetEnumerator()
	{
		return _hash.GetEnumerator();
	}

	public bool Remove(TValue item)
	{
		return _hash.Remove(item);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)_hash).GetEnumerator();
	}

	public void UnionWith(IEnumerable<TValue> other)
	{
		_hash.UnionWith(other);
	}



	public void OnAfterDeserialize()
	{
		_hash.Clear();
		for (int i = 0; i < _values.Count; ++i)
		{
			_hash.Add(_values[i]);
		}
	}

	public void OnBeforeSerialize()
	{
		_values.Clear();
		foreach (var item in _hash)
		{
			_values.Add(item);
		}
	}

	
}
