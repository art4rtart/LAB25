using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
	public AutomaticDoor automaticDoor;
	[HideInInspector]
	public bool isOpend = false;

	public void OpenGate()
	{
		if (isOpend) return;
		automaticDoor.OpenDoor();
		isOpend = true;
	}
}
