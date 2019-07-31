using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShake : MonoBehaviour
{
	public CameraShake.Properties testProperties;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			FindObjectOfType<CameraShake>().StartShake(testProperties);
		}
	}

	public void Shake()
	{
		FindObjectOfType<CameraShake>().StartShake(testProperties);
	}
}