using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlackOut : MonoBehaviour
{
	public TextMeshProUGUI locationNameMain;
	public TextMeshProUGUI locationNameSecurity;

	AudioSource audioSource;
	Color currentColor;
	Color targetColor = Color.black;
	float duration = 2f;
	float smoothness = 0.05f;
	public bool blackOutStart = false;

	[Header("Security Room Settings")]
	public Light roomLight;
	public TextMeshProUGUI Description;
	public GameObject Button3DUI;
	public GameObject gateSensor;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	public void LightControl()
	{
		StartCoroutine(LightOff());
	}

	public static BlackOut Instance
	{
		get
		{
			if (instance != null)
				return instance;
			instance = FindObjectOfType<BlackOut>();
			return instance;
		}
	}
	private static BlackOut instance;

	public IEnumerator LightSecurityRoom()
	{
		float alpha = 0.2f;

		while (locationNameSecurity.color.a < 1)
		{
			alpha += .05f;
			locationNameSecurity.color = new Color(locationNameSecurity.color.r, locationNameSecurity.color.g, locationNameSecurity.color.b, alpha);
			yield return new WaitForSeconds(smoothness);
		}
		Button3DUI.SetActive(true);
		roomLight.enabled = true;
		Description.text = "Service Available.";
		gateSensor.tag = "MissionObject";
		yield return null;
	}

	public IEnumerator LightOff()
	{
		float value = 0;
		float count = 0;

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
		float alpha = 1f;

		audioSource.Play();
		blackOutStart = true;
		while (progress < 1)
		{
			locationNameMain.color = new Color(locationNameMain.color.r, locationNameMain.color.g, locationNameMain.color.b, alpha);
			currentColor = Color.Lerp(current, target, progress);
			RenderSettings.ambientLight = currentColor;
			progress += increment;
			alpha -= .02f;
			yield return new WaitForSeconds(smoothness);
		}

		yield return null;
	}
}

