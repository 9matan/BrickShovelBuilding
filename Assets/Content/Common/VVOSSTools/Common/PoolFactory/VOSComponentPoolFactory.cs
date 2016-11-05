using UnityEngine;
using System.Collections;

public interface IVOSComponentPoolFactory<C> :
	IVOSPoolFactory<C>
	where C : Component
{
	IVOSComponentCreator creator { get; }
}

public class VOSComponentPoolFactory<C> : VOSPoolFactory<C>,
	IVOSComponentPoolFactory<C>,
	IVOSBuilder
	where C : Component
{

	public IVOSComponentCreator creator { get { return _creatorImplementer; } }

	public bool hideOnFree = true;
	public bool showOnAllocate = true;

	[SerializeField]
	protected VOSComponentCreator _creatorImplementer;

	protected override C _Create()
	{
		return creator.Create<C>();
	}

	public override C Allocate()
	{
		var item = base.Allocate();

		if (showOnAllocate)
			item.transform.gameObject.Show();

		return item;
	}

	protected override void _FreeItem(C item)
	{
		item.transform.parent = transform;
		item.transform.localPosition = Vector3.zero;

		if (hideOnFree)
			item.gameObject.Hide();

		base._FreeItem(item);
	}

#if UNITY_EDITOR

	[ContextMenu("Build")]
	public void Build()
	{
		_BuildCreator();
	}

	protected void _BuildCreator()
	{
		if (_creatorImplementer != null) return;

		_creatorImplementer = gameObject.BuildAtObject(_creatorImplementer);
	}

#else

	public void Build(){}

#endif
}
