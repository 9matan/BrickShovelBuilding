using UnityEngine;
using System.Collections;

public interface IVOSTransformable
{
	Transform transform { get; }
}

public interface IVOSInitializable
{
	void Initialize();
}

public interface IVOSKeyable<TKey>
{
	TKey key { get; }
}

public interface IVOSRecordable
{
	void RecordObject(string undo);
}

public interface IVOSResetable
{
	void Reset();
}