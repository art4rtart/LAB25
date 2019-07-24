using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompanyLogo : MonoBehaviour
{
	public Image fadeImage;
	public GameObject blurRenderer;

    void Start()
    {
		blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", 0);
		Invoke("FadeOut", 3.5f);
	}

	void FadeOut()
	{
		StartCoroutine(FadeOutScene());
	}

	IEnumerator FadeOutScene()
	{
		float alpha = 0;
		float fadeSpeed = 0.005f;

		while(fadeImage.color.a <= 1)
		{
			alpha += fadeSpeed;
			fadeImage.color = new Color(0, 0, 0, alpha);
			blurRenderer.GetComponent<Renderer>().sharedMaterial.SetFloat("_Size", alpha * 200f);
			yield return null;
		}
	}
}
