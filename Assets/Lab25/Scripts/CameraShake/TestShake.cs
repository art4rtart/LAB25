using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShake : MonoBehaviour
{
	public CameraShake.Properties testProperties;

	public static TestShake Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<TestShake>();
			return instance;
		}
	}
	private static TestShake instance;

	public void Shake()
	{
		FindObjectOfType<CameraShake>().StartShake(testProperties);
	}
}