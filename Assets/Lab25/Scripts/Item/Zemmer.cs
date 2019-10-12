using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zemmer : MonoBehaviour
{
	public bool activate;
	public Image jammerIconImage;
	float defaultAlpha = 0f;
	float maxAlpha = 1f;

	void Awake()
	{
		if (jammerIconImage == null) return;
		defaultAlpha = jammerIconImage.color.a;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
			UseZemmer();
	}
	public void UseZemmer()
	{
		activate = !activate;
		StopAllCoroutines();
		StartCoroutine(ImageAlphaChange(activate));
	}

	IEnumerator ImageAlphaChange(bool activated)
	{
		float value = jammerIconImage.color.a;
		float fadeSpeed = 2f;

		while (true)
		{
			if (activated)
			{
				value = Mathf.Clamp(value += Time.deltaTime * fadeSpeed, defaultAlpha, maxAlpha);
			}

			else
			{
				value = Mathf.Clamp(value -= Time.deltaTime * fadeSpeed, defaultAlpha, maxAlpha);
			}

			jammerIconImage.color = new Color(jammerIconImage.color.r, jammerIconImage.color.g, jammerIconImage.color.b, value);

			yield return null;
		}
	}
}
