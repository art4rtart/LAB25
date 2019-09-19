using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
	public float smoothness = 0.05f;
	public float duration = 1f;
	public float increaseValue = 0f;
	public Renderer[] rend;

	Color currentColor = Color.black;
	Color targetColor = Color.black;

	void Start()
    {
		targetColor = rend[0].material.GetColor("_EmissionColor");
		for(int i = 0; i < rend.Length; i ++) rend[i].material.SetColor("_EmissionColor", Color.black);

		StartCoroutine(waitForBlackOut());
    }

	IEnumerator waitForBlackOut()
	{
		while (!BlackOut.Instance.blackOutStart) yield return null;
		StartCoroutine(LerpEmissionColor(currentColor, targetColor));
	}

	IEnumerator LerpEmissionColor(Color current, Color target)
	{
		yield return new WaitForSeconds(2f);

		float progress = 0;
		float increment = smoothness / duration;
		float emissionScale = 0;

		while (progress < 1)
		{
			currentColor = Color.Lerp(current, target, progress);

			for (int i = 0; i < rend.Length; i++) {
				rend[i].material.SetColor("_EmissionColor", currentColor);
				rend[i].material.SetFloat("_EmissionScaleUI", emissionScale);
			}

			progress += increment;
			emissionScale += increaseValue;

			yield return new WaitForSeconds(smoothness);
		}

		yield return null;
	}
}
