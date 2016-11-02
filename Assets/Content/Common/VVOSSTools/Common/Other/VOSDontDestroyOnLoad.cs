using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VOSDontDestroyOnLoad : MonoBehaviour
{

	static HashSet<string> _instances = new HashSet<string>();

	public bool isChecked { get; protected set; }

	[Header("Singleton")]
	[SerializeField]
	protected bool		_singleton = true;
	[SerializeField]
	protected bool		_checkAtAwake = true;
	[SerializeField]
	protected string 	_id = string.Empty;

	protected virtual void Awake()
	{
		if (_checkAtAwake)
			CheckObject();
	}

	public virtual bool CheckObject()
	{
		bool result = true;

		if (_singleton)
		{
			if (DoesExist(_id))
			{
				Destroy(gameObject);
				result = false;
			}
			else
				_AddHash();
		}

		DontDestroyOnLoad(gameObject);		
		isChecked = true;

		return result;
	}

	protected virtual void OnDestroy()
	{
		if(_singleton)
			this._RemoveHash ();
	}


	protected virtual void _AddHash()
	{
		_instances.Add (_id);
	}

	protected virtual void _RemoveHash()
	{
		_instances.Remove (_id);
	}
	


	static public bool DoesExist(string __id)
	{
		return _instances.Contains(__id);
	}

#if UNITY_EDITOR

	public string id
	{
		get { return _id; }
		set { _id = value; }
	}

#endif

}
