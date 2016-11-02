using UnityEngine;
using System.Collections;

public interface IVOSGOPoolFactory :
	IVOSPoolFactory<GameObject>
{

}

public class VOSGOPoolFactory : VOSPoolFactory<GameObject>
{
	[SerializeField]
	protected VOSGOCreator _creator;

	protected override GameObject _Create()
	{
		return _creator.Create();
	}
}
