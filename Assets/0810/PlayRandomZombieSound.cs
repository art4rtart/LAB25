using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomZombieSound : MonoBehaviour
{
	AudioSource audioSource;
	public AudioClip[] zombieSoundClips;
	//public GameObject zombieModel;
	//public GameObject ZombieControlModel;

	[Range(0,100)]
	public float percentage;

	void Awake()
	{
		var listener = GameObject.FindObjectOfType<AudioListener>();
		Debug.Log(listener.transform.name);

		audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		float value = Random.Range(0, 100);
		if (value < 100 - percentage) this.enabled = false;
		else StartCoroutine(PlayRandomSound());
	}

	//void Update()
	//{
	//	this.transform.position = zombieModel.transform.localPosition + ZombieControlModel.transform.localPosition;
	//}

	IEnumerator PlayRandomSound()
	{
		float waitUntilNextSoundPlay = 0;
		float soundPlayWaitTime = Random.Range(0f, 1f);

		yield return new WaitForSeconds(soundPlayWaitTime);

		while (this.gameObject.activeSelf)
		{
			waitUntilNextSoundPlay = Random.Range(0f, 2f);
			if (!audioSource.isPlaying)
			{
				audioSource.clip = zombieSoundClips[Random.Range(0, zombieSoundClips.Length)];
				audioSource.Play();
			}

			//if (!zombieModel.activeSelf)
			//	break;

			yield return new WaitForSeconds(waitUntilNextSoundPlay);
		}
		//audioSource.loop = false;
		//while (audioSource.isPlaying) yield return null;
		//audioSource.enabled = false;
	}
}
