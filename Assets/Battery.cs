using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
	public BlackOut blackOut;

	void OnDisable()
	{
		blackOut.LightControl();
	}
}
