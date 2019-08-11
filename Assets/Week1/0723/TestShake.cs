using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShake : MonoBehaviour
{
	public CameraShake.Properties testProperties;

	public void Shake()
	{
		FindObjectOfType<CameraShake>().StartShake(testProperties);
	}
}