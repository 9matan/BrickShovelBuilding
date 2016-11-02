using UnityEngine;
using System.Collections;

public interface IVOSComponentPoolFactory<I, C> :
	IVOSPoolFactory<I>
	where I : IVOSTransformable
	where C : Component, I
{
	IVOSComponentCreator creator { get; }
}

public class VOSComponentPoolFactory<I, C> : VOSPoolFactory<I>,
	IVOSComponentPoolFactory<I, C>,
	IVOSBuilder
	where I : IVOSTransformable
	where C : Component, I
{

	public IVOSComponentCreator creator { get { return _creatorImplementer; } }

	public bool hideOnFree = true;
	public bool showOnAllocate = true;

	[SerializeField]
	protected VOSComponentCreator _creatorImplementer;

	protected override I _Create()
	{
		return creator.Create<C>();
	}

	public override I Allocate()
	{
		var item = base.Allocate();

		if (showOnAllocate)
			item.transform.gameObject.Show();

		return item;
	}

	protected override void _FreeItem(I item)
	{
		item.transform.parent = transform;

		if (hideOnFree)
			item.transform.gameObject.Hide();

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
