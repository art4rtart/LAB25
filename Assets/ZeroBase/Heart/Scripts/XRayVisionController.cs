using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRayVisionController : MonoBehaviour
{
	GameObject[] teamMates;

	void Start()
	{
		teamMates = GameObject.FindGameObjectsWithTag("Limpid");
	}

	public void changeToVisionMaterial()
	{
		for (int i = 0; i < teamMates.Length; i++)
		{
			teamMates[i].GetComponent<Limpid>().ChangeToVisionMaterial();
		}
	}

	public void changeToDefaultMaterial()
	{
		for (int i = 0; i < teamMates.Length; i++)
		{
			teamMates[i].GetComponent<Limpid>().ChangeToNormalMaterial();
		}
	}
}
