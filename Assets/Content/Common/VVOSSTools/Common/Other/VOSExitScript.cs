using UnityEngine;
using System.Collections;

public class MSExitScript : MonoBehaviour
{
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
