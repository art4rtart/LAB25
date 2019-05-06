using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialValue : MonoBehaviour
{
	public Material material;
	public Color tColor;

	IEnumerator TransParent;
	IEnumerator Default;

	void Start()
	{
		material.color = tColor;
	}

	public void TransparentTrigger()
    {
		StopAllCoroutines();
		TransParent = ChangeToTransparent();
		StartCoroutine(TransParent);
    }

	public void DefaultTrigger()
	{
		StopAllCoroutines();
		Default = ChangeToDefault();
		StartCoroutine(Default);
	}

	IEnumerator ChangeToTransparent()
	{
		Color a = material.color;

		while (a.a >= 0)
		{
			a.a -= 2f * Time.deltaTime;
			material.color = a;
			yield return null;
		}

		StopCoroutine(TransParent);
	}

	IEnumerator ChangeToDefault()
	{
		Color a = material.color;

		while (a.a <= 1)
		{
			a.a += 2f * Time.deltaTime;
			material.color = a;
			yield return null;
		}

		StopCoroutine(Default);
	}
}
