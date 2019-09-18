using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
	AudioSource audiosource;
	public AudioClip[] sounds;

	public AutomaticDoor automaticDoor;
	[HideInInspector]
	public bool isOpend = false;

	public bool isMissionSensor;
	public bool isCollidingWithMoveableObject;

	void Awake()
	{
		audiosource = GetComponent<AudioSource>();
	}

	public void OpenGate()
	{
		if (isMissionSensor)
		{
			audiosource.clip = sounds[1];
			if (!audiosource.isPlaying) audiosource.Play();
			if (isOpend) return;
			automaticDoor.OpenDoor();
			isOpend = true;
		}

		else
		{
			if (!ItemManager.hasCardKey)
			{
				audiosource.clip = sounds[0];
				if (!audiosource.isPlaying) audiosource.Play();
				return;
			}

			else
			{
				audiosource.clip = sounds[1];
			}

			if (!audiosource.isPlaying) audiosource.Play();

			if (isOpend) return;
			automaticDoor.OpenDoor();
			isOpend = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Moveable") && isMissionSensor)
		{
			StartCoroutine(Checker());
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Moveable") && isMissionSensor)
		{
			StartCoroutine(automaticDoor.CloseDoor());
		}
	}


	IEnumerator Checker()
	{
		float time = 0;
		isCollidingWithMoveableObject = true;
		while (true)
		{
			time += Time.deltaTime;

			if (time > 0.5f)
			{
				break;
			}

			yield return null;
		}
		OpenGate();
	}
}
