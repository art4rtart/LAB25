using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIActive : MonoBehaviour
{
	public GameObject ItemUI;
	public GameObject oldCrossHair;

	void Start()
    {
		Invoke("SetActiveTrue", .5f);
    }

	void SetActiveTrue()
	{
		oldCrossHair.SetActive(false);
		ItemUI.SetActive(true);
	}
}
