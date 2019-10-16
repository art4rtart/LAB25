using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
	public MissionScripts missionScript;
	AudioSource audioSource;
	bool isStartedRadio = false;
	Renderer rend;
	public GameObject scriptTrigger;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		rend = GetComponent<Renderer>();
		StartCoroutine(StartRadio());
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			isStartedRadio = true;
		}
	}

	IEnumerator StartRadio()
	{
		float emissionColor = 0;

		while(!isStartedRadio) { yield return null; }

		yield return new WaitForSeconds(5f);
		audioSource.Play();

		Color baseColor = rend.material.color;
		while (emissionColor <= 4f)
		{
			emissionColor += Time.deltaTime * 2f;
			Color finalColor = baseColor * emissionColor;
			rend.material.SetColor("_EmissionColor", finalColor);
			yield return null;
		}

		yield return new WaitForSeconds(1f);
	}
}
