using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour
{
	AudioManager audioManager;
	void Awake()
	{
		audioManager = FindObjectOfType<AudioManager>();
	}

	public void DissolveSoundTrigger()
	{
		audioManager.Play("DissolveSound");
	}
}
