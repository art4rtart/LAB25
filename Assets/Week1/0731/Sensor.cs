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
	
	void Awake()
	{
		audiosource = GetComponent<AudioSource>();
	}

	void Update()
	{
		//if(Input.GetKeyDown(KeyCode.B))
		//{
		//	ItemManager.hasCardKey = !ItemManager.hasCardKey;
		//}
	}

	public void OpenGate()
	{
		if(!ItemManager.hasCardKey){
			audiosource.clip = sounds[0];
			if(!audiosource.isPlaying) audiosource.Play();
			return;
		}

		else{
			audiosource.clip = sounds[1];
		}

		if (!audiosource.isPlaying) audiosource.Play();

		if (isOpend) return;
		automaticDoor.OpenDoor();
		isOpend = true;
	}
}
