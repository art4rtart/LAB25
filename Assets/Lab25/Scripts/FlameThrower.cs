using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
	private bool playAura = false;
	public ParticleSystem particleObject;

	void Start()
	{
		particleObject.Stop();
	}

	public void UseFlameThrower()
	{
        /*if (!particleObject.isPlaying)*/
        particleObject.Play();
	}

	public void StopFlameThrower()
	{
		if (particleObject.isPlaying) particleObject.Stop();
	}
}
