using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBreak : MonoBehaviour
{
	public GameObject brokenGlasses;

	public void Break()
	{
		brokenGlasses.transform.SetParent(null);
		brokenGlasses.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
