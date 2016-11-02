using UnityEngine;
using System.Collections;

public class VOSScreenNeverSleep : MonoBehaviour
{

	void Awake()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

}
