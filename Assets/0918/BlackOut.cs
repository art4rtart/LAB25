using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOut : MonoBehaviour
{
	AudioSource audioSource;
	Color currentColor;
	Color targetColor = Color.black;
	float duration = 2f;
	float smoothness = 0.05f;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		StartCoroutine(LightOff());
	}

	public IEnumerator LightOff()
	{
		float value = 0;
		float count = 0;

		yield return new WaitForSeconds(20f);

		while(count < 10)
		{
			value = Random.Range(0.5f, 1f);
			currentColor = new Color(value, value, value, 1f);
			RenderSettings.ambientLight = currentColor;
			count++;
			yield return new WaitForSeconds(0.05f);
		}
		yield return StartCoroutine(LerpColor(currentColor, targetColor));
	}

	IEnumerator LerpColor(Color current, Color target)
	{
		float progress = 0;
		float increment = smoothness / duration;

		audioSource.Play();

		while (progress < 1)
		{
			currentColor = Color.Lerp(current, target, progress);
			RenderSettings.ambientLight = currentColor;
			progress += increment;
			yield return new WaitForSeconds(smoothness);
		}

		yield return null;
	}
}

